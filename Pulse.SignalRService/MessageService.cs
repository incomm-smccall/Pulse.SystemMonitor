using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using Pulse.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(Pulse.SignalRService.Startup))]
namespace Pulse.SignalRService
{
    public class MessageService : IDisposable
    {
        protected IDisposable _signalRApp = null;
        //private Thread _mainThread;
        private static readonly Lazy<MessageService> _instance = new Lazy<MessageService>(() => new MessageService(GlobalHost.ConnectionManager.GetHubContext<MessageHub>().Clients));

        public static MessageService Instance
        {
            get => _instance.Value;
        }

        private IHubConnectionContext<dynamic> Clients { get; set; }

        public MessageService()
        {

        }

        public MessageService(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;
        }

        public void OnStart()
        {
            MonitorLogging.LogMessage(LoggingLevel.Info, "Starting the WebApp service...");
            string url = "http://localhost:8899";
            try
            {
                //_signalRApp = WebApp.Start(url);
                MonitorLogging.LogMessage(LoggingLevel.Info, "WebApp is starting...");
                //_mainThread = new Thread(new ParameterizedThreadStart(RunService));
                //_mainThread.Start(DateTime.MinValue);
                using (WebApp.Start(url))
                {
                    Thread.Sleep(-1);
                }
            }
            catch (Exception ex)
            {
                MonitorLogging.LogErrorMessage($"There was an error starting the WebApp service:", ex);
            }

        }

        public void RunService(object timeToComplete)
        {
            MonitorLogging.LogMessage(LoggingLevel.Info, "RunService is starting...");
            DateTime dtTimeToComplete = timeToComplete != null ? Convert.ToDateTime(timeToComplete) : DateTime.MaxValue;
            MonitorLogging.LogMessage(LoggingLevel.Info, $"The DateTime is now {dtTimeToComplete}");
            while (DateTime.UtcNow < dtTimeToComplete)
            {
                MonitorLogging.LogMessage(LoggingLevel.Info, "In while loop...");
                Thread.Sleep(200);
            }
        }

        public void Dispose()
        {
            _signalRApp?.Dispose();
        }
    }

}
