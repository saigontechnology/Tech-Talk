using AzureOpenAIResearch.Models.RequestModels;
using AzureOpenAIResearch.Models.ResponseModels;
using AzureOpenAIResearch.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureOpenAIResearch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OpenAIController(IAzureOpenAIService _azureOpenAIService) : ControllerBase
    {
        [HttpPost("send-chat")]
        public async Task<ActionResult<SendMessageResponse>> ChatAsync([FromBody] SendMessageRequest request)
        {
            try
            {
                var result = await _azureOpenAIService.SendChatAsync(request.Prompt, request.Histories);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
