using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Way.Lib.ScriptRemoting.CodeBuilder
{
    public partial class MainForm : Form
    {
      
        public MainForm()
        {
            InitializeComponent();
            if(Directory.Exists(Application.StartupPath + "\\temp") == false)
            {
                Directory.CreateDirectory(Application.StartupPath + "\\temp");
            }
            Stream stream = typeof(MainForm).Assembly.GetManifestResourceStream("Way.Lib.ScriptRemoting.CodeBuilder.index.html");
            byte[] htmlBytes = new byte[stream.Length];
            stream.Read(htmlBytes, 0, htmlBytes.Length);
            File.WriteAllBytes(Application.StartupPath + "\\temp\\index.html", htmlBytes);

            stream = typeof(MainForm).Assembly.GetManifestResourceStream("Way.Lib.ScriptRemoting.CodeBuilder.jquery.js");
            htmlBytes = new byte[stream.Length];
            stream.Read(htmlBytes, 0, htmlBytes.Length);
            File.WriteAllBytes(Application.StartupPath + "\\temp\\jquery.js", htmlBytes);

            webBrowser1.Navigate("file:///"+ Application.StartupPath + "\\temp\\index.html");
        }

       
    }
}
