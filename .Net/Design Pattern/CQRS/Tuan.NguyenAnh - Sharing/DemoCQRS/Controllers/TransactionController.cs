using BusinessLayer.Command;
using BusinessLayer.Services;
using DataLayer.Query;
using DataLayer.RequestDto;
using DataLayer.Service.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoCQRS.Controllers
{
    [Route("api/transactions")]
    public class TransactionController : Controller
    {
        private readonly ICommandQueue _commandQueue;
        private readonly IQuerySearch _querySearch;

        public TransactionController(ICommandQueue commandQueue, IQuerySearch querySearch)
        {
            _commandQueue = commandQueue;
            _querySearch = querySearch;
        }

        [HttpPost]
        public IActionResult CreateTransaction([FromBody] NewTransactionDto dto)
        {
            _commandQueue.Send(new StartTransfer(Guid.NewGuid(), dto.FromAccount, dto.ToAccount, dto.Amount));

            return Ok();
        }
    }
}
