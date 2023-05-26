using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TStore.Shared.Constants;
using TStore.Shared.Entities;
using TStore.Shared.Models;
using TStore.Shared.Repositories;

namespace TStore.Shared.Services
{
    public interface IOrderService
    {
        Task<PagingListResponse<OrderModel>> GetOrdersAsync(SimpleFilterModel filter);
        Task<Guid> CreateOrderAsync(SubmitOrderModel order);
        Task<double> ApplyShipAsync(Guid orderId, OrderModel order);
        Task<double> ApplyDiscountAsync(Guid orderId, OrderModel order);
    }

    public class OrderService : IOrderService
    {
        private readonly IRealtimeNotiService _realtimeNotiService;
        private readonly IMessagePublisher _messagePublisher;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public OrderService(
            IRealtimeNotiService realtimeNotiService,
            IMessagePublisher messagePublisher,
            IOrderRepository orderRepository,
            IProductRepository productRepository)
        {
            _realtimeNotiService = realtimeNotiService;
            _messagePublisher = messagePublisher;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public async Task<double> ApplyDiscountAsync(Guid orderId, OrderModel order)
        {
            await Task.Delay(new Random().Next(2000, 5000));

            double discount = 0;

            if (order.UserName.Contains("T")
                || order.UserName.Contains("N"))
            {
                discount += new Random().Next(500, 5000);
            }

            string[] discountProducts = new[] { "Laptop", "Iphone 14", "Xbox" };

            if (order.Items.Any(i => discountProducts.Contains(i.Name)))
            {
                discount += new Random().Next(500, 5000);
            }

            Order orderEntity = new Order
            {
                Id = orderId
            };

            _orderRepository.Attach(orderEntity);

            orderEntity.Discount = discount;

            await _orderRepository.UnitOfWork.SaveChangesAsync();

            await PublishPromotionAppliedAsync(orderId, discount);

            return discount;
        }

        public async Task<double> ApplyShipAsync(Guid orderId, OrderModel order)
        {
            double shipAmount = 0;

            if (!order.UserName.Contains("T")
                && !order.UserName.Contains("N"))
            {
                shipAmount += new Random().Next(500, 5000);
            }

            string[] discountProducts = new[] { "Laptop", "Iphone 14", "Xbox" };

            if (!order.Items.Any(i => discountProducts.Contains(i.Name)))
            {
                shipAmount += new Random().Next(500, 5000);
            }

            Order orderEntity = new Order
            {
                Id = orderId
            };

            _orderRepository.Attach(orderEntity);

            orderEntity.ShipAmount = shipAmount;

            await _orderRepository.UnitOfWork.SaveChangesAsync();

            await PublishShipAppliedAsync(orderId, shipAmount);

            return shipAmount;
        }

        public async Task<Guid> CreateOrderAsync(SubmitOrderModel order)
        {
            Dictionary<Guid, double> productPriceMap = _productRepository.Get().Where(p => order.ProductIds.Contains(p.Id))
                .Select(p => new
                {
                    p.Id,
                    p.Price
                })
                .ToDictionary(p => p.Id, p => p.Price);

            Order orderEntity = new Order
            {
                Id = Guid.NewGuid(),
                CreatedTime = DateTimeOffset.UtcNow,
                UserName = order.UserName,
                OrderItems = order.ProductIds.Select(pId => new OrderItem
                {
                    ProductId = pId
                }).ToArray(),
                Total = order.ProductIds.Select(pId => productPriceMap[pId]).Sum()
            };

            _orderRepository.Create(orderEntity);

            await _orderRepository.UnitOfWork.SaveChangesAsync();

            await PublishNewOrderAsync(orderEntity);

            return orderEntity.Id;
        }

        public async Task<PagingListResponse<OrderModel>> GetOrdersAsync(SimpleFilterModel filter)
        {
            IQueryable<Entities.Order> query = _orderRepository.Get();

            if (!string.IsNullOrWhiteSpace(filter.Terms))
            {
                query = query.Where(p => p.Id.ToString().Contains(filter.Terms));
            }

            int total = await query.CountAsync();

            query = query.OrderByDescending(o => o.CreatedTime);

            if (filter.PageSize != null && filter.Page != null)
            {
                query = query.Skip(filter.Page.Value * filter.PageSize.Value).Take(filter.PageSize.Value);
            }

            OrderModel[] orders = await query
                .Select(o => new OrderModel
                {
                    Id = o.Id,
                    CreatedTime = o.CreatedTime,
                    Discount = o.Discount,
                    ShipAmount = o.ShipAmount,
                    Total = o.Total,
                    UserName = o.UserName,
                    Items = o.OrderItems.Select(item => new ProductModel
                    {
                        Id = item.ProductId,
                        Name = item.Product.Name,
                        Price = item.Product.Price
                    }).ToList()
                }).ToArrayAsync();

            return new PagingListResponse<OrderModel>
            {
                Total = total,
                Items = orders
            };
        }

        private async Task PublishNewOrderAsync(Order orderEntity)
        {
            Guid[] productIds = orderEntity.OrderItems.Select(o => o.ProductId).ToArray();
            Dictionary<Guid, ProductModel> productMap = await _productRepository.Get().Where(p => productIds.Contains(p.Id))
                .ToDictionaryAsync(o => o.Id, o => new ProductModel
                {
                    Id = o.Id,
                    Name = o.Name,
                    Price = o.Price
                });

            OrderModel orderModel = new OrderModel
            {
                Id = orderEntity.Id,
                CreatedTime = orderEntity.CreatedTime,
                Discount = orderEntity.Discount,
                ShipAmount = orderEntity.ShipAmount,
                Total = orderEntity.Total,
                UserName = orderEntity.UserName,
                Items = orderEntity.OrderItems.Select(o => productMap[o.ProductId]).ToList()
            };

            await _messagePublisher.PublishAndWaitAsync(
                EventConstants.Events.NewOrder,
                orderModel.Id.ToString(),
                orderModel as object);

            await _realtimeNotiService.NotifyAsync(new NotificationModel
            {
                Type = NotificationModel.NotificationType.NewOrder,
                Data = orderModel
            });
        }

        private async Task PublishPromotionAppliedAsync(Guid orderId, double discountAmount)
        {
            PromotionAppliedEvent promotionAppliedEvent = new PromotionAppliedEvent
            {
                OrderId = orderId,
                Discount = discountAmount
            };

            await _messagePublisher.PublishAndWaitAsync(
                EventConstants.Events.PromotionApplied,
                orderId.ToString(),
                promotionAppliedEvent as object);

            await _realtimeNotiService.NotifyAsync(new NotificationModel
            {
                Type = NotificationModel.NotificationType.PromotionApplied,
                Data = promotionAppliedEvent
            });
        }

        private async Task PublishShipAppliedAsync(Guid orderId, double shipAmount)
        {
            ShipAppliedEvent shipAppliedEvent = new ShipAppliedEvent
            {
                OrderId = orderId,
                ShipAmount = shipAmount
            };

            await _messagePublisher.PublishAndWaitAsync(
                EventConstants.Events.ShipApplied,
                orderId.ToString(),
                shipAppliedEvent as object);

            await _realtimeNotiService.NotifyAsync(new NotificationModel
            {
                Type = NotificationModel.NotificationType.ShipApplied,
                Data = shipAppliedEvent
            });
        }
    }
}
