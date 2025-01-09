using System.ComponentModel.DataAnnotations;

namespace AzureDocumentIntelligenceStudio.Models.RequestModels
{
    public class ResumeParserRequest
    {
        [Required]
        public IFormFile File { get; set; }
    }
}
