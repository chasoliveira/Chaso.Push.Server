using System;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Owin.Hosting;
using System.ServiceProcess;
using Chaso.Push.Server.Events;
using NLog;

namespace Chaso.Push.Server
{
    static class Program
    {
        public static string ServiceName { get; private set; }
        public static IDisposable WebAppPushSignalR;
        public static ILogger Log;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">todo: describe args parameter on Main</param>
        static void Main(string[] args)
        {
            Log = NLog.LogManager.GetCurrentClassLogger();

            ServiceName = "Chaso.Push.Server";
            if (!Environment.UserInteractive)
            {
                try
                {
                    Log.Info("Start as Service.");
                    using (var servicesToRun = new WindowsService())
                        ServiceBase.Run(servicesToRun);
                }
                catch (Exception exception)
                {
                    Log.Log(LogLevel.Error, exception);
                }
            }
            else
            {
                Log.Info("Start as Console.");
                Start(args);
            }
        }

        internal static void Stop()
        {
            Log.Info("Finishing Push Service.");
            WebAppPushSignalR?.Dispose();
        }

        public static void Start(string[] args)
        {
            SetWebService();
        }

        private static void SetWebService()
        {
            try
            {
                var baseAddress = $"{Properties.Settings.Default.ServiceUrl}:{Properties.Settings.Default.ServicePort}/";
                //Start Owin Host
                WebAppPushSignalR = WebApp.Start<Startup>(url: baseAddress);

                StartListenSignalR(baseAddress);

                if (Environment.UserInteractive)
                    Console.ReadKey();
            }
            catch (Exception exception)
            {
                Log.Log(LogLevel.Error, exception);
            }
        }

        private static void StartListenSignalR(string baseAddress)
        {
            // Let's wire up a SignalR client here to easily inspect what
            //  calls are happening
            //
            var hubConnection = new HubConnection(baseAddress);
            IHubProxy eventHubProxy = hubConnection.CreateHubProxy(nameof(EventHub));
            eventHubProxy.On<string, ChannelEvent>("OnEvent", (channel, ev)
                => Log.Info($"Event received on Channel: {channel}, Event: {ev.Name}, Origin: {ev.Origin}"));

            hubConnection.Start().Wait();
            Log.Info($"Server is running on {baseAddress}");

            // Join the channel for task updates in our console window
            //
            var channels = Properties.Settings.Default.Channels;
            foreach (var channel in channels)
            {
                eventHubProxy.Invoke("Subscribe", channel);
                Log.Info("Subscribe on channel: {0}", channel);
            }
        }
    }
}
