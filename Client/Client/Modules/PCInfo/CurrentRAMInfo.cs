using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Modules.PCInfo.Models;
using System.Management;
using System.Runtime.Versioning;

namespace Client.Modules.PCInfo
{
    public class CurrentRAMInfo
    {
        public CurrentRAMInfoModel GetCurrentRAMInfo()
        {
            var model = new CurrentRAMInfoModel();

            var searcherRAM = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");

            foreach(ManagementObject obj in searcherRAM.Get())
            {
                model.TotalMem = Convert.ToString(Math.Round((Convert.ToDouble(obj["TotalVisibleMemorySize"])) / 1048576.0, 2)) ?? "Unknown";
                model.FreeMem = Convert.ToString(Math.Round((Convert.ToDouble(obj["FreePhysicalMemory"])) / 1048576.0, 2)) ?? "Unknown";
            }

            return model;
        }
    }
}
