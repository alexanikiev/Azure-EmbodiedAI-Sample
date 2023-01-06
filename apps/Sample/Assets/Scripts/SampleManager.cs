using UnityEngine;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Xml.Linq;
using UnityEngine.Analytics;
using UnityEngine.TextCore.Text;
using System.Runtime.InteropServices.ComTypes;
using System.Collections;
using static UnityEngine.AudioClip;
using UnityEngine.Networking;

namespace AzureEmbodiedAISamples
{
    public class SampleManager : MonoBehaviour
    {
        private SpeechConfig TTSConfig;
        private SpeechConfig STTConfig;
        private AudioConfig AudioMicrophoneConfig;
        private AudioConfig AudioSpeakerConfig; // SpeakAsyncSystem
        private AudioSource AudioSource; // SpeakAsyncUnity
        private int SampleRate = 48000; // SpeakAsyncUnity // 48KHz

        public SpeechSynthesizer SpeechSynthesizer { get; private set; }
        public SpeechRecognizer SpeechRecognizer { get; private set; }
        public int recognizerInitialSilenceTimeoutMs = 30000; // 30 secs
        public int recognizerSegmentationSilenceTimeoutMs = 1500; // 1.5 secs

        public bool useBodyTracking;
        public BackgroundData m_lastFrameData = new BackgroundData();
        private SkeletalTrackingProvider m_skeletalTrackingProvider;
        private bool bodyTrackingResult = false;

        private SynchronizationContext synchronizationContext;
        private CancellationTokenSource cancellationToken;

        private void Awake()
        {
            synchronizationContext = SynchronizationContext.Current;
        }

        private void OnEnable()
        {
            cancellationToken = new CancellationTokenSource();
        }

        void Start()
        {
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), Application.productName + ".auth");
            string jsonString = File.ReadAllText(filePath);
            SampleConfig SampleConfig = JsonUtility.FromJson<SampleConfig>(jsonString);

