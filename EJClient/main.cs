using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EJClient
{
    class main
    {
        static AutoUpdate autoUpdate;
        [STAThread]//single thread apartment
        static void Main(string[] parameters)
        {
            // 定义Application对象作为整个应用程序入口  
            Application app = new Application();


            // 方法一：调用Run方法，参数为启动的窗体对象 ，也是最常用的方法 
            if (parameters != null && parameters.Length > 0 && parameters[0] == "/sql")
            {
                app.Run(new Forms.DatabaseUpdate());
            }
            else
            {
                autoUpdate = new AutoUpdate();
                //app.Run(new Forms.DatabaseUpdate());
                //app.Run(new Forms.BugCenter.BugRecorder());
                //app.Run(new Forms.BugCenter.BugEditor());
                app.Run(new Login());
            }


            // 方法二：指定Application对象的MainWindow属性为启动窗体，然后调用无参数的Run方法  
            //Window2 win = new Window2();  
            //app.MainWindow = win; 
            //win.Show();  // win.Show()是必须的，否则无法显示窗体
            //app.Run();  


            // 方法三：通过Url的方式启动
            //app.StartupUri = new Uri("Window2.xaml", UriKind.Relative); 
            //app.Run();
        }
    }
}
