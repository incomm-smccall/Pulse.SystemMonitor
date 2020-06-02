using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceMonitor
{
    public class ServiceMonitor
    {
        private HubConnection _signalRConnection;
        private IHubProxy _hubProxy;

        public void Monitor()
        {
            Connect();
            while (true)
            {
                if (_signalRConnection.State == ConnectionState.Connected)
                {
                    SendProxyMessage(SrvsMonitorClass.CheckServices("PulseJob"));
                    SendProxyMessage(SrvsMonitorClass.CheckServices("JobsMonitor"));
                    Thread.Sleep(3000);
                }
            }
        }

        private void SendProxyMessage<T>(T sc)
        {
            string msg = string.Empty;
            switch (sc)
            {
                case ServiceController svc:
                    msg = $"{svc.ServiceName} : {svc.Status}";
                    _hubProxy.Invoke("ServiceStatus", msg);
                    break;
                case String message:
                    msg = message;
                    _hubProxy.Invoke("Send", msg);
                    break;
            }
            //ServiceController svc = sc;
            //_hubProxy.Invoke("Send", msg);
        }

        private void Connect()
        {
            _signalRConnection = new HubConnection("http://localhost:8899");
            _signalRConnection.StateChanged += signalRConnection_StateChanged;
            _hubProxy = _signalRConnection.CreateHubProxy("MessageHub");

            try
            {
                _signalRConnection.Start();

                do
                {
                    Thread.Sleep(1000);
                } while (_signalRConnection.State == ConnectionState.Disconnected);
            }
            catch (Exception)
            {

            }
        }

        private void signalRConnection_StateChanged(StateChange obj)
        {
            if (obj.NewState == ConnectionState.Connected)
                SendProxyMessage("Connected");
            else if (obj.NewState == ConnectionState.Disconnected)
                SendProxyMessage("Disconnected");
        }
    }
}
