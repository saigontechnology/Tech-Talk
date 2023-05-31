using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public interface IQueryQueue
    {
        /// <summary>
        /// Registers a handler for a specific query.
        /// </summary>
        void Subscribe<T>(Func<T, dynamic> action) where T : IQuery;

        /// <summary>
        /// Sends the query as a synchronous operation. 
        /// </summary>
        dynamic Send(IQuery query);
    }
}
