using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Pulse.SystemMonitor
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        private readonly StringBuilder _aceString;

        public ProjectInstaller()
        {
            InitializeComponent();
            _aceString = new StringBuilder();
        }

        protected override void OnAfterInstall(IDictionary savedState)
        {
            base.OnAfterInstall(savedState);
            using (var serviceController = new ServiceController(SystemMonitorInstaller.ServiceName, Environment.MachineName))
            {
                try
                {
                    serviceController.Start();
                    serviceController.WaitForStatus(ServiceControllerStatus.Running);
                    ServiceAceStrings("SC SDSHOW SystemMonitor");
                    if (string.IsNullOrEmpty(_aceString.ToString()))
                    {
                        return;
                    }

                    string aceString = _aceString.ToString();
                    _aceString.Clear();
                    SetNewAceString(aceString);

                }
                catch (Exception)
                {

                }
            }
        }

        private void SetNewAceString(string aceString)
        {
            string userControls = "(A;;RPWP:::BU)";
            int index = aceString.IndexOf(@")s");
            aceString = aceString.Insert(index + 1, userControls);

            ServiceAceStrings($"SC SDSET SystemMonitor");
        }

        private void ServiceAceStrings(string svcCommand)
        {
            Process p = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "CMD.exe",
                    Arguments = $"/C {svcCommand}",
                    CreateNoWindow = true,
                    ErrorDialog = false,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    Verb = "runas"
                }
            };

            p.EnableRaisingEvents = true;
            p.OutputDataReceived += (s, e) => _aceString.Append(e.Data);
            p.Exited += Proc_Exited;

            p.Start();
            p.BeginOutputReadLine();
            p.WaitForExit();
            p.Close();
        }

        private void Proc_Exited(object sender, EventArgs e)
        {
            // Log results
        }
    }
}
