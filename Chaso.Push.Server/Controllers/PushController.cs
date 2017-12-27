using Chaso.Push.Server.Events;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Web.Http;

namespace Chaso.Push.Server.Controllers
{
    /// <summary>
    /// Api to send a notification by SingnalR
    /// </summary>
    [RoutePrefix("api/push")]
    public class PushController : ApiController
    {
        private IHubContext _context;
        /// <summary>
        /// Constructor of Controller to notify SugnalR
        /// </summary>
        /// <param name="context"></param>
        public PushController(IHubContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Return all Channel listening by SignalR
        /// </summary>
        /// <returns></returns>
        [Route("channels")]
        [HttpGet]
        public async Task<IHttpActionResult> GetChannels()
        {
            var channels = await Task.Run(() => Properties.Settings.Default.Channels);

            return Ok(JsonConvert.SerializeObject(channels));
        }
        /// <summary>
        /// Notify a channel that an event was trigged
        /// </summary>
        /// <param name="notify"></param>
        /// <returns>string message</returns>
        [Route("notify")]
        [HttpPost]
        public async Task<IHttpActionResult> Notify(Notify notify)
        {
            await PublishEvent(notify.Origin, notify.Channel, notify.EventName, notify);
            return Ok(string.Format($"Event {notify.EventName} on ${notify.Channel} from {notify.Origin} completed!"));
        }

        private async Task PublishEvent(string origin, string channel, string eventName, Notify status)
        {
            var result = await Task.Run(() => _context.Clients.Group(channel).OnEvent(channel, ChannelEvent.TaskChannel(origin, channel, eventName, status.Data)));
        }
    }
}
