using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using ProcessImage.Services;
using System;
using System.IO;

namespace ProcessImage
{
    [StorageAccount("BlobConnectionString")]
    public class ProcessSmallImageFunction
    {
        private readonly IImageResizer _imageResizer;
        private readonly IImageAnalysis _imageAnalysis;
        public ProcessSmallImageFunction(IImageResizer imageResizer, IImageAnalysis imageAnalysis)
        {
            _imageResizer = imageResizer;
            _imageAnalysis = imageAnalysis;
        }
        [FunctionName("ProcessSmallImageFunction")]
        public void Run(
            [BlobTrigger("raw-image/{name}")] Stream inputBlob,
            [Blob("small-image/{name}", FileAccess.Write)] Stream smallImageBlob,
            string name,
            ILogger log,
            [ServiceBus("processedimage", Connection = "AzureServiceBus")] out ServiceBusMessage message)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {inputBlob.Length} Bytes");
            try
            {
                this._imageResizer.ResizeSmall(inputBlob, smallImageBlob);
                log.LogInformation("Reduced image saved to blob storage");
            }
            catch (Exception ex)
            {
                log.LogError("Resize fails" + ex.Message, ex);
            }
            var metadata = _imageAnalysis.AnalyzeImage(name, inputBlob.Length);
            message = new ServiceBusMessage(metadata.Name)
            {
                Subject = metadata.Name,
                ApplicationProperties =
                    {
                        { "color-red", metadata.Color.Red },
                        { "color-green", metadata.Color.Green },
                        { "color-blue", metadata.Color.Blue },
                        { "category", metadata.Category.ToString() },
                        { "size", "small"}
                    }
            };
        }
    }
}
