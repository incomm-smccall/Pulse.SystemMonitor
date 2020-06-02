using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessMonitor
{
    public static class ProcCommands
    {
        public static IList<ProcessModel> InitProcess()
        {
            IList<ProcessModel> procList = new List<ProcessModel>();
            List<string> processlist = new List<string>
            {
                "Pulse.Job", "Pulse.JobsMonitor", "ScheduledJob", "RunNow"
            };

            foreach (string proc in processlist)
            {
                ProcessModel procModel = new ProcessModel();
                procModel.ProcessName = proc;
                procModel.Process = Process.GetProcessesByName(proc).FirstOrDefault();
                if (procModel.Process == null) continue;
                procModel.ProcessRam = new PerformanceCounter("Process", "Working Set", procModel.Process.ProcessName);
                procModel.ProcessCpu = new PerformanceCounter("Process", "% Processor Time", procModel.Process.ProcessName);
                procList.Add(procModel);
            }
            return procList;
        }
    }
}
