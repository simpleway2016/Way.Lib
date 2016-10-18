using AppLib.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace AppLib.Http
{
    class HttpProcessor
    {
        internal HttpApplication m_app;
        HttpWorkerRequest m_wr;
        internal string m_boundary;
        internal int m_requestLength;
        internal int m_readed = 0;
        internal byte[] TotalBuffer = new byte[5096];
        internal int m_bufferPosition = 0;
        internal int m_TotalLength = 0;
        internal List<limit> extLimits = null;
        internal JsonResult jsonResult = new JsonResult();
        public HttpProcessor(HttpApplication app )
        {
            m_app = app;
            string contenttype = app.Request.ContentType;
            m_boundary = Regex.Match(contenttype, @"boundary=(?<a>(\w|-)+)").Groups["a"].Value;
            IServiceProvider provider = (IServiceProvider)app.Context;
            m_wr = (HttpWorkerRequest)provider.GetService(typeof(HttpWorkerRequest));
            m_requestLength = m_wr.GetTotalEntityBodyLength();
        }

        internal void resetTotalBuffer()
        {
            if (m_bufferPosition > 0)
            { 
                byte[] bs = new byte[m_TotalLength];
                Array.Copy(TotalBuffer, m_bufferPosition, bs, 0, bs.Length);
                m_bufferPosition = 0;
                Array.Copy(bs, 0, TotalBuffer, 0, bs.Length);
            }
        }

        internal void pushToBuffer()
        {
            if (m_readed < m_requestLength && TotalBuffer.Length > m_TotalLength)
            {
                int read = m_wr.ReadEntityBody(TotalBuffer, m_TotalLength, Math.Min(m_requestLength - m_readed, TotalBuffer.Length - m_TotalLength));
                m_readed += read;
                m_TotalLength += read;
            }
        }

        internal byte[] getLineBytes()
        {
            pushToBuffer();
            bool finded换行 = false;
            List<byte> bs = new List<byte>();
            for (int i = 0; i < m_TotalLength; i++)
            {
                bs.Add(TotalBuffer[i]);
                if (TotalBuffer[i] == 0xa && i > 0 && TotalBuffer[i - 1] == 0xd)
                {
                    finded换行 = true;
                    m_bufferPosition = i + 1;
                    m_TotalLength -= m_bufferPosition;
                    break;
                }
            }
            if (!finded换行)
            {
                m_bufferPosition = m_TotalLength / 2;
                m_TotalLength -= m_bufferPosition;
                bs.RemoveRange(m_bufferPosition, bs.Count - m_bufferPosition);
            }
            resetTotalBuffer();
            
            return bs.ToArray();

        }

        internal string readLine()
        {
            pushToBuffer();
            List<byte> bs = new List<byte>();
            for (int i = 0; i < m_TotalLength; i++)
            {
                if (TotalBuffer[i] == 0xd)
                {
                    m_bufferPosition = i + 2;
                    m_TotalLength -= m_bufferPosition;
                    break;
                }
                else
                {
                    bs.Add(TotalBuffer[i]);
                }
            }
            resetTotalBuffer();
            return System.Text.Encoding.UTF8.GetString(bs.ToArray());
        }

      
         FormEntity getNewEntity()
        {
            string line = readLine();
            if (line.Contains(m_boundary))
            {
                if (line.EndsWith("--"))
                    return null;
                else
                {
                    return new FormEntity(this);
                }
            }
            return null;
        }
         internal FormEntity m_nextEntity = null;
        public FormEntity Process()
        {
            FormEntity entity = null;
            if (m_nextEntity != null)
            {
                entity = m_nextEntity;
                m_nextEntity = null;
                if (entity.isEnd)
                    return null;
            }
            else
            {
                entity = getNewEntity();
            }
           
            return entity;
        }
    }

    class FormEntity
    {
        public delegate void GettingFileDataHandler(FormEntity entity ,int totalLength,int readedLength, byte[] data , bool finished);
        public event GettingFileDataHandler GettingFileData;
        internal HttpProcessor m_processor;
        public string Name;
        public string TempFolder;
        public string Guid;
        public string FileName;
        internal int FileWrited = 0;
        public System.IO.FileStream FileStream;
        public string value;
        int SizeLimit = -1;
        public bool isEnd = false;
        List<byte> m_buffer = new List<byte>();
        internal limit mylimit = null;
        public FormEntity(HttpProcessor  processor)
        {
            m_processor = processor;
        }

        internal void ProcessHeader()
        {
            while (true)
            {
                string line = m_processor.readLine();
                if (line.StartsWith("Content-Disposition:"))
                {
                    try
                    {
                        Name = Regex.Match(line, @"name=\""(?<a>(.)+)\""").Groups["a"].Value;
                    }
                    catch
                    {
                    }
                    try
                    {
                        FileName = Regex.Match(line, @"filename=\""(?<a>(.)+)\""").Groups["a"].Value;
                        
                    }
                    catch
                    {
                    }
                    if (!string.IsNullOrEmpty(FileName) && m_processor.extLimits != null)
                    {
                        string ext = Path.GetExtension(FileName).ToLower();
                        mylimit = m_processor.extLimits.FirstOrDefault(m => m.ext.ToLower() == ext);
                        if (mylimit == null)
                            throw (new Exception(ext + "文件不允许上传"));
                        else
                        {
                            SizeLimit = mylimit.size;
                        }
                    }
                }
                else if (line.Length == 0)
                {
                    //进入正文
                    m_processor.m_nextEntity = this.Process();
                    return;
                }
            }
        }

        void OnGetedFileData(byte[] data,bool finished)
        {
            if (GettingFileData != null)
                GettingFileData(this,m_processor.m_requestLength , m_processor.m_readed , data, finished);
        }

        internal FormEntity Process()
        {
            while (true)
            {
                byte[] lineBytes = m_processor.getLineBytes();
                if (lineBytes.Length == 0)
                {
                    m_buffer.RemoveRange(m_buffer.Count - 2, 2);
                    if (string.IsNullOrEmpty(FileName))
                    {
                        value = System.Text.Encoding.UTF8.GetString(m_buffer.ToArray());
                    }
                    else
                    {
                        OnGetedFileData(m_buffer.ToArray() , true);
                    }
                    break;
                }
                if (lineBytes.Length >= m_processor.m_boundary.Length && lineBytes.Length < m_processor.m_boundary.Length + 10)
                {
                    string linetext = System.Text.Encoding.UTF8.GetString(lineBytes).Trim();
                    if (linetext.Contains(m_processor.m_boundary))
                    {
                        m_buffer.RemoveRange(m_buffer.Count - 2, 2);
                        if (string.IsNullOrEmpty(FileName))
                        {
                            value = System.Text.Encoding.UTF8.GetString(m_buffer.ToArray());
                        }
                        else
                        {
                            OnGetedFileData(m_buffer.ToArray(), true);
                        }
                        return new FormEntity(m_processor)
                        {
                            isEnd = linetext.EndsWith("--")
                        };
                    }
                }
                m_buffer.AddRange(lineBytes);
                if (string.IsNullOrEmpty(FileName) == false && m_buffer.Count >= 2048)
                {
                    if (m_buffer[m_buffer.Count - 1] == 0xa && m_buffer[m_buffer.Count - 2] == 0xd)
                    {
                        m_buffer.RemoveRange(m_buffer.Count - 2, 2);
                        OnGetedFileData(m_buffer.ToArray(), false);
                        m_buffer.Clear();
                        m_buffer.Add( 0xd );
                        m_buffer.Add(0xa);
                    }
                    else
                    {
                        //is file
                        OnGetedFileData(m_buffer.ToArray(), false);
                        m_buffer.Clear();
                    }
                }
            }

            return null;
        }
    }
}
