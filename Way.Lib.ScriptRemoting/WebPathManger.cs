using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Way.Lib.ScriptRemoting
{
    internal class WebPathManger
    {
        static List<String> Paths = new List<string>();
        static string root;
        static WebPathManger()
        {
            root = ScriptRemotingServer.Root;
            findPath(root);
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
            return (from m in Paths
                    where string.Equals(m, url, StringComparison.CurrentCultureIgnoreCase)
                    select m).FirstOrDefault();
        }
    }
}
