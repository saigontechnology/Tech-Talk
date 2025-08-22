using PlanningBook.Domain.Interfaces;

namespace PlanningBook.Domain
{
    public class QueryExecutor : IQueryExecutor
    {
        private readonly IServiceProvider _serviceProvider;
        //TODO: Implement Logger
        //private readonly ILogger<QueryExecutor> _logger;
        public QueryExecutor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            //_logger = (ILogger<QueryExecutor>)_serviceProvider.GetService(typeof(ILogger<QueryExecutor>));
        }
        public Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
        {
            var errorMessage = string.Empty;
            var htype = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            if (htype == null)
            {
                errorMessage = $"Handler for query type {query.GetType().Name} and result type {typeof(TResult).Name} not found htype.";
                //TODO: Implement Logger
                //_logger.LogTrace(message);
                throw new InvalidOperationException(errorMessage);
            }

            dynamic handler = _serviceProvider.GetService(htype);
            if (handler == null)
            {
                errorMessage = $"Handler for query type {query.GetType().Name} and result type {typeof(TResult).Name} not found handler.";
                //TODO: Implement Logger
                //_logger.LogTrace(message);
                throw new InvalidOperationException(errorMessage);
            }

            //_logger.LogTrace($"[Query Executor] [{handler.GetType().Name}] {JsonConvert.SerializeObject(query)}");
            return handler.HandleAsync((dynamic)query);
        }
    }
}
