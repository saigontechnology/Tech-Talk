using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using TStore.Shared.Models;
using TStore.Shared.Services;

namespace TStore.InteractionApi.Controllers
{
    [Route("api/interactions")]
    [ApiController]
    public class InteractionsController : ControllerBase
    {
        private readonly IInteractionService _interactionService;
        private readonly IApplicationLog _log;
        private readonly bool _saveByConsumers;

        public InteractionsController(IInteractionService interactionService,
            IConfiguration configuration,
            IApplicationLog log)
        {
            _interactionService = interactionService;
            _log = log;
            _saveByConsumers = configuration.GetValue<bool>("SaveByConsumers");
        }

        [HttpPost("")]
        public async Task<IActionResult> SaveInteraction([FromBody] InteractionModel interactionModel)
        {
            if (!_saveByConsumers)
            {
                // [DEMO] direct save
                await _interactionService.SaveInteractionsAsync(new List<InteractionModel>
                {
                    interactionModel
                });

                await _log.LogAsync($"Finish saving interaction of type {interactionModel.Action}");
            }
            else
            {
                // [DEMO] use Kafka
                await _interactionService.PublishNewUnsavedInteractionAsync(interactionModel);

                await _log.LogAsync($"Finish publishing interaction event of type {interactionModel.Action}");
            }

            return Accepted();
        }

        [HttpGet("reports")]
        public async Task<IActionResult> GetInteractionReports()
        {
            IEnumerable<InteractionReportModel> reports = await _interactionService.GetInteractionReportsAsync();

            return Ok(reports);
        }
    }
}
