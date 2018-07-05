using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Way.WindowsService
{
    public partial class Service1 : ServiceBase
    {
        System.Diagnostics.Process process;
        public Service1()
        {
            InitializeComponent();
        }


        protected override void OnStart(string[] args)
        {
            using (System.IO.FileStream fs = System.IO.File.Create(AppDomain.CurrentDomain.BaseDirectory + "Way.WindowsService.Log.txt"))
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fs))
            {
                sw.WriteLine("OnStart");
                try
                {
                    sw.WriteLine("Application:" + Config.Instance.Application);
                    sw.WriteLine("Args:" + Config.Instance.Args);
                    process = Process.Start(Config.Instance.Application, Config.Instance.Args);
                   
                    if (process == null)
                    {
                        sw.WriteLine("Process.Start pass. Process is null");
                    }
                    else
                    {
                        sw.WriteLine("Process.Start pass. ProcessID:" + process.Id);
                    }
                }
                catch(Exception ex)
                {
                    sw.WriteLine("error:" + ex.ToString());
                }
            }
        }

        protected override void OnStop()
        {
            try
            {
                process.Kill();
            }
            catch
            {

            }
        }
    }
}
