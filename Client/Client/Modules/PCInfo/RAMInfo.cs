using Client.Modules.PCInfo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace Client.Modules.PCInfo
{
    public class RAMInfo
    {
        [SupportedOSPlatform("windows")]
        public RAMInfoModel GetRAMInfo()
        {
            var model = new RAMInfoModel();

            var searcherRAM = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory");

            foreach (ManagementObject obj in searcherRAM.Get())
            {
                model.Type = Convert.ToString(obj["Caption"]) ?? "Unknown";
                model.PartNumber = Convert.ToString(obj["PartNumber"]) ?? "Unknown";
                model.Frequency = Convert.ToString(obj["ConfiguredClockSpeed"]) ?? "Unknown";
                model.MemoryCount = Convert.ToString((Convert.ToInt64(obj["Capacity"])) / 1073741824.0) + "GB" ?? "Unknown";
            }

            return model;
        }
    }
}
