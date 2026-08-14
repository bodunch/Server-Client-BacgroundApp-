using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Remotion.Linq.Clauses.ResultOperators;
using Client.Modules.PCInfo.Models;

namespace Client.Modules.PCInfo
{
    public class ProcessesInfo
    {
        public ProcessesInfoModel GetProcessesInfo()
        {
            var model = new ProcessesInfoModel()
            {
                Process = new List<ProcessProperty>()
            };

            foreach(var proc in Process.GetProcesses())
            {
                string name = proc.ProcessName;
                string id = proc.Id.ToString();
                string ram = Math.Round(proc.WorkingSet64 / 1048576.0, 2).ToString();
                string startTime = "Unknown Time";
                try
                {
                    startTime = proc.StartTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
                catch
                {
                    startTime = "Access Denied";
                }
                string path = "Unknown Path";
                try
                {
                    path = proc.MainModule?.FileName ?? "Unknown Path";
                }
                catch
                {
                    path = "Access Denied";
                }

                var processProperty = new ProcessProperty()
                {
                    Name = name,
                    Id = id,
                    RAM = ram,
                    StartTime = startTime,
                    Path = path
                };

                model.Process.Add(processProperty);
            }

            return model;
        }
    }
}
