namespace Chaso.Push.Server
{
    /// <summary>
    /// Class for Send and receive a Notification
    /// </summary>
    public class Notify
    {
        /// <summary>
        /// Nome of the Channel to interact
        /// </summary>
        public string Channel { get; set; }
        /// <summary>
        /// Name of Evento to execute in Channel
        /// </summary>
        public string EventName { get; set; }
        /// <summary>
        /// Object to Send and Receive by Notification
        /// </summary>
        public object Data { get; set; }
    }
}
