using BusinessLayer.Command;
using BusinessLayer.Queries;
using BusinessLayer.Services;
using DataLayer.Query;
using DataLayer.RequestDto;
using DataLayer.Service.Query;
using Microsoft.AspNetCore.Mvc;

namespace DemoCQRS.Controllers
{
    [Route("api/accounts")]
    public class AccountController : Controller
    {
        private readonly ICommandQueue _commandQueue;
        private readonly IQueryQueue _queryQueue;

        public AccountController(ICommandQueue commandQueue, IQueryQueue queryQueue)
        {
            _commandQueue = commandQueue;
            _queryQueue = queryQueue;
        }

        [HttpPost]
        public IActionResult CreateAccount([FromBody] OpenAccountDto dto)
        {
            var newAccountId = Guid.NewGuid();
            _commandQueue.Send(new OpenAccount(newAccountId,dto.Owner, dto.Code));
            _commandQueue.Send(new DepositMoney(newAccountId, dto.Balance));
            return Ok();
        }

        [HttpGet("{code}")]
        public IActionResult GetAccount(string code)
        {
            return Ok(_queryQueue.Send(new GetAccountByCodeQuery(code)));
        }

        [HttpPut("{id}/deposit")]
        public IActionResult Deposit(Guid id, [FromForm] decimal amount)
        {
            _commandQueue.Send(new DepositMoney(id, amount));

            return Ok();
        }
    }
}
