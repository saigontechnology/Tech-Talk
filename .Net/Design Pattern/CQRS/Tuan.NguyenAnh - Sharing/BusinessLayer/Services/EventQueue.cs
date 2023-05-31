using DataLayer;
using DataLayer.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Serivces
{
    public class EventQueue : IEventQueue
    {
        /// <summary>
        /// An event's full class name is used as the key to a list of event-handling methods.
        /// </summary>
        readonly Dictionary<string, List<Action<IEvent>>> _subscribers;


        /// <summary>
        /// Constructs the queue.
        /// </summary>
        public EventQueue()
        {
            _subscribers = new Dictionary<string, List<Action<IEvent>>>();
        }

        /// <summary>
        /// Invokes each subscriber method registered to handle the event.
        /// </summary>
        /// <param name="event"></param>
        public void Publish(IEvent @event)
        {
            var name = @event.GetType().FullName;
            
            if (_subscribers.ContainsKey(name))
            {
                var actions = _subscribers[name];
                foreach (var action in actions)
                    action.Invoke(@event);
            }
            else
            {
                throw new MissingHandlerException("Can not find event's handler");
            }
        }

        /// <summary>
        /// Any number of subscribers can register for an event, and any one subscriber can register any number of
        /// methods to be invoked when the event is published. 
        /// </summary>
        public void Subscribe<T>(Action<T> action) where T : IEvent
        {
            var name = typeof(T).FullName;

            if (!_subscribers.Any(x => x.Key == name))
                _subscribers.Add(name, new List<Action<IEvent>>());

            _subscribers[name].Add((@event) => action((T)@event));
        }
    }
}
