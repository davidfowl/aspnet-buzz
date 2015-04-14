using System.Threading.Tasks;
using Jabbot.ApiHelpers.Github;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace AspNet.Buzz
{
    [HubName("events")]
    public class EventsHub : Hub
    {
        private readonly IEventStore<ApiEvent> _eventStore;
        
        public EventsHub(IEventStore<ApiEvent> eventStore)
        {
            _eventStore = eventStore;
        }
        
        public override Task OnConnected()
        {
            foreach (var @event in _eventStore.GetTopN())
            {
                Clients.Caller.githubEvent(@event);
            }
            
            return base.OnConnected();
        }
    }
}