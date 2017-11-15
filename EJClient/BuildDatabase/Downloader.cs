using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EJClient.BuildDatabase
{
    class Downloader
    {
        public delegate void DownloadingFileHandler(string fileName,byte[] fileData , int fileCount, int readedFileCount);
        public static bool downloadFile(string pagename, int databaseid,string filepath , DownloadingFileHandler callback)
        {
            HttpWebRequest req = null;
            HttpWebResponse res = null;
            System.IO.Stream stream = null;
            bool result = false;
            System.GC.Collect();
            try
            {
                req = HttpWebRequest.Create(Helper.WebSite + "/"+ pagename + "?databaseid=" + databaseid + "&filepath=" + System.Web.HttpUtility.UrlEncode(filepath , System.Text.Encoding.UTF8)) as System.Net.HttpWebRequest;
                req.Headers["Cookie"] = $"WayScriptRemoting={Net.RemotingClient.SessionID}";
                req.AllowAutoRedirect = true;
                req.KeepAlive = false;
                req.Timeout = 20000;
                req.ServicePoint.ConnectionLeaseTimeout = 2 * 60 * 1000;

                res = req.GetResponse() as System.Net.HttpWebResponse;
                stream = res.GetResponseStream();

                using (var fs = System.IO.File.Create(filepath))
                {

                    byte[] header;
                    if (pagename == "DownloadDatabaseCode.aspx")
                    {
                        header = System.Text.Encoding.UTF8.GetBytes(@"
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;

");
                    }
                    else
                    {
                        header = System.Text.Encoding.UTF8.GetBytes(@"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

");
                    }
                    fs.Write(header, 0, header.Length);

                    System.IO.BinaryReader br = new System.IO.BinaryReader(stream);
                    string msg = br.ReadString();
                    if (msg != "start")
                        throw new Exception(msg);

                    int fileCount = br.ReadInt32();
                    int readed = 0;
                    while (true)
                    {
                        string filename = br.ReadString();
                        if (filename == ":end")
                            break;
                        int len = br.ReadInt32();
                        byte[] data = br.ReadBytes(len);

                        readed++;
                        fs.Write(data , 0 , data.Length);
                        callback(filename, data, fileCount, readed);
                    }
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close(); stream.Dispose();
                }
                if (req != null)
                {
                    req.Abort();
                    req = null;
                }
                if (res != null)
                {
                    res.Close();
                    res = null;
                }
            }
            return result;
        }
    }
}
