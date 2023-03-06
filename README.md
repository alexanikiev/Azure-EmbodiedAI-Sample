# Azure-EmbodiedAI-Sample

Welcome to the Azure EmbodiedAI Sample repository on GitHub. With only about 500 lines of custom code written with the help of [ChatGPT](https://chat.openai.com/) this simple Unity project demonstrates how to build an EmbodiedAI character which is able to speak, listen and see by using Azure Cognitive Services (Azure Speech SDK), Azure Kinect Body Tracking SDK and Unity Visual Scripting. Initial design for Microsoft's legendary Clippy character is inspired by Blender project [here](https://github.com/david0429/Clippy). 

## Motivation

Make everything as simple as possible, but not simpler.

## Getting Started

This Unity sample is intended for Windows Desktop platform. [Azure Kinect DK](https://azure.microsoft.com/en-us/products/kinect-dk/) and Azure Kinect Body Tracking SDK are leveraged for Body Tracking (seeing), it is recommended to use NVIDIA GeForce RTX graphics and CUDA processing mode for better performance, however CPU processing mode may also be used otherwise. For speaking and listening Azure Speech SDK is utilized, it is assumed that your PC has microphone and speakers.

This sample is built based on the following foundational GitHub templates:

* [Sample Unity Body Tracking Application](https://github.com/microsoft/Azure-Kinect-Samples/tree/master/body-tracking-samples/sample_unity_bodytracking)
* [Synthesize speech in Unity using the Speech SDK for Unity](https://github.com/Azure-Samples/cognitive-services-speech-sdk/tree/master/quickstart/csharp/unity/text-to-speech)
* [Recognize speech from a microphone in Unity using the Speech SDK for Unity](https://github.com/Azure-Samples/cognitive-services-speech-sdk/tree/master/quickstart/csharp/unity/from-microphone)

Solution architecture of this template is described in detail in the following Medium articles:

* [Thoughts about Embodiment of AI](https://medium.com/@alexanikiev/thoughts-about-embodiment-of-ai-4fa4d7841d2c)
* [More about Embodiment of AI](https://medium.com/@alexanikiev/more-about-embodiment-of-ai-d9871251090d)

Solution demo video can be found [here](https://youtu.be/4azVEpdIp0Y) and Code walkthrough is available [here](https://youtu.be/z17xC5Bq7DI).

<a href="http://www.youtube.com/watch?feature=player_embedded&v=4azVEpdIp0Y" target="_blank"><img src="http://img.youtube.com/vi/4azVEpdIp0Y/0.jpg" alt="Azure EmbodiedAI Sample Setup and Demo" width="560" height="315" border="10" /></a>

## Prerequisites

Please see the list below with the compatibility guidance for the versions of software the sample was developed and tested on.

| Prerequisite                   | Download Link                                                   | Version           |
|--------------------------------|-----------------------------------------------------------------|-------------------|
| Unity                          | https://unity.com/download                                      | 2021.3.16f1 (LTS) |
| Azure Speech SDK for Unity     | https://aka.ms/csspeech/unitypackage                            | 1.24.2            |
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

## Cloud Deployment

This sample features One-Click Deployment for the necessary Azure Backend services. If you need to sign up for Azure Subscription please follow [this](https://azure.microsoft.com/en-us/free/) link.

[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Falexanikiev%2FAzure-EmbodiedAI-Sample%2Fmain%2Fcloud%2Finfra%2Ftemplate.json)

## Edge Deployment

| Capability                           | Docker Container                                                                | Protocol | Port  |
|--------------------------------------|---------------------------------------------------------------------------------|----------|-------|
| STT (Speech-to-Text)                 | mcr.microsoft.com/azure-cognitive-services/speechservices/speech-to-text        | WS(S)    | 5001  |
| TTS (Neural-Text-to-Speech)          | mcr.microsoft.com/azure-cognitive-services/speechservices/neural-text-to-speech | WS(S)    | 5002  |
| LUIS (Intent Recognition)            | mcr.microsoft.com/azure-cognitive-services/language/luis                        | HTTP(S)  | 5003  |
| ELASTIC-OSS Elasticsearch            | docker.elastic.co/elasticsearch/elasticsearch-oss:7.10.2                        | HTTP(S)  | 9200  |
| ELASTIC-OSS Kibana                   | docker.elastic.co/kibana/kibana-oss:7.10.2                                      | HTTP(S)  | 5601  |
| GPT-J                                |                                                                                 | HTTP(S)  | 5004  |

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

Please also review important Game Development Concepts in the context of Azure Speech SDK explained in detail in [this](https://learn.microsoft.com/en-us/azure/cognitive-services/speech-service/gaming-concepts) article.

## Future Updates

* Character Rig and animations using Unity Mechanim
* Azure Backend services deployment options using Bicep and Terraform
* Backend services deployed on the Edge using Helm chart on Kubernetes (K8S) cluster
* Sample template for Unreal Engine (UE) using Blueprints
* Sample template for WebGL using Babylon.js

## Lip Sync Visemes

This template features a simple and yet robust implementation of Lip Sync Visemes using Azure Speech SDK as explained in [this](https://www.youtube.com/watch?v=lo898xUzPPA) video. You can choose to introduce visemes for your character using Maya Blend shapes, Blender shape keys, etc. Then in Unity you can animate visemes by manipulating with Blend shape weights, by means of Unity Blend tree, etc.

![Lip Sync Visemes Maya Blend Shapes](/docs/images/ClippyMaya_LipSync_Visemes.png)

## Azure OpenAI

Sample context: 
`Clippy is an endearing and helpful digital assistant, designed to make using Microsoft Office Suite of products more efficient and user-friendly. With his iconic paperclip shape and friendly personality, Clippy is always ready and willing to assist users with any task or question they may have. His ability to anticipate and address potential issues before they even arise has made him a beloved and iconic figure in the world of technology, widely recognized as an invaluable tool for productivity.`

Experiment (1/23/2023): Azure OpenAI is deployed in South Central US region and Unity App client runs in West US geo.  

| Chat turn # | Sample Human request | Responsiveness test example 1 | Responsiveness test example 2 |
|----------|----------|----------|----------|
| 1 | `What is Azure Cognitive Search?` | Total tokens: 117, Azure OpenAI processing timing: 90.6115 ms, Azure OpenAI Studio browser request/response timing: 1.56 s | Total tokens: 117, Azure OpenAI processing timing: 96.2115 ms, Azure OpenAI Studio browser request/response timing: 1.40 s |
| 2 | `What are the benefits of using Azure Cognitive Search?` | Total tokens: 177, Azure OpenAI processing timing: 128.6817 ms, Azure OpenAI Studio browser request/response timing: 1.41 s | Total tokens: 167, Azure OpenAI processing timing: 73.1746 ms, Azure OpenAI Studio browser request/response timing: 957.91 ms |
| 3 | `How can I index my data with Azure Cognitive Search?` | Total tokens: 246, Azure OpenAI processing timing: 168.1983 ms, Azure OpenAI Studio browser request/response timing: 1.34 s | Total tokens: 229, Azure OpenAI processing timing: 229.8757 ms, Azure OpenAI Studio browser request/response timing: 597.07 ms |

## GPU Acceleration

When building modern immersive EmbodiedAI experiences responsiveness, fluidity and speed of interaction is very important. Thus GPU Acceleration will be an important factor for the successful adoption of your solution. 

We've already described [above](https://github.com/alexanikiev/Azure-EmbodiedAI-Sample#installing) how we take advantage of GPU Acceleration for Azure Kinect Body Tracking SDK.

GPU Acceleration can also help in the context of any custom Machine Learning models you may want to build, train, deploy and expose as a Web Service (Web API) for consumption in your EmbodiedAI solution. Please find more information about Building and Deploying custom Machine Learning models in Azure Cloud and Edge in these Medium articles: [here](https://alexanikiev.medium.com/building-ai-applications-for-azure-cloud-65252b602042) and [here](https://alexanikiev.medium.com/deploying-ai-models-in-azure-cloud-23f3f52a8813). [PyTorch](https://pytorch.org/) is a popular open source machine learning framework which allows you to build such models and has built-in support for GPU Acceleration.

## Disclaimer

This code is provided "as is" without warranties to be used at your own risk.
