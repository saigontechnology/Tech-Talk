using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TStore.Shared.Persistence;
using TStore.Shared.Repositories;
using TStore.Shared.Services;

namespace TStore.Shared.Helpers
{
    public static class ServiceRegistration
    {
        public static void RegisterTStoreCommonServices(this IServiceCollection services,
            string logId,
            string connStr)
        {
            services.AddDbContext<TStoreContext>(opt =>
                opt.UseLazyLoadingProxies().UseSqlServer(connStr));

            services.AddScoped<ITStoreUnitOfWork, TStoreUnitOfWork>()
                .AddScoped<IProductRepository, ProductRepository>()
                .AddScoped<IOrderRepository, OrderRepository>();

            services.AddScoped<IProductService, ProductService>()
                .AddScoped<IOrderService, OrderService>();

            services.AddSingleton<IRealtimeNotiService, RealtimeNotiService>();

            services.AddSingleton<IKafkaProducerManager, KafkaProducerManager>();

            services.AddSingleton<IMessagePublisher, KafkaMessagePublisher>();

            services.AddSingleton<IApplicationLog>(p =>
            {
                IRealtimeNotiService notiService = p.GetRequiredService<IRealtimeNotiService>();
                return new ApplicationLog(logId, notiService);
            });
        }

        public static void RegisterInteractionCommonServices(this IServiceCollection services,
            string logId,
            string connStr)
        {
            services.AddDbContext<InteractionContext>(opt =>
                opt.UseLazyLoadingProxies().UseSqlServer(connStr));

            services.AddScoped<IInteractionUnitOfWork, InteractionUnitOfWork>()
                .AddScoped<IInteractionRepository, InteractionRepository>()
                .AddScoped<IInteractionReportRepository, InteractionReportRepository>();

            services.AddScoped<IInteractionService, InteractionService>();

            services.AddSingleton<IRealtimeNotiService, RealtimeNotiService>();

            services.AddSingleton<IKafkaProducerManager, KafkaProducerManager>();

            services.AddSingleton<IMessagePublisher, KafkaMessagePublisher>();

            services.AddSingleton<IApplicationLog>(p =>
            {
                IRealtimeNotiService notiService = p.GetRequiredService<IRealtimeNotiService>();
                return new ApplicationLog(logId, notiService);
            });
        }
    }
}
