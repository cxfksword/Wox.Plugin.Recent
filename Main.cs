using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Wox.Plugin.Recent
{
    public class Main : IPlugin
    {
        public List<Result> Query(Query query)
        {
            var result = new List<Result>();

            var recentPath = Environment.GetFolderPath(Environment.SpecialFolder.Recent);
            var dir = new DirectoryInfo(recentPath);
            var files = dir.GetFiles().OrderByDescending(x => x.LastWriteTime);
            WshShell shell = new WshShell();
            foreach(var file in files)
            {
                if ((file.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                    continue;

                IWshShortcut link = (IWshShortcut)shell.CreateShortcut(file.FullName);
                result.Add(new Result()
                {
                    Title = file.Name.Replace(".lnk", ""),
                    SubTitle = link == null ? file.FullName : link.TargetPath,
                    IcoPath = file.FullName,
                    Action = (c) =>
                    {
                        Process.Start(file.FullName);
                        return true;
                    }
                });
            }

            return result;
        }

        public void Init(PluginInitContext context)
        {
        }
    }
}
