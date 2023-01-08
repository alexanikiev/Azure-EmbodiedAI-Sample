# Azure-EmbodiedAI-Sample

Welcome to the Azure EmbodiedAI Sample repository on GitHub. With only about 500 lines of custom code written with the help of [ChatGPT](https://chat.openai.com/) this simple Unity project demonstrates how to build an EmbodiedAI character which is able to speak, listen and see by using Azure Cognitive Services (Azure Speech SDK), Azure Kinect Body Tracking SDK and Unity Visual Scripting. Initial design for Microsoft's legendary Clippy character is inspired by the Blender project [here](https://github.com/david0429/Clippy). 

## Getting Started

This Unity sample is intended for Windows Desktop platform. [Azure Kinect DK](https://azure.microsoft.com/en-us/products/kinect-dk/) and Azure Kinect Body Tracking SDK are leveraged for Body Tracking (seeing), it is recommended to use NVIDIA GeForce RTX graphics and CUDA processing mode for better performance, however CPU processing mode may also be used otherwise. For speaking and listening Azure Speech SDK is utilized, it is assumed that your PC has microphone and speakers.

This sample is built based on the following foundational GitHub templates:

* [Sample Unity Body Tracking Application](https://github.com/microsoft/Azure-Kinect-Samples/tree/master/body-tracking-samples/sample_unity_bodytracking)
* [Synthesize speech in Unity using the Speech SDK for Unity](https://github.com/Azure-Samples/cognitive-services-speech-sdk/tree/master/quickstart/csharp/unity/text-to-speech)
* [Recognize speech from a microphone in Unity using the Speech SDK for Unity](https://github.com/Azure-Samples/cognitive-services-speech-sdk/tree/master/quickstart/csharp/unity/from-microphone)

Solution architecture of this template is described in detail in the following Medium articles:

* [Thoughts about Embodiment of AI](https://medium.com/@alexanikiev/thoughts-about-embodiment-of-ai-4fa4d7841d2c)
* [More about Embodiment of AI](https://medium.com/@alexanikiev/more-about-embodiment-of-ai-d9871251090d)

Solution demo video can be found [here](https://www.youtube.com/channel/UCNWEuI2ijKLNe_txLEQTEDA) and Code walkthrough is available [here](https://www.youtube.com/channel/UCNWEuI2ijKLNe_txLEQTEDA).

## Prerequisites

Please see the list below with the compatibility guidance for the versions of software the sample was developed and tested on.

| Prerequisite                   | Download Link                                                   | Version           |
|--------------------------------|-----------------------------------------------------------------|-------------------|
| Unity                          | https://unity.com/download                                      | 2021.3.16f1 (LTS) |
| Azure Speech SDK for Unity*    | https://aka.ms/csspeech/unitypackage                            | 1.24.2            |
| Azure Kinect Body Tracking SDK | https://www.microsoft.com/en-us/download/details.aspx?id=104221 | 1.1.2             |
| NVIDIA CUDA Toolkit            | https://developer.nvidia.com/cuda-downloads                     | 11.7              |
| NVIDIA cuDNN for CUDA          | https://developer.nvidia.com/rdp/cudnn-archive                  | 8.5.0             |

Please note that you will need to install Unity, Azure Kinect Body Tracking SDK, NVIDIA CUDA Toolkit and NVIDIA cuDNN for CUDA on your PC. And Azure Speech SDK for Unity and select Azure Kinect Body Tracking SDK, NVIDIA CUDA Toolkit and NVIDIA cuDNN for CUDA DLLs have been already packaged with the project for convenience of setup and configuration.

## Installing

After you've cloned or downloaded the code please install the prerequisites. Please note that select large files (DLLs) are stored with [Git LFS](https://docs.github.com/en/repositories/working-with-files/managing-large-files/about-git-large-file-storage), so you will need to manually download the files listed [here](https://github.com/alexanikiev/Azure-EmbodiedAI-Sample/blob/main/.gitattributes).

The default processing mode for Body Tracking is set to [CUDA](https://github.com/alexanikiev/Azure-EmbodiedAI-Sample/blob/faef9453e2ea3655ac76c51900bfa04e4fefc203/apps/Sample/Assets/Scripts/BodyTracking/SkeletalTrackingProvider.cs#L49) to optimize performance, to configure CUDA support please refer to guidance [here](https://medium.com/@alexanikiev/thoughts-about-embodiment-of-ai-4fa4d7841d2c) and [here](https://github.com/microsoft/Azure-Kinect-Samples/tree/master/body-tracking-samples/sample_unity_bodytracking). However you may change the processing mode to [CPU](https://github.com/alexanikiev/Azure-EmbodiedAI-Sample/blob/faef9453e2ea3655ac76c51900bfa04e4fefc203/apps/Sample/Assets/Scripts/BodyTracking/SkeletalTrackingProvider.cs#L49) as necessary.

Because this sample leverages Azure Backend service deployed in the Cloud, please follow Deployment guidance to deploy the necessary Azure Backend services. Once deployed, please copy [this](https://github.com/alexanikiev/Azure-EmbodiedAI-Sample/blob/main/Sample.auth) Auth file to the Desktop of your PC and propagate configuration details based on the deployed Azure Backend services.

Also this sample introduces a number of custom C# nodes for Unity Visual Scripting, after you open the project in Unity please Generate Nodes as described [here](https://docs.unity3d.com/Packages/com.unity.visualscripting@1.7/manual/vs-configuration.html#Regen).

![Unity Sample Project](/docs/images/UnitySampleProject.png)

To run the sample please open SampleScene and hit Play. You may also select Clippy game object in the Hierarchy and open SampleStateMachine graph to visually inspect the sample flow. 

![Unity Visual Scripting State Graph](/docs/images/UnityVisualScriptingStateGraph.png)

## Deployment

This sample features One-Click Deployment for the necessary Azure Backend services. If you need to sign up for Azure Subscription please follow [this](https://azure.microsoft.com/en-us/free/) link.

[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Falexanikiev%2FAzure-EmbodiedAI-Sample%2Fmain%2Fcloud%2Finfra%2Ftemplate.json)

## Next Steps

This minimalistic template may be a great jump start for your own EmbodiedAI project and the possibilities from now on are truly endless.

This template already gets you started with the following capabilities:

* Speech Synthesis: [Azure Speech SDK](https://learn.microsoft.com/en-us/azure/cognitive-services/speech-service/speech-sdk) [SpeechSynthesizer class](https://learn.microsoft.com/en-us/dotnet/api/microsoft.cognitiveservices.speech.speechsynthesizer)
* Speech Recognition: [Azure Speech SDK](https://learn.microsoft.com/en-us/azure/cognitive-services/speech-service/speech-sdk) [SpeechRecognizer class](https://learn.microsoft.com/en-us/dotnet/api/microsoft.cognitiveservices.speech.speechrecognizer)
* Computer Vision (Non-verbal interaction): [Azure Kinect Body Tracking SDK](https://learn.microsoft.com/en-us/azure/kinect-dk/body-sdk-download)
* Dialog Management: [Unity Visual Scripting](https://docs.unity3d.com/Packages/com.unity.visualscripting@1.7/manual/index.html) State Graph and Script Graphs

Please consider adding the following capabilities to your EmbodiedAI project as described in [this](https://medium.com/@alexanikiev/more-about-embodiment-of-ai-d9871251090d) Medium article:

* Natural Language Understanding: [Language Understanding (LUIS)](https://www.luis.ai/), [Conversational Language Understanding (CLU)](https://learn.microsoft.com/en-us/azure/cognitive-services/language-service/conversational-language-understanding), etc. 
* Natural Language Generation: [Azure OpenAI Service](https://azure.microsoft.com/en-us/products/cognitive-services/openai-service/)
* More Computer Vision: [Azure Kinect Sensor SDK](https://learn.microsoft.com/en-us/azure/kinect-dk/sensor-sdk-download)
* Knowledge Base: [Azure Cognitive Search (Semantic Search)](https://azure.microsoft.com/en-us/products/search/), [Question Answering](https://azure.microsoft.com/en-us/products/cognitive-services/question-answering/), etc.
* Any integrations with custom AI services via WebAPI: [Azure Machine Learning](https://azure.microsoft.com/en-us/products/machine-learning/), etc.

Note: If you are interested in leveraging [Microsot Power Virtual Agents](https://azure.microsoft.com/en-us/products/power-virtual-agents/) (PVA) on Azure while building your EmbodiedAI project, please review [this](https://medium.com/@alexanikiev/thoughts-about-embodiment-of-ai-4fa4d7841d2c) Medium article which provides details about using [DialogServiceConnector class](https://learn.microsoft.com/en-us/dotnet/api/microsoft.cognitiveservices.speech.dialog.dialogserviceconnector) based on [Interact with a bot in C# Unity](https://github.com/Azure-Samples/cognitive-services-speech-sdk/tree/master/samples/csharp/unity/virtual-assistant) GitHub template.   

## Future Updates

* Character Rig, animations using Unity Mechanim and Lip sync visemes
* Azure Backend services deployment options using Bicep and Terraform
* Backend services deployed on the Edge using Azure Cognitive Services containers

## Disclaimer

This code is provided "as is" without warranties to be used at your own risk.