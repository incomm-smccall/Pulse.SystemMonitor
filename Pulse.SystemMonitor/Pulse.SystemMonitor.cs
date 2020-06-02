using Microsoft.AspNet.SignalR.Client;
using ProcessMonitor;
using Pulse.Shared;
using Pulse.SignalRService;
using ServiceMonitor;
using System;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace Pulse.SystemMonitor
{
    public partial class SystemMonitor : ServiceBase
    {
        //private static HubConnection _signalRConnection;
        //private static IHubProxy _hubProxy;

        public SystemMonitor()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            MonitorLogging.LogMessage(LoggingLevel.Info, "Starting the Message service.");
            Task.Factory.StartNew(() => new MessageService().OnStart());
            //Task.Factory.StartNew(() => new ProcMonitor().Monitor());
            Task.Factory.StartNew(() => new ServiceMonitor.ServiceMonitor().Monitor());
            //Task.Factory.StartNew(() => MonitorServices());
        }

        protected override void OnStop()
        {
        }

        //private static void MonitorServices()
        //{
        //    Connect();
        //    while (true)
        //    {
        //        if (_signalRConnection.State == ConnectionState.Connected)
        //        {
        //            SendProxyMessage(SrvsMonitorClass.CheckServices("PulseJob"));
        //            SendProxyMessage(SrvsMonitorClass.CheckServices("JobsMonitor"));
        //            Thread.Sleep(3000);
        //        }
        //    }
        //}

        //private static void SendProxyMessage<T>(T sc)
        //{
        //    string msg = string.Empty;
        //    switch(sc)
        //    {
        //        case ServiceController svc:
        //            msg = $"{svc.ServiceName} : {svc.Status}";
        //            break;
        //        case String message:
        //            msg = message;
        //            break;
        //    }
        //    //ServiceController svc = sc;
        //    _hubProxy.Invoke("Send", msg);
        //}

        //private static void JobSvcModel_SvcStatusChanged(object sender, ServiceChangedEventArgs e)
        //{
        //    _hubProxy.Invoke("Send", $"{e.Service.SvcName} : {e.Service.SvcStatus}");
        //}

        //private static void Connect()
        //{
        //    _signalRConnection = new HubConnection("http://localhost:8899");
        //    _signalRConnection.StateChanged += signalRConnection_StateChanged;
        //    _hubProxy = _signalRConnection.CreateHubProxy("MessageHub");
            
        //    try
        //    {
        //        _signalRConnection.Start();

        //        do
        //        {
        //            Thread.Sleep(1000);
        //        } while (_signalRConnection.State == ConnectionState.Disconnected);
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}

        //private static void signalRConnection_StateChanged(StateChange obj)
        //{
        //    if (obj.NewState == ConnectionState.Connected)
        //        SendProxyMessage("Connected");
        //    else if (obj.NewState == ConnectionState.Disconnected)
        //        SendProxyMessage("Disconnected");
        //}
    }
}
