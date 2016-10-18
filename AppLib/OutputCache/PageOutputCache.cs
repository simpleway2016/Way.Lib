using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

namespace AppLib
{
    class PageCache
    {
        public string FilePath;
        public DateTime ExpiredTime;
        /// <summary>
        /// 如果不为空，表示以此文件的修改时间为准，如果修改时间大于ExpiredTime，则表示过期
        /// </summary>
        public string PhysicalPath;
    }
    public class PageOutputCache
    {
        static int toDeleteFilesIndex = 0;
        static System.Collections.Hashtable toDeleteFiles = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
        static System.Collections.Hashtable caches = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
        int m_seconds;
        bool cacheByURL;
        public PageOutputCache(int seconds, bool cacheByURL)
        {
            this.cacheByURL = cacheByURL;
            m_seconds = seconds;
            if (!inited)
                init();
        }
        static void deleteFiles()
        {
            while (true)
            {
                if (toDeleteFiles.Keys.Count > 0)
                {
                    bool deleted = false;
                    foreach (object key in toDeleteFiles.Keys)
                    {
                        PageCache cache = toDeleteFiles[key] as PageCache;
                        if ((DateTime.Now - cache.ExpiredTime).TotalSeconds > 10)
                        {
                            deleted = true;
                            toDeleteFiles.Remove(key);
                            try
                            {
                                File.Delete(cache.FilePath);
                            }
                            catch
                            {
                            }
                            
                        }
                        break;
                    }
                    if(deleted)
                        continue;
                }
                System.Threading.Thread.Sleep(10000);
            }
        }

        static bool inited = false;
        static void init()
        {
            inited = true;
            new System.Threading.Thread(deleteFiles).Start();
            string folder = HttpContext.Current.Server.MapPath("/_temp_html");
            if (Directory.Exists(folder) == false)
                Directory.CreateDirectory(folder);
            string[] files = System.IO.Directory.GetFiles(folder);
            foreach (string file in files)
            {
                try
                {
                    System.IO.File.Delete(file);
                }
                catch
                {
                }
            }
        }
        string m_filepath = null;
        HtmlTextWriter writer;
        FileStream m_fs;
        /// <summary>
        /// 获取缓存的
        /// </summary>
        /// <returns></returns>
        public string GetCacheFilePath()
        {
            string url = getUrl();
            while (true)
            {
                PageCache cache = caches[url] as PageCache;
                if (cache == null)
                {
                    m_filepath = HttpContext.Current.Server.MapPath("/_temp_html") + "\\" + Guid.NewGuid();
                    m_fs = File.Create(m_filepath);
                    StreamWriter sw = new StreamWriter(m_fs);
                    writer = new HtmlTextWriter(sw);
                    
                    return null;
                }
                else
                {
                    if (cache.PhysicalPath != null && new FileInfo(cache.PhysicalPath).LastWriteTime > cache.ExpiredTime)
                    {
                        cache.ExpiredTime = DateTime.Now;
                        caches.Remove(url);
                        int key = System.Threading.Interlocked.Increment(ref toDeleteFilesIndex);
                        toDeleteFiles.Add(key, cache);
                        continue;
                    }
                    else if (cache.PhysicalPath == null && cache.ExpiredTime < DateTime.Now)
                    {
                        caches.Remove(url);
                        int key = System.Threading.Interlocked.Increment(ref toDeleteFilesIndex);
                        toDeleteFiles.Add(key, cache);
                        continue;
                    }
                }

                return cache.FilePath;
            }
        }
        public HtmlTextWriter ReadToSave()
        {
            return writer;
        }

        string getUrl()
        {
            string url = HttpContext.Current.Request.Url.ToString();
            if (!cacheByURL)
            {
                int flag = url.IndexOf("?");
                if (flag > 0)
                {
                    url = url.Substring(0, flag);
                }
            }
            return url;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="physicalPath">物理文件路径,如：Request.PhysicalPath</param>
        /// <returns></returns>
        public string Save(string physicalPath)
        {
            writer.Flush();
            writer.Dispose();
            m_fs.Dispose();
            try
            {
                if (m_seconds == 0)
                {
                    caches.Add(getUrl(), new PageCache
                    {
                        FilePath = m_filepath,
                        ExpiredTime = new FileInfo(physicalPath).LastWriteTime,
                        PhysicalPath = physicalPath,
                    });
                }
                else
                {
                    caches.Add(getUrl(), new PageCache
                    {
                        FilePath = m_filepath,
                        ExpiredTime = DateTime.Now.AddSeconds(m_seconds),
                    });
                }
                return m_filepath;
            }
            catch {
                return GetCacheFilePath();
            }
        }

    }
}
