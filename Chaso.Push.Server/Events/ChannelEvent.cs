using Newtonsoft.Json;
using System;

namespace Chaso.Push.Server.Events
{
    public class ChannelEvent
    {
        private object _data;
        /// <summary>
        /// The name of the event
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The name of the channel the event is associated with
        /// </summary>
        public string ChannelName { get; set; }

        /// <summary>
        /// The date/time that the event was created
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// The data associated with the event
        /// </summary>
        public object Data
        {
            get { return _data; }
            set
            {
                _data = value;
                this.Json = JsonConvert.SerializeObject(_data);
            }
        }

        /// <summary>
        /// A JSON representation of the event data. This is set automatically
        /// when the Data property is assigned.
        /// </summary>
        public string Json { get; private set; }

        public ChannelEvent()
        {
            Timestamp = DateTimeOffset.Now;
        }
        internal static ChannelEvent Subscribe(string connectionId, string channel)
        {
            return new ChannelEvent
            {
                ChannelName = channel,
                Name = $"{channel}.subscribed",
                Data = new
                {
                    ConnectionId = connectionId,
                    ChannelName = channel
                }
            };
        }

        internal static ChannelEvent Unsubscribe(string connectionId, string channel)
        {
            return new ChannelEvent
            {
                ChannelName = channel,
                Name = $"{channel}.unsubscribed",
                Data = new
                {
                    ConnectionId = connectionId,
                    ChannelName = channel
                }
            };
        }
        internal static ChannelEvent ConnectedAdmin(string connectionId)
        {
            return new ChannelEvent
            {
                ChannelName = Constants.AdminChannel,
                Name = "user.connected",
                Data = new
                {
                    ConnectionId = connectionId,
                }
            };
        }
        internal static ChannelEvent DisconnectedAdmin(string connectionId)
        {
            return new ChannelEvent
            {
                ChannelName = Constants.AdminChannel,
                Name = "user.disconnected",
                Data = new
                {
                    ConnectionId = connectionId,
                }
            };
        }

        internal static ChannelEvent TaskChannel(string channel,string eventName, object data)
        {
            return new ChannelEvent
            {
                ChannelName = channel,
                Name = eventName,
                Data = data
            };
        }
    }
}
