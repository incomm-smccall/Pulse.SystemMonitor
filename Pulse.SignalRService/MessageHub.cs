using Microsoft.AspNet.SignalR;
using ProcessMonitor;
using Pulse.Shared;
using System.Threading.Tasks;

namespace Pulse.SignalRService
{
    public class MessageHub : Hub
    {
        public void ServiceStatus(string message)
        {
            Clients.All.receiveStatusUpdate(message);
            //Clients.All.receiveMonitorStatusUpdate(message);
        }

        public void PluginProcess(string message)
        {
            Clients.Caller.addMessage(message);
        }

        public void Send(string message)
        {
            Clients.All.addMessage(message);
        }

        public void Command(string command)
        {
            Clients.All.addMessage(command);
        }

        public async Task GetProcessList(string processValue)
        {

            await Clients.All.procMessage(processValue);
        }
    }
}
