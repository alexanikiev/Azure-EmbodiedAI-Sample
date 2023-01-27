# docker run --rm -it -p 5001:5000 --memory 4g --cpus 4 mcr.microsoft.com/azure-cognitive-services/speechservices/speech-to-text Eula=accept Billing=https://your_stt_region.api.cognitive.microsoft.com/sts/v1.0/issuetoken ApiKey=your_stt_subscription_key

# Use the official image for Azure Cognitive Services Speech SDK, Speech-to-Text
FROM mcr.microsoft.com/azure-cognitive-services/speechservices/speech-to-text

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