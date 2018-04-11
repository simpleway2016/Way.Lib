using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EJClient.Forms
{
    /// <summary>
    /// BuildeCode.xaml 的交互逻辑
    /// </summary>
    public partial class BuildeCode : Window
    {
        EJ.Databases m_database;
        int m_databaseID;
        string m_outputFileName;
        string m_pagename;
        public BuildeCode(int databaseid,string filename,string pagename)
        {
            m_pagename = pagename;
            m_outputFileName = filename;

            m_databaseID = databaseid;
            InitializeComponent();
            this.Loaded += BuildeCode_Loaded;
           
        }

        void BuildeCode_Loaded(object sender, RoutedEventArgs e)
        {
            new Thread(() =>
                {
                    try
                    {

                        var database = Helper.Client.InvokeSync<EJ.Databases>("GetDatabase", m_databaseID);

                        BuildDatabase.Downloader.downloadFile(m_pagename, this.m_databaseID, m_outputFileName, new BuildDatabase.Downloader.DownloadingFileHandler(downloading));
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            lblStatus.Content = "Building...";
                        }));



                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            lblStatus.Content = "Output " + database.Name + " Completed!";
                            setOutputText("Target:" + m_outputFileName);
                            btnOK.IsEnabled = true;
                        }));
                    }
                    catch (Exception ex)
                    {
                        this.Dispatcher.Invoke(new Action(() =>
                            {
                                MessageBox.Show(this, ex.GetBaseException().Message);
                                btnOK.IsEnabled = true;
                                this.Close();
                            }));
                    }
                   
                }).Start();
            
        }
        
        private const string CscPath = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe";
        public void Build()
        {
            //string folderPath = m_database.dllPath;
            //if (folderPath.EndsWith("\\") == false)
            //    folderPath += "\\";

            //if (File.Exists(CscPath) == false)
            //{
            //    System.Windows.Forms.MessageBox.Show("无法找到" + CscPath);
            //    return;
            //}
            //string savedDllPath = folderPath + m_database.Name + "DataObjects.dll";
            //string oldResDllPath = "";
            //if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "DataSpace.dll"))
            //{
            //    oldResDllPath = "/reference:\"" + AppDomain.CurrentDomain.BaseDirectory + "DataSpace.dll\"";
            //}
            //ProcessStartInfo startinfo = new ProcessStartInfo(CscPath, @"/target:library /resource:"""+ AppDomain.CurrentDomain.BaseDirectory + @"codes\database.actions"" /platform:anycpu /doc:""" + folderPath + m_database.Name + @"DataObjects.xml"" /out:""" + folderPath + m_database.Name + @"DataObjects.dll"" /reference:""" + AppDomain.CurrentDomain.BaseDirectory + @"EntityDB.Design.dll"" /reference:""" + AppDomain.CurrentDomain.BaseDirectory + @"EntityDB.dll"" " + oldResDllPath + @" /reference:""System.ComponentModel.DataAnnotations.dll"" /reference:""" + AppDomain.CurrentDomain.BaseDirectory + @"Microsoft.EntityFrameworkCore.dll"" """ + AppDomain.CurrentDomain.BaseDirectory + "Codes" + @"\*.cs""");
            //startinfo.RedirectStandardError = true;
            //startinfo.RedirectStandardOutput = true;
            //startinfo.UseShellExecute = false;
            //startinfo.CreateNoWindow = true;
            //startinfo.WindowStyle = ProcessWindowStyle.Hidden;

            //setOutputText("Target:" + savedDllPath);
            //try
            //{
            //    Process p = Process.Start(startinfo);
            //    new Thread(() =>
            //        {
            //            while (true)
            //            {
            //                string line = p.StandardOutput.ReadLine();
            //                if (line == null)
            //                    break;
            //                setOutputText(line);
            //            }
            //        }).Start();
            //    p.WaitForExit();
                
            //}
            //catch (Exception ex)
            //{
            //    System.Windows.Forms.MessageBox.Show(ex.GetBaseException().Message);
            //}
        }

        void setOutputText(string text)
        {
            this.Dispatcher.Invoke(new Action(() =>
                {
                    txtInfo.AppendText(text + "\r\n");
                }));
        }

        void downloading(string fileName, byte[] fileData, int fileCount, int readedFileCount)
        {
            this.Dispatcher.Invoke(() =>
                {
                    progressBar.Maximum = fileCount;
                    progressBar.Value = Math.Min(fileCount , readedFileCount);
                });
           
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (btnOK.IsEnabled == false)
                e.Cancel = true;
            base.OnClosing(e);
        }
    }
}
