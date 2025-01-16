using Azure;
using Azure.AI.DocumentIntelligence;
using AzureDocumentIntelligenceStudio.Helpers;
using AzureDocumentIntelligenceStudio.Models;
using AzureDocumentIntelligenceStudio.Models.Utils;
using Microsoft.Extensions.Options;

namespace AzureDocumentIntelligenceStudio.Services
{
    public interface IAzureDocumentIntelligenceService
    {
        Task<Resume> ResumeExtractionAsync(IFormFile file, string modelId = null, CancellationToken cancellationToken = default);
    }

    public class AzureDocumentIntelligenceService(DocumentIntelligenceClient _documentIntelligenceClient,
        IOptions<AzureDocumentIntelligenceConfig> _azureDocumentIntelligenceConfig,
        IFileHelper _fileHelper,
        IResumeHelpers _resumeHelper) : IAzureDocumentIntelligenceService
    {
        public async Task<Resume> ResumeExtractionAsync(IFormFile file, string? modelId, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrEmpty(modelId))
                {
                    modelId = _azureDocumentIntelligenceConfig.Value.ModelId;
                }

                if (file == null)
                    throw new ArgumentNullException("File is empty!!!");

                if (string.IsNullOrWhiteSpace(modelId))
                    throw new ArgumentNullException("ModelId is not found!!!");

                var stream = await _fileHelper.ToMemoryStreamAsync(file, cancellationToken);
                var binaryData = await BinaryData.FromStreamAsync(stream, cancellationToken);
                var operation = await _documentIntelligenceClient.AnalyzeDocumentAsync(WaitUntil.Completed, modelId, binaryData);

                var document = operation.Value.Documents.FirstOrDefault();
                if (document == null)
                    throw new Exception("Can not process file");

                var resume = _resumeHelper.ExtractResumeData(document);
                return resume;
            }
            catch (Exception ex)
            {
                throw new Exception($"Something went wrong: {ex.Message}");
            }
        }
    }
}
