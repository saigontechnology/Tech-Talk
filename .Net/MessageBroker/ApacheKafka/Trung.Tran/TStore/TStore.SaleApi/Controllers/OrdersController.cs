using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TStore.Shared.Models;
using TStore.Shared.Services;

namespace TStore.SaleApi.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IApplicationLog _log;

        public OrdersController(IOrderService orderService,
            IApplicationLog log)
        {
            _orderService = orderService;
            _log = log;
        }

        [HttpPost("filter")]
        public async Task<IActionResult> GetOrders([FromBody] SimpleFilterModel filter)
        {
            PagingListResponse<OrderModel> orderPaging = await _orderService.GetOrdersAsync(filter);

            return Ok(orderPaging);
        }

        [HttpPost("")]
        public async Task<IActionResult> SubmitOrder([FromBody] SubmitOrderModel order)
        {
            System.Guid id = await _orderService.CreateOrderAsync(order);

            await _log.LogAsync($"Finish creating order {id} of {order.UserName}");

            return NoContent();
        }
    }
}
