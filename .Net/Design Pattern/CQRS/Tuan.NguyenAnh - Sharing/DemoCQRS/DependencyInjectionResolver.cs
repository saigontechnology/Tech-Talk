using BusinessLayer.CommandHandlers;
using BusinessLayer.EventHandlers;
using DataLayer.Service.Query;
using BusinessLayer.Serivces;
using BusinessLayer.Services;
using DataLayer.Service.Command;
using DataLayer.Service.Helper;
using BusinessLayer.Helper;
using BusinessLayer.QueryHandlers;

namespace DemoCQRS
{
    public static class DependencyInjectionResolver
    {
        public static void ConfigureDI(this IServiceCollection services, string connectionnString)
        {
            services.AddSingleton<ISerializer, Serializer>();
            services.AddSingleton<IQueryStore>(new QueryStore(connectionnString));
            services.AddSingleton<IQuerySearch>(new QuerySearch(connectionnString));
            services.AddSingleton<IEventQueue, EventQueue>();
            services.AddSingleton<IQueryQueue>(
                new QueryQueue()    
            );

            services.AddSingleton<ICommandStore>(
                new CommandStore(services.BuildServiceProvider().GetService<ISerializer>()
                , connectionnString)
            );

            services.AddSingleton<ICommandQueue>(
                new CommandQueue(services.BuildServiceProvider().GetService<ICommandStore>())
            );


            services.AddSingleton<IEventStore>(
                new EventStore(services.BuildServiceProvider().GetService<ISerializer>()
                , connectionnString)
            );

            services.AddSingleton<ISnapshotStore>(
                new SnapshotStore(connectionnString)
            );

            services.AddSingleton<IEventRepository>(
                new EventRepository(services.BuildServiceProvider().GetService<IEventStore>())
            );

            var queryQueue = services.BuildServiceProvider().GetService<IQueryQueue>();
            var commandQueue = services.BuildServiceProvider().GetService<ICommandQueue>();
            var eventQueue = services.BuildServiceProvider().GetService<IEventQueue>();
            var eventRepository = services.BuildServiceProvider().GetService<IEventRepository>();
            var querySearch = services.BuildServiceProvider().GetService<IQuerySearch>();
            var queryStore = services.BuildServiceProvider().GetService<IQueryStore>();
            var snapshotStore = services.BuildServiceProvider().GetService<ISnapshotStore>();

            services.AddSingleton<ISnapshotRepository>(
                new SnapshotRepository(services.BuildServiceProvider().GetService<IEventStore>(), eventRepository, snapshotStore)
            );

            var snapshotRepo = services.BuildServiceProvider().GetService<ISnapshotRepository>();

            services.AddSingleton<AccountCommandHandler>(new AccountCommandHandler(commandQueue, eventQueue, eventRepository));
            services.AddSingleton<AccountEventHandler>(new AccountEventHandler(eventQueue, queryStore));

            services.AddSingleton<UserCommandHandler>(new UserCommandHandler(commandQueue, eventQueue, snapshotRepo, querySearch));
            services.AddSingleton<UserEventHandler>(new UserEventHandler(eventQueue, queryStore));

            services.AddSingleton<TransferCommandHandler>(new TransferCommandHandler(commandQueue, eventQueue, eventRepository));
            services.AddSingleton<TransferEventHandler>(new TransferEventHandler(eventQueue, queryStore, querySearch));
            services.AddSingleton<TransferProcessHandler>(new TransferProcessHandler(commandQueue, eventQueue, eventRepository));

            services.AddSingleton<AccountQueryHandler>(new AccountQueryHandler(queryQueue, querySearch));
            services.AddSingleton<UserQueryHandler>(new UserQueryHandler(queryQueue, querySearch));
        }
    }
}
