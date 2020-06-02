using Microsoft.AspNet.SignalR.Client;
using Pulse.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessMonitor
{
    public class ProcMonitor
    {
        private HubConnection _signalRConnection;
        private IHubProxy _hubProxy;
        //private Process _jobProcess;
        //private Process _monitorProcess;
        //private Process _scheduleProcess;
        //private Process _runProcess;
        private IList<ProcessModel> _processList;
        //private ObservableCollection<ProcessModel> _processList;
        
        public void Monitor()
        {
            MonitorLogging.LogMessage(LoggingLevel.Info, "Starting the InitProcess");
            //_processList = new ObservableCollection<ProcessModel>(ProcCommands.InitProcess());
            _processList = ProcCommands.InitProcess();
            Connect();
            while (true)
            {
                if (_signalRConnection.State == ConnectionState.Connected)
                {
                    foreach (var item in _processList)
                    {
                        double ram = item.ProcessRam.NextValue();
                        SendProxyMessage($"{ram / 1024 / 1024} MB");
                        double cpu = item.ProcessCpu.NextValue();
                        SendProxyMessage(cpu.ToString());
                    }
                    Thread.Sleep(3000);
                    _processList = ProcCommands.InitProcess();
                }

                //foreach (var item in _processList)
                //{
                //    double ram = item.ProcessRam.NextValue();
                //    SendProxyMessage($"{ram / 1024 / 1024} MB");
                //    MonitorLogging.LogMessage(LoggingLevel.Info, $"{ram / 1024 / 1024} MB");
                //    double cpu = item.ProcessCpu.NextValue();
                //    SendProxyMessage(cpu.ToString());
                //    MonitorLogging.LogMessage(LoggingLevel.Info, cpu.ToString());
                //}
                //Thread.Sleep(3000);
            }
        }

        private void SendProxyMessage(string msg)
        {
            _hubProxy.Invoke("ProcMessage", msg);
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
            {
                SendProxyMessage("Connected");
                MonitorLogging.LogMessage(LoggingLevel.Info, "Connected");
            }
            else if (obj.NewState == ConnectionState.Disconnected)
            {
                SendProxyMessage("Disconnected");
                MonitorLogging.LogMessage(LoggingLevel.Info, "Disconnected");
            }
        }
    }
}