            TTSConfig = SpeechConfig.FromSubscription(SampleConfig.TTSSubscriptionKey, SampleConfig.TTSRegion);
            TTSConfig.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Raw48Khz16BitMonoPcm); // 48KHz
            TTSConfig.SpeechSynthesisVoiceName = "en-US-JacobNeural";
            //AudioSpeakerConfig = AudioConfig.FromDefaultSpeakerOutput(); // SpeakAsyncSystem
            //SpeechSynthesizer = new SpeechSynthesizer(TTSConfig, AudioSpeakerConfig); // SpeakAsyncSystem
            SpeechSynthesizer = new SpeechSynthesizer(TTSConfig, null); // SpeakAsyncUnity

            SpeechSynthesizer.SynthesisStarted += SpeechSynthesizer_SynthesisStarted;
            SpeechSynthesizer.Synthesizing += SpeechSynthesizer_Synthesizing;
            SpeechSynthesizer.SynthesisCompleted += SpeechSynthesizer_SynthesisCompleted;
            SpeechSynthesizer.SynthesisCanceled += SpeechSynthesizer_SynthesisCanceled;
            SpeechSynthesizer.VisemeReceived += SpeechSynthesizer_VisemeReceived;
            SpeechSynthesizer.BookmarkReached += SpeechSynthesizer_BookmarkReached;

            STTConfig = SpeechConfig.FromSubscription(SampleConfig.STTSubscriptionKey, SampleConfig.STTRegion);
            STTConfig.SetProperty(PropertyId.SpeechServiceConnection_InitialSilenceTimeoutMs, recognizerInitialSilenceTimeoutMs.ToString());
            STTConfig.SetProperty(PropertyId.Speech_SegmentationSilenceTimeoutMs, recognizerSegmentationSilenceTimeoutMs.ToString());
            AudioMicrophoneConfig = AudioConfig.FromDefaultMicrophoneInput();
            SpeechRecognizer = new SpeechRecognizer(STTConfig, AudioMicrophoneConfig);

            SpeechRecognizer.SpeechStartDetected += SpeechRecognizer_SpeechStartDetected;
            SpeechRecognizer.Recognized += SpeechRecognizer_Recognized;
            SpeechRecognizer.SpeechEndDetected += SpeechRecognizer_SpeechEndDetected;

            if (useBodyTracking)
            {
                const int TRACKER_ID = 0;
                m_skeletalTrackingProvider = new SkeletalTrackingProvider(TRACKER_ID);
            }

            AudioSource = GetComponent<AudioSource>(); // SpeakAsyncUnity
        }

        void Update()
        {
            UpdateBodyTracking();
        }

        #region Speech SDK
        private string GenerateSsml(string inputText)
        {
            var ssml = new XDocument(
                new XElement("speak",
                    new XAttribute("version", "1.0"),
                    new XAttribute(XNamespace.Xml + "lang", "en-US"),
                    new XElement("voice",
                        new XAttribute(XNamespace.Xml + "lang", "en-US"),
                        new XAttribute(XNamespace.Xml + "gender", "Male"),
                        new XAttribute("name", "en-US-JacobNeural"),
                        new XAttribute("style", "hopeful"),
                        new XElement("prosody",
                            new XAttribute(XNamespace.Xml + "rate", "+10.00%"),
                            new XAttribute(XNamespace.Xml + "pitch", "-5.00%"),
                            new XAttribute(XNamespace.Xml + "volume", "+5.00%"),
                            new XAttribute(XNamespace.Xml + "contour", "(5%, -61%) (48%, -6%)"),
                            inputText))));

            return ssml.ToString();
        }

        public async Task<bool> SpeakAsyncSystem(string inputText)
        {
            await SpeechSynthesizer.SpeakSsmlAsync(GenerateSsml(inputText)).ConfigureAwait(false);
            return true;
        }

        public async Task<bool> SpeakAsyncUnity(string inputText)
        { 
            // Reference:
            // https://github.com/Azure-Samples/cognitive-services-speech-sdk/blob/master/quickstart/csharp/unity/text-to-speech/Assets/Scripts/HelloWorld.cs

            var result = await SpeechSynthesizer.SpeakSsmlAsync(GenerateSsml(inputText));

            // Native playback is not supported on Unity yet (currently only supported on Windows/Linux Desktop).
            // Use the Unity API to play audio here as a short term solution.
            // Native playback support will be added in the future release.
            var audioDataStream = AudioDataStream.FromResult(result);
            var isFirstAudioChunk = true;
            var audioClip = AudioClip.Create(
                "Speech",
                SampleRate * 600, // Can speak 10mins audio as maximum
                1,
                SampleRate,
                true,
                (float[] audioChunk) =>
                {
                    var chunkSize = audioChunk.Length;
                    var audioChunkBytes = new byte[chunkSize * 2];
                    var readBytes = audioDataStream.ReadData(audioChunkBytes);
                    if (isFirstAudioChunk && readBytes > 0)
                    {
                        isFirstAudioChunk = false;
                    }

                    for (int i = 0; i < chunkSize; ++i)
                    {
                        if (i < readBytes / 2)
                        {
                            audioChunk[i] = (short)(audioChunkBytes[i * 2 + 1] << 8 | audioChunkBytes[i * 2]) / 32768.0F;
                        }
                        else
                        {
                            audioChunk[i] = 0.0f;
                        }
                    }

                    if (readBytes == 0)
                    {
                        synchronizationContext.Post(_ => { 
                            AudioSource.clip = null;  
                            AudioSource.Stop(); 
                        }, null);
                    }
                });

            AudioSource.clip = audioClip;
            AudioSource.Play();

            while (AudioSource.isPlaying)
            {
                await Task.Delay(100);
            }

            return true;
        }

        public async Task<string> ListenAsync()
        {
            var result = await SpeechRecognizer.RecognizeOnceAsync().ConfigureAwait(false);
            return result.Text;
        }
        #endregion

        #region BodyTracking SDK
        private void UpdateBodyTracking()
        {
            if (useBodyTracking && m_skeletalTrackingProvider != null && m_skeletalTrackingProvider.IsRunning)
            {
                if (m_skeletalTrackingProvider.GetCurrentFrameData(ref m_lastFrameData))
                {
                    bodyTrackingResult = m_lastFrameData.NumOfBodies > 0;
                }
            }
        }

        public async Task<bool> WatchAsync(float timeOut)
        {
            int timeLimit = (int)(timeOut * 1000);
            int timeElapsed = 0;
            int timeStep = 250;

            while (timeLimit > 0 && !bodyTrackingResult)
            {
                timeLimit -= timeStep;
                await Task.Delay(timeStep);
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
                timeElapsed += timeStep;
            }

            return bodyTrackingResult;
        }
        #endregion

        #region Generic WebAPI
        public async Task<string> WebApiAsync()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://postman-echo.com/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await httpClient.GetAsync("get").ConfigureAwait(false);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return string.Empty;
            }
            else
            {
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return json;
            }
        }
        #endregion

        #region Speech SDK event handlers
        private void SpeechRecognizer_SpeechStartDetected(object sender, RecognitionEventArgs e)
        {
            //TODO: Implement event handler as necessary
            //synchronizationContext.Post(_ => { Variables.Application.Set("SampleApplicationVariable", string.Empty); }, null);
        }

        private void SpeechRecognizer_Recognized(object sender, SpeechRecognitionEventArgs e)
        {
            //TODO: Implement event handler as necessary
            //synchronizationContext.Post(_ => { Variables.Application.Set("SampleApplicationVariable", string.Empty); }, null);
        }

        private void SpeechRecognizer_SpeechEndDetected(object sender, RecognitionEventArgs e)
        {
            //TODO: Implement event handler as necessary
            //synchronizationContext.Post(_ => { Variables.Application.Set("SampleApplicationVariable", string.Empty); }, null);
        }

        private void SpeechSynthesizer_SynthesisStarted(object sender, SpeechSynthesisEventArgs e)
        {
            //TODO: Implement event handler as necessary
            //synchronizationContext.Post(_ => { Variables.Application.Set("SampleApplicationVariable", string.Empty); }, null);
        }

        private void SpeechSynthesizer_Synthesizing(object sender, SpeechSynthesisEventArgs e)
        {
            //TODO: Implement event handler as necessary
            //synchronizationContext.Post(_ => { Variables.Application.Set("SampleApplicationVariable", string.Empty); }, null);
        }

        private void SpeechSynthesizer_SynthesisCompleted(object sender, SpeechSynthesisEventArgs e)
        {
            //TODO: Implement event handler as necessary
            //synchronizationContext.Post(_ => { Variables.Application.Set("SampleApplicationVariable", string.Empty); }, null);
        }

        private void SpeechSynthesizer_SynthesisCanceled(object sender, SpeechSynthesisEventArgs e)
        {
            //TODO: Implement event handler as necessary
            //synchronizationContext.Post(_ => { Variables.Application.Set("SampleApplicationVariable", string.Empty); }, null);
        }

        private void SpeechSynthesizer_VisemeReceived(object sender, SpeechSynthesisVisemeEventArgs e)
        {
            //TODO: Implement event handler as necessary
            //Debug.Log($"Viseme event received. Audio offset: " + $"{e.AudioOffset / 10000}ms, viseme id: {e.VisemeId}.");
        }

        private void SpeechSynthesizer_BookmarkReached(object sender, SpeechSynthesisBookmarkEventArgs e)
        {
            //TODO: Implement event handler as necessary
            //synchronizationContext.Post(_ => { Variables.Application.Set("SampleApplicationVariable", string.Empty); }, null);
        }
        #endregion

        void OnApplicationQuit()
        {
            cancellationToken.Cancel();
        }

        void OnDestroy()
        {
            if (SpeechSynthesizer != null)
            {
                SpeechSynthesizer.SynthesisStarted -= SpeechSynthesizer_SynthesisStarted;
                SpeechSynthesizer.Synthesizing -= SpeechSynthesizer_Synthesizing;
                SpeechSynthesizer.SynthesisCompleted -= SpeechSynthesizer_SynthesisCompleted;
                SpeechSynthesizer.SynthesisCanceled -= SpeechSynthesizer_SynthesisCanceled;

                SpeechSynthesizer.VisemeReceived -= SpeechSynthesizer_VisemeReceived;
                SpeechSynthesizer.BookmarkReached -= SpeechSynthesizer_BookmarkReached;

                SpeechSynthesizer.Dispose();
            }

            if (SpeechRecognizer != null)
            {
                SpeechRecognizer.SpeechStartDetected -= SpeechRecognizer_SpeechStartDetected;
                SpeechRecognizer.Recognized -= SpeechRecognizer_Recognized;
                SpeechRecognizer.SpeechEndDetected -= SpeechRecognizer_SpeechEndDetected;

                SpeechRecognizer.Dispose();
            }

            StopAllCoroutines();
        }
    }
}