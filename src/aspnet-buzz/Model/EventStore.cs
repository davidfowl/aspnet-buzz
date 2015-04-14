using System;
using System.Collections.Generic;

namespace AspNet.Buzz
{
    public class EventStore<TEvent> : IEventStore<TEvent>
    {
        private readonly Queue<TEvent> _events = new Queue<TEvent>();
        private static readonly int MaxEvents = 25;

        public void Add(TEvent @event)
        {
            lock (_events)
            {
                _events.Enqueue(@event);

                if (_events.Count > MaxEvents)
                {
                    _events.Dequeue();
                }
            }
        }

        public IList<TEvent> GetTopN()
        {
            lock (_events)
            {
                return _events.ToArray();
            }
        }
    }
}
