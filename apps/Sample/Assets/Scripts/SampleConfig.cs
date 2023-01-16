using System;

namespace AzureEmbodiedAISamples
{
    [Serializable]
    public class SampleConfig
    {
        public string TTSSubscriptionKey;
        public string TTSRegion;
        public string STTSubscriptionKey;
        public string STTRegion;
        public string CLUSubscriptionKey;
        public string CLUURL;
        public string KBSubscriptionKey;
        public string KBURL;
        public string GPTSubscriptionKey;
        public string GPTURL;
    }
}