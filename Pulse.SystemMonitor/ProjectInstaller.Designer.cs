namespace Pulse.SystemMonitor
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SystemMonitorProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.SystemMonitorInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // SystemMonitorProcessInstaller
            // 
            this.SystemMonitorProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.SystemMonitorProcessInstaller.Password = null;
            this.SystemMonitorProcessInstaller.Username = null;
            // 
            // SystemMonitorInstaller
            // 
            this.SystemMonitorInstaller.Description = "Monitor all services, processes and Jobs associated with Pulse.";
            this.SystemMonitorInstaller.DisplayName = "System Pulse Monitor";
            this.SystemMonitorInstaller.ServiceName = "SystemPulseMonitor";
            this.SystemMonitorInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.SystemMonitorProcessInstaller,
            this.SystemMonitorInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller SystemMonitorProcessInstaller;
        private System.ServiceProcess.ServiceInstaller SystemMonitorInstaller;
    }
}