using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProcessImage.Models;
using ProcessImage.Services;
using System;
using System.IO;
using System.Text;

namespace ProcessImage
{
    [StorageAccount("BlobConnectionString")]
    public class ProcessBigImageFunction
    {
        private readonly IImageResizer _imageResizer;
        private readonly IImageAnalysis _imageAnalysis;
        public ProcessBigImageFunction(IImageResizer imageResizer, IImageAnalysis imageAnalysis)
        {
            _imageResizer = imageResizer;
            _imageAnalysis = imageAnalysis;
        }
        [FunctionName("ProcessBigImageFunction")]
        public void Run(
            [BlobTrigger("raw-image/{name}")] Stream inputBlob,
            [Blob("big-image/{name}", FileAccess.Write)] Stream bigImageBlob,
            string name,
            ILogger log,
            [ServiceBus("processedimage", Connection = "AzureServiceBus")] out ServiceBusMessage message)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {inputBlob.Length} Bytes");
            try
            {
                _imageResizer.ResizeBig(inputBlob, bigImageBlob);
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
                        { "size", "big"}
                    }
            };
        }
    }
}
