using DataLayer.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Service.Command
{
    public abstract class AggregateState
    {
        public void Apply(IEvent @event)
        {
            var when = GetType().GetMethod("When", new[] { @event.GetType() });

            if (when == null)
                throw new MissingHandlerException("Can not find event's handler");

            when.Invoke(this, new object[] { @event });
        }
    }
}
