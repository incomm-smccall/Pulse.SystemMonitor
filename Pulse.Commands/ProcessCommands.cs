using Pulse.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pulse.Commands
{
    public static class ProcessCommands
    {
        private static StringBuilder Result = new StringBuilder();

        public static string SystemCommand(string filename, string args)
        {
            Task taskSync = Task.Run(() =>
            {
                Process syncProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        //FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Pulse.PluginSync.exe"),
                        FileName = filename,
                        Verb = "runas",
                        Arguments = args,
                        CreateNoWindow = true,
                        ErrorDialog = false,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    }
                };
                syncProcess.EnableRaisingEvents = true;
                syncProcess.OutputDataReceived += ProcessOutput;
                syncProcess.ErrorDataReceived += ProcessError;
                syncProcess.Exited += ProcessExited;

                syncProcess.Start();
                syncProcess.BeginOutputReadLine();
                syncProcess.BeginErrorReadLine();
                //runResult = syncProcess.StandardOutput.ReadToEnd();
                syncProcess.WaitForExit();
                syncProcess.Close();
            });
            taskSync.Wait();
            return Result.ToString();
        }

        private static void ProcessOutput(object sender, DataReceivedEventArgs output)
        {
            if (!string.IsNullOrEmpty(output.Data))
            {
                Result.AppendLine(output.Data);
                MonitorLogging.LogMessage(LoggingLevel.Info, $"The request completed at {DateTime.Now}. Request result: {output.Data}");
            }
        }

        private static void ProcessError(object sender, DataReceivedEventArgs error)
        {
            if (!string.IsNullOrEmpty(error.Data))
            {
                Result.AppendLine(error.Data);
                MonitorLogging.LogMessage(LoggingLevel.Error, $"The request failed to complete at {DateTime.Now}. Process result: {error.Data}");
            }
        }

        private static void ProcessExited(object sender, EventArgs e)
        {
            string msg = $"The request exited at {DateTime.Now}";
            Result.AppendLine(msg);
            MonitorLogging.LogMessage(LoggingLevel.Info, msg);
        }
    }
}
