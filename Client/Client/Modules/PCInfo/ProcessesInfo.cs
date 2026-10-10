using Client.Modules.PCInfo.Models;
using Remotion.Linq.Clauses.ResultOperators;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

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

            model.ComputerName = Environment.MachineName;

            foreach (var proc in Process.GetProcesses())
            {
                string name = proc.ProcessName ?? "Unknown";
                string id = proc.Id.ToString() ?? "Unknown";
                string ram = Math.Round(proc.WorkingSet64 / 1048576.0, 2).ToString() ?? "Unknown";

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

                string sha256 = "Unknown";
                if (File.Exists(path))
                {
                    sha256 = GetSha256(path);
                }

                var processProperty = new ProcessProperty()
                {
                    Name = name,
                    Id = id,
                    RAM = ram,
                    SHA256 = sha256,
                    StartTime = startTime,
                    Path = path
                };

                model.Process.Add(processProperty);
            }

            return model;
        }

        public static string GetSha256(string path)
        {
            try
            {
                using var sha256 = SHA256.Create();
                using var stream = File.OpenRead(path);

                byte[] hash = sha256.ComputeHash(stream);

                return Convert.ToHexString(hash);
            }
            catch (UnauthorizedAccessException)
            {
                return "Access Denied";
            }
            catch (IOException)
            {
                return "File Unavailable";
            }
            catch (Exception)
            {
                return "Unknown";
            }
        }
    }
}
