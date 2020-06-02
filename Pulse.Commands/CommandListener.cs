using Pulse.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pulse.Commands
{
    public class CommandListener
    {
        public ManualResetEvent allDone = new ManualResetEvent(false);

        public CommandListener()
        {

        }

        public void StartListening()
        {
            IPEndPoint localEp = new IPEndPoint(IPAddress.Any, 9998);
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                listener.Bind(localEp);
                listener.Listen(100);
                while (true)
                {
                    allDone.Reset();
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
                    allDone.WaitOne();
                }
            }
            catch (Exception ex)
            {
                MonitorLogging.LogErrorMessage("The listener encountered an error", ex);
            }
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            allDone.Set();
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            StateObject state = new StateObject();
            state.WorkSocket = handler;
            handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            string content = string.Empty;
            string result = string.Empty;
            bool sendResult = true;
            //ProcessCommands processCmds = new ProcessCommands();

            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.WorkSocket;

            int bytesRead = handler.EndReceive(ar);
            MonitorLogging.LogMessage(LoggingLevel.Info, $"Message has been read. BytesRead={bytesRead}");
            if (bytesRead > 0)
            {
                state.Sb.Append(Encoding.ASCII.GetString(state.Buffer, 0, bytesRead));
                MonitorLogging.LogMessage(LoggingLevel.Info, $"Message sent: {state.Sb}");
                content = state.Sb.ToString();
                MonitorLogging.LogMessage(LoggingLevel.Info, $"The content of the message: {content}");
                if (content.IndexOf(":", StringComparison.Ordinal) > -1)
                {
                    MonitorLogging.LogMessage(LoggingLevel.Info, $"Message content {content}");
                    string[] contentArray = content.Split(':');
                    switch (contentArray[0])
                    {
                        case "SyncPlugins":
                            //result = processCmds.SyncPluginsCommand();
                            break;
                        case "ServiceStatus":
                            //if (string.IsNullOrEmpty(contentArray[1]))
                                //result = processCmds.JobServiceStatusCommand("/C SC Query " + contentArray[1]);
                            //else
                                //result = "No service found";
                            break;
                        case "JobServiceStart":
                            //result = processCmds.JobServiceStatusCommand("/C SC START PulseJob");
                            //result = processCmds.StartJobService();
                            break;
                        case "JobServiceStop":
                            //result = processCmds.JobServiceStatusCommand("/C SC STOP PulseJob");
                            //result = processCmds.StopJobService();
                            break;
                        case "JobServiceCycle":
                            //result = processCmds.JobServiceCycleCommand();
                            //sendResult = false;
                            break;
                    }
                    MonitorLogging.LogMessage(LoggingLevel.Info, $"Result from process commands. Result={result}");
                    if (sendResult)
                        Send(handler, result);
                }
                else
                {
                    handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                }
            }
        }

        private void Send(Socket handler, string data)
        {
            try
            {
                byte[] byteData = Encoding.ASCII.GetBytes(data);
                handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
            }
            catch
            {

            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            throw new NotImplementedException();
        }
    }

    public class StateObject
    {
        public const int BufferSize = 4096;
        private Socket _workSocket;
        public Socket WorkSocket
        {
            get => _workSocket;
            set
            {
                if (value != _workSocket)
                    _workSocket = value;
            }
        }
        public byte[] Buffer { get; set; } = new byte[BufferSize];
        public StringBuilder Sb { get; set; } = new StringBuilder();
    }
}
