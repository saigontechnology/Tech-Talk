using DataLayer;
using DataLayer.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class QueryQueue : IQueryQueue
    {
        readonly Dictionary<string, Func<IQuery, dynamic>> _subscribers;

        /// <summary>
        /// Constructs the queue.
        /// </summary>
        public QueryQueue()
        {
            _subscribers = new Dictionary<string, Func<IQuery, dynamic>>();
        }

        #region Methods (subscription)

        /// <summary>
        /// One and only one subscriber can register for each query. If a query is sent then it must have a handler.
        /// </summary>
        public void Subscribe<T>(Func<T, dynamic> func) where T : IQuery
        {
            var name = typeof(T).AssemblyQualifiedName;

            if (_subscribers.Any(x => x.Key == name))
                throw new MissingHandlerException("Can not find query's handler");

            _subscribers.Add(name, (query) => func((T)query));
        }
        #endregion

        #region Methods (sending synchronous query)

        /// <summary>
        /// Executes the query synchronously.
        /// </summary>
        public dynamic Send(IQuery query)
        {
            return Execute(query, query.GetType().AssemblyQualifiedName);
        }

        #endregion


        /// <summary>
        /// Invokes the subscriber method registered to handle the query.
        /// </summary>
        private dynamic Execute(IQuery query, string @class)
        {
            if (_subscribers.ContainsKey(@class))
            {
                var action = _subscribers[@class];
                return action.Invoke(query);
            }
            else
            {
                throw new MissingHandlerException("Can not find query's handler");
            }
        }

    }
}
