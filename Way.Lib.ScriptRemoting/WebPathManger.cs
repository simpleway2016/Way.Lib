﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Way.Lib.ScriptRemoting
{
    /// <summary>
    /// linux文件名区分大小写，所以只能记录当前所有文件路径
    /// </summary>
    internal class WebPathManger
    {
        static System.IO.FileSystemWatcher watcher;
        static List<String> Paths = new List<string>();
        static string root;
        HttpServer _server;
        internal WebPathManger(HttpServer server)
        {
            _server = server;
               watcher = new System.IO.FileSystemWatcher(_server.Root);
            watcher.IncludeSubdirectories = true;
            watcher.Changed += Watcher_event;
            watcher.Created += Watcher_event;
            watcher.Renamed += Watcher_event;
            watcher.Deleted += Watcher_event;
            watcher.EnableRaisingEvents = true;

            root = _server.Root;
            findPath(root);
        }
        private static void Watcher_event(object sender, System.IO.FileSystemEventArgs e)
        {
            string url = e.FullPath.Substring(root.Length - 1).Replace("\\", "/");
            if (e.ChangeType == System.IO.WatcherChangeTypes.Deleted)
            {
                try
                {
                    for (int i = 0; i < Paths.Count; i++)
                    {
                        if (string.Equals(Paths[i], url, StringComparison.CurrentCultureIgnoreCase))
                        {
                            Paths.RemoveAt(i);
                            return;
                        }
                    }
                }
                catch
                {
                }
            }
            else
            {
                try
                {
                    for (int i = 0; i < Paths.Count; i++)
                    {
                        if (string.Equals(Paths[i], url, StringComparison.CurrentCultureIgnoreCase))
                        {
                            Paths[i] = url;
                            return;
                        }
                    }
                }
                catch
                {
                }
                Paths.Add(url);
            }
        }
        static void findPath(string folder)
        {
            try
            {
                var files = System.IO.Directory.GetFiles(folder);
                foreach (var file in files)
                {
                    Paths.Add(file.Substring(root.Length - 1).Replace("\\", "/"));
                }
            }
            catch(Exception ex)
            {
                using (CLog log = new CLog("WebPathManger GetFiles error"))
                {
                    log.Log("folder:{0}" , folder);
                    if(ex is System.ArgumentException)
                    {
                        log.Log("请查看是否有中文名字的文件");
                    }
                    log.Log(ex.ToString());
                }
            }

            try
            {
                var dirs = System.IO.Directory.GetDirectories(folder);
                foreach (var dir in dirs)
                {
                    findPath(dir);
                }
            }
            catch (Exception ex)
            {
                using (CLog log = new CLog("WebPathManger GetDirectories error"))
                {
                    log.Log("folder:{0}", folder);
                    if (ex is System.ArgumentException)
                    {
                        log.Log("请查看是否有中文名字的文件夹");
                    }
                    log.Log(ex.ToString());
                }
            }
        }
        internal string GetFileUrl(string url)
        {
            string result = null;
            try
            {
                for (int i = 0; i < Paths.Count; i++)
                {
                    if (string.Equals(Paths[i], url, StringComparison.CurrentCultureIgnoreCase))
                    {
                        result = Paths[i];
                        break;
                    }
                }
            }
            catch
            {
            }

            if (result == null)
                return url;
            return result;
        }
    }
}
