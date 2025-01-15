using AzureDocumentIntelligenceStudio.Models;
using AzureDocumentIntelligenceStudio.Models.RequestModels;
using AzureDocumentIntelligenceStudio.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureDocumentIntelligenceStudio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AzureDocumentIntelligenceController(IAzureDocumentIntelligenceService _azureDocumentIntelligenceService) : ControllerBase
    {
        [HttpPost("resume-parser")]
        public async Task<ActionResult<Resume>> CVParserAsync([FromForm] ResumeParserRequest request)
        {
            try
            {
                var result = await _azureDocumentIntelligenceService.ResumeExtractionAsync(request.File);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
