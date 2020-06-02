using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessMonitor
{
    public class ProcessModel
    {
        public string ProcessName { get; set; }
        public Process Process { get; set; }
        public PerformanceCounter ProcessRam { get; set; }
        public PerformanceCounter ProcessCpu { get; set; }
        public bool Ignore { get; set; }
    }
}
