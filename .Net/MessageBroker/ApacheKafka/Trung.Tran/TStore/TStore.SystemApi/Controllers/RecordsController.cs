using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TStore.Shared.Services;
using TStore.SystemApi.Models;
using TStore.SystemApi.Services;

namespace TStore.SystemApi.Controllers
{
    [Route("api/records")]
    [ApiController]
    public class RecordsController : ControllerBase
    {
        private readonly IMessageBrokerService _messageBrokerService;
        private readonly IApplicationLog _log;

        public RecordsController(IMessageBrokerService messageBrokerService,
            IApplicationLog log)
        {
            _messageBrokerService = messageBrokerService;
            _log = log;
        }

        [HttpDelete("{topic}")]
        public async Task<IActionResult> DeleteRecords(string topic)
        {
            await _messageBrokerService.ClearRecordsAsync(topic);

            return NoContent();
        }

        [HttpPost("tombstone")]
        public async Task<IActionResult> CreateTombstone(TombstoneModel model)
        {
            await _messageBrokerService.CreateTombstoneAsync(model);

            await _log.LogAsync($"Tombstone created for topic {model.Topic} and key {model.Key}");

            return NoContent();
        }
    }
}
