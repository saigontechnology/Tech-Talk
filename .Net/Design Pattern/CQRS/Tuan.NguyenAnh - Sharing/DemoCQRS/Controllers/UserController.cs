using BusinessLayer.Command;
using BusinessLayer.Queries;
using BusinessLayer.Services;
using DataLayer.Query;
using DataLayer.RequestDto;
using DataLayer.Service.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoCQRS.Controllers
{
    [Route("api/Users")]
    public class UserController : Controller
    {
        private readonly  ICommandQueue _commandQueue;
        private readonly IQueryQueue _queryQueue;

        public UserController(ICommandQueue commandQueue, IQueryQueue queryQueue)
        {
            _commandQueue = commandQueue;
            _queryQueue = queryQueue;
        }

        [HttpPost]
        public IActionResult Register([FromBody] NewUserDto dto)
        {
            var command = new RegisterUser(Guid.NewGuid(),dto.FirstName, dto.LastName, dto.UserName, dto.Password);

            _commandQueue.Send(command);

            return Ok();
        }

        [HttpGet("{userName}")]
        public IActionResult GetUser(string userName)
        {
            return Ok(_queryQueue.Send(new GetUserByUserNameQuery(userName)));
        }

        [HttpPut("{id}")]
        public IActionResult EditUseralInfo(Guid id, [FromBody] RenameUserDto dto)
        {
            var command = new RenameUser(id, dto.FirstName, dto.LastName);
            _commandQueue.Send(command);

            return Ok();
        }
    }
}
