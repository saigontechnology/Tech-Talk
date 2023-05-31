using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public interface ICommandQueue
    {
        /// <summary>
        /// Registers a handler for a specific command.
        /// </summary>
        void Subscribe<T>(Action<T> action) where T : ICommand;

        /// <summary>
        /// Sends the command as a synchronous operation. 
        /// </summary>
        void Send(ICommand command);
    }
}
