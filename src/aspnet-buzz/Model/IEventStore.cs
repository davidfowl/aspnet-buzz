using System;
using System.Collections.Generic;

namespace AspNet.Buzz
{
    public interface IEventStore<TEvent>
    {
        void Add(TEvent @event);
        IList<TEvent> GetTopN();
    }
}
