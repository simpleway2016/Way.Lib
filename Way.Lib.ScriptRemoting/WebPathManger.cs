using System;
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
        static WebPathManger()
        {
            watcher = new System.IO.FileSystemWatcher(ScriptRemotingServer.Root);
            watcher.IncludeSubdirectories = true;
            watcher.Changed += Watcher_event;
            watcher.Created += Watcher_event;
            watcher.Renamed += Watcher_event;
            watcher.Deleted += Watcher_event;
            watcher.EnableRaisingEvents = true;

            root = ScriptRemotingServer.Root;
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
            var files = System.IO.Directory.GetFiles(folder);
            foreach (var file in files)
            {
                Paths.Add(file.Substring(root.Length - 1).Replace("\\", "/"));
            }

            var dirs = System.IO.Directory.GetDirectories(folder);
            foreach( var dir in dirs )
            {
                findPath(dir);
            }
            
        }
        internal static string getFileUrl(string url)
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
