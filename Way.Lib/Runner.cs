
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

using System.Runtime.InteropServices;

namespace Way.Lib
{
    class Runner 
    {
        public string Exec(string filename, string args)
        {
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(filename);
            startInfo.Arguments = args;
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            var process = System.Diagnostics.Process.Start(startInfo);
            process.WaitForExit();
            return process.StandardOutput.ReadToEnd();
        }

        public void Exec(string filename, string args, Func<string,string> outputAction)
        {
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                args = $"-p {filename} {args}";
                filename = "unbuffer";
            }
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(filename);
            startInfo.Arguments = args;
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = true;
            var process = new System.Diagnostics.Process();
            process.StartInfo = startInfo;
            process.OutputDataReceived += (s, e) =>
            {
                if (e.Data != null)
                {
                    var toInput = outputAction(e.Data);
                    if (!string.IsNullOrEmpty(toInput))
                    {
                        process.StandardInput.Write(toInput);
                    }
                }
            };

            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();

        }

        public Process OpenProcess(string filename, string args)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                args = $"-p {filename} {args}";
                filename = "unbuffer";
            }
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(filename);
            startInfo.Arguments = args;
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = true;
            var process = new System.Diagnostics.Process();
            process.StartInfo = startInfo;
            process.Start();
            return process;
        }
    }
}
