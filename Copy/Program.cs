using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Copy
{
    public class DstSource
    {
        public string dst;
        public string src;
    }
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] jsonString)
        {
            //System.Diagnostics.Debugger.Launch();
            var ps = System.Diagnostics.Process.GetProcessesByName("EJClient");
            foreach (var p in ps)
            {
                p.Kill();
                p.WaitForExit();
            }
            var json = new System.Web.Script.Serialization.JavaScriptSerializer();
            DstSource ds = json.Deserialize<DstSource>(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(jsonString[0])));
            try
            {
                CopyDirectory(ds.src, ds.dst);

                var files = System.IO.Directory.GetFiles(ds.src);
                foreach (var f in files)
                    System.IO.File.Delete(f);

                System.Diagnostics.Process.Start("EJClient.exe");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 拷贝文件夹
        /// </summary>
        /// <param name="srcdir"></param>
        /// <param name="desdir"></param>
        internal static void CopyDirectory(string srcdir, string desdir)
        {
            if (desdir.EndsWith("\\") == false)
                desdir += "\\";

            var dirs = System.IO.Directory.GetDirectories(srcdir);
            foreach (string path in dirs)
            {
                string foldername = System.IO.Path.GetFileName(path);
                if (System.IO.Directory.Exists(desdir + foldername) == false)
                    System.IO.Directory.CreateDirectory(desdir + foldername);

                CopyDirectory(path, desdir + foldername);
            }

            var files = System.IO.Directory.GetFiles(srcdir);
            foreach (string path in files)
            {
                string filename = System.IO.Path.GetFileName(path);
                if (System.IO.File.Exists(desdir + filename))
                    System.IO.File.Delete(desdir + filename);

                File.Copy(path, desdir + filename);
            }
        }
    }
}
