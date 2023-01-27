# docker run --rm -it -p 5002:5000 --memory 12g --cpus 6 mcr.microsoft.com/azure-cognitive-services/speechservices/neural-text-to-speech Eula=accept Billing=https://your_tts_region.api.cognitive.microsoft.com/sts/v1.0/issuetoken ApiKey=your_tts_subscription_key

# Use the official image for Azure Cognitive Services Speech SDK, Neural Text-to-Speech
FROM mcr.microsoft.com/azure-cognitive-services/speechservices/neural-text-to-speech

# Accept the EULA
ENV Eula=accept

# Set the billing endpoint for the service
ENV Billing=https://${AzureRegion}.api.cognitive.microsoft.com/sts/v1.0/issuetoken

# Set the API key for the service
ENV ApiKey=${ApiKey}

# Expose the port for the service
EXPOSE 5000

# Start the service
CMD ["dotnet", "run"]