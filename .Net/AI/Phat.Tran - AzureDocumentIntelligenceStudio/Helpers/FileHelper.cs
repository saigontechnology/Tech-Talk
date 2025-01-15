namespace AzureDocumentIntelligenceStudio.Helpers
{
    public interface IFileHelper
    {
        Task<MemoryStream> ToMemoryStreamAsync(IFormFile file, CancellationToken cancellationToken = default);
    }
    public class FileHelper : IFileHelper
    {
        public async Task<MemoryStream> ToMemoryStreamAsync(IFormFile file, CancellationToken cancellationToken = default)
        {
            var memoryStream = new MemoryStream();
            var stream = file.OpenReadStream();
            await stream.CopyToAsync(memoryStream, cancellationToken);
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
