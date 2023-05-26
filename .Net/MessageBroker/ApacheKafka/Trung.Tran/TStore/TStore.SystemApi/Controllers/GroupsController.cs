using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TStore.Shared.Services;
using TStore.SystemApi.Services;

namespace TStore.SystemApi.Controllers
{
    [Route("api/groups")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IMessageBrokerService _messageBrokerService;
        private readonly IApplicationLog _log;

        public GroupsController(IMessageBrokerService messageBrokerService,
            IApplicationLog log)
        {
            _messageBrokerService = messageBrokerService;
            _log = log;
        }

        [HttpDelete("{groupId}")]
        public async Task<IActionResult> DeleteGroup(string groupId)
        {
            await _messageBrokerService.DeleteGroupAsync(groupId);

            await _log.LogAsync($"Deleted group {groupId}");

            return NoContent();
        }
    }
}
