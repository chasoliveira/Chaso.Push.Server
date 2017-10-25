using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace Chaso.Push.Server.Events
{
    public class EventHub: Hub
    {
        public async Task Subscribe(string channel)
        {
            await Groups.Add(Context.ConnectionId, channel);

            var ev = ChannelEvent.Subscribe(Context.ConnectionId, channel);
            await Publish(ev);
        }

        public async Task Unsubscribe(string channel)
        {
            await Groups.Remove(Context.ConnectionId, channel);
            var ev = ChannelEvent.Unsubscribe(Context.ConnectionId, channel);
            await Publish(ev);
        }


        public override Task OnConnected()
        {
            var ev = ChannelEvent.ConnectedAdmin(Context.ConnectionId);
            
            Publish(ev);

            return base.OnConnected();
        }


        public override Task OnDisconnected(bool stopCalled)
        {
            var ev = ChannelEvent.DisconnectedAdmin(Context.ConnectionId);
            Publish(ev);

            return base.OnDisconnected(stopCalled);
        }

        public Task Publish(ChannelEvent ev)
        {
            Clients.Group(ev.ChannelName).OnEvent(ev.Name, ev);
            return Task.FromResult(0);
        }
    }
}
