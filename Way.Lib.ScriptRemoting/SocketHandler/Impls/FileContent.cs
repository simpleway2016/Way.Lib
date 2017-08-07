using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Way.Lib.ScriptRemoting
{
    public class FileContent
    {
        public string ContentType
        {
            get;
            set;
        }

        public int ContentLength
        {
            get;
            set;
        }

        public string FileName
        {
            get;
            set;
        }
        protected System.IO.Stream Stream;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="contentType">如："image/png"</param>
        public FileContent(System.IO.Stream stream , string contentType)
        {
            this.Stream = stream;
            this.ContentLength = (int)stream.Length;
            stream.Position = 0;
            this.ContentType = contentType;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="contentType">如："image/png"</param>
        public FileContent(string filepath, string contentType):this(System.IO.File.OpenRead(filepath) , contentType)
        {

        }

        internal void Output()
        {
            try
            {
                if (this.FileName != null)
                {
                    RemotingContext.Current.Response.Headers["Content-Disposition"] = "attachment;filename=" + WebUtility.UrlEncode(this.FileName);
                }
                RemotingContext.Current.Response.ContentLength = this.ContentLength;
                if (this.ContentType.IsNullOrEmpty() == false)
                {
                    RemotingContext.Current.Response.Headers["Content-Type"] = this.ContentType;
                }
                byte[] bs = new byte[8096];
                int done = 0;
                while (true)
                {
                    var readed = this.Stream.Read(bs, 0, bs.Length);
                    RemotingContext.Current.Response.Write(bs, 0, readed);
                    done += readed;
                    if (done >= this.ContentLength)
                        break;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                this.Stream.Dispose();
            }
        }
    }
}
