# docker run --rm -it -p 5003:5000 --memory 4g --cpus 2 --mount type=bind,src=c:\input,target=/input --mount type=bind,src=c:\output\,target=/output mcr.microsoft.com/azure-cognitive-services/language/luis Eula=accept Billing=https://your_luis_authoring_resource.cognitiveservices.azure.com/ ApiKey=your_luis_subscription_key

# Use the official image for the Azure Cognitive Services Language Understanding (LUIS)
FROM mcr.microsoft.com/azure-cognitive-services/language/luis

# Accept the EULA
ENV Eula=accept

# Set the billing endpoint for the service
ENV Billing=https://${LUISAuthoringResource}.cognitiveservices.azure.com/

# Set the API key for the service
ENV ApiKey=${ApiKey}

# Expose the port for the service
EXPOSE 5000

# Start the service
CMD ["dotnet", "run"]