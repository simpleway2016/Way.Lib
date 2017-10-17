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
            process = Process.Start(Config.Application, Config.Args);
        }

        protected override void OnStop()
        {
            process.Kill();
        }
    }
}
