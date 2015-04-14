using System;
using System.Collections.Generic;

namespace AspNet.Buzz
{
    public class EventStore<TEvent> : IEventStore<TEvent>
    {
        private readonly List<TEvent> _events = new List<TEvent>();
        
        public void Add(TEvent @event)
        {
            lock (_events)
            {
                _events.Add(@event);
            }
        }

        public IList<TEvent> GetTopN()
        {
            lock (_events)
            {
                return _events;
            }
        }
    }
}
