
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

using System.Runtime.InteropServices;

namespace Way.Lib
{
    public class Runner 
    {
        public static string Exec(string filename, string args)
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

        /// <summary>
        /// 调用命令，并获取output的信息
        /// linux需要安装unbuffer，如centos：yum install expect-devel
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="args"></param>
        /// <param name="outputAction">获取output信息，如果返回值不为null，则表示要输入的信息，输入的信息如果要靠回车换行结束，应该这样：myinfo\r\n，字符串需要包含\r\n</param>
        public static void Exec(string filename, string args, Func<string,string> outputAction)
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

        /// <summary>
        /// linux需要安装unbuffer，如centos：yum install expect-devel
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static Process OpenProcess(string filename, string args)
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
