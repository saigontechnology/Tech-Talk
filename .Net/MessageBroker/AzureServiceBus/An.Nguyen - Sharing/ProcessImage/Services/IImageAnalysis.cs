using ProcessImage.Models;

namespace ProcessImage.Services
{
    public interface IImageAnalysis
    {
        public FileMetadata AnalyzeImage(string name, long size);
    }
}
