using PlanningBook.Domain.Interfaces;

namespace PlanningBook.Domain
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly IServiceProvider _serviceProvider;
        //TODO: Implement Logger
        //private readonly ILogger<CommandExecutor> _logger;

        public CommandExecutor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            //_logger = (ILogger<QueryExecutor>)_serviceProvider.GetService(typeof(ILogger<QueryExecutor>));
        }

        public Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
        {
            var errorMessage = string.Empty;

            var htype = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
            if(htype == null)
            {
                errorMessage = $"Handler for command type {command.GetType().Name} and result type {typeof(TResult).Name} not found htype.";
                //TODO: Implement Logger
                //_logger.LogTrace(message);
                throw new InvalidOperationException(errorMessage);
            }

            dynamic handler = _serviceProvider.GetService(htype);
            if (handler == null) {
                errorMessage = $"Handler for command type {command.GetType().Name} and result type {typeof(TResult).Name} not found handler.";
                //TODO: Implement Logger
                //_logger.LogTrace(message);
                throw new InvalidOperationException(errorMessage);
            }

            //TODO: Implement Logger
            //_logger.LogTrace($"[Command Executor] [{handler.GetType().Name}] {JsonConvert.SerializeObject(command)}");
            return ((dynamic)handler).HandleAsync((dynamic)command);
        }
    }
}
