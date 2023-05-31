using DataLayer;
using DataLayer.Exceptions;
using DataLayer.WriteModels;
using DataLayer.Service.Command;
using DataLayer.Service.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class CommandQueue : ICommandQueue
    {
        readonly Dictionary<string, Action<ICommand>> _subscribers;

        readonly ICommandStore _store;

        /// <summary>
        /// Constructs the queue.
        /// </summary>
        public CommandQueue(ICommandStore store)
        {
            _store = store;
            _subscribers = new Dictionary<string, Action<ICommand>>();
        }

        #region Methods (subscription)

        /// <summary>
        /// One and only one subscriber can register for each command. If a command is sent then it must have a handler.
        /// </summary>
        public void Subscribe<T>(Action<T> action) where T : ICommand
        {
            var name = typeof(T).AssemblyQualifiedName;

            if (_subscribers.Any(x => x.Key == name))
                throw new MissingHandlerException("Can not find command's handler");

            _subscribers.Add(name, (command) => action((T)command));
        }
        #endregion

        #region Methods (sending synchronous commands)

        /// <summary>
        /// Executes the command synchronously.
        /// </summary>
        public void Send(ICommand command)
        {
            SerializedCommand serialized = null;

            serialized = _store.Serialize(command);
            serialized.SendStarted = DateTimeOffset.UtcNow;
            serialized.SendStatus = "Started";
            _store.Save(serialized, true);

            Execute(command, command.GetType().AssemblyQualifiedName);

            serialized.SendCompleted = DateTimeOffset.UtcNow;
            serialized.SendStatus = "Completed";
            _store.Save(serialized, false);
        }

        #endregion


        /// <summary>
        /// Invokes the subscriber method registered to handle the command.
        /// </summary>
        private void Execute(ICommand command, string @class)
        {
            if (_subscribers.ContainsKey(@class))
            {
                var action = _subscribers[@class];
                action.Invoke(command);
            }
            else
            {
                throw new MissingHandlerException("Can not find command's handler");
            }
        }

    }
}
