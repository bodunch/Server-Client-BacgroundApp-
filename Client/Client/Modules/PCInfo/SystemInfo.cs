using Client.Modules.PCInfo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.Versioning;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Client.Modules.PCInfo
{
    public class SystemInfo
    {
        [SupportedOSPlatform("windows")]
        public SystemInfoModel GetSystemInfo()
        {
            var model = new SystemInfoModel();

            var searcherSystem = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");

            foreach (ManagementObject obj in searcherSystem.Get())
            {
                model.OperatingSystem = Convert.ToString(obj["Caption"]) ?? "Unknown";
                model.Version = Convert.ToString(obj["Version"]) ?? "Unknown";
                model.ComputerName = Convert.ToString(obj["CSName"]) ?? "Unknown";
                model.RegisteredUser = Convert.ToString(obj["RegisteredUser"]) ?? "Unknown";

                string? value = obj["LastBootUpTime"].ToString();
                if(!string.IsNullOrEmpty(value))
                {
                    DateTime bootTime = ManagementDateTimeConverter.ToDateTime(value);
                    model.LastBootTime = bootTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            return model;
        }
    }
}
