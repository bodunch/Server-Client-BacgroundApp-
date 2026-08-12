using Client.Modules.PCInfo.Models;
using System;
using System.Management;
using System.Runtime.Versioning;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Modules.PCInfo
{
    public class CPUInfo
    {
        [SupportedOSPlatform("windows")]
        public CPUInfoModel GetCPUInfo()
        {
            var model = new CPUInfoModel();

            var searcherCPU = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");

            foreach(ManagementObject obj in searcherCPU.Get())
            {
                model.CPUName = Convert.ToString(obj["Name"]) ?? "Unknown";
                model.Manufacturer = Convert.ToString(obj["Manufacturer"]) ?? "Unknown";
                model.NumOfCores = Convert.ToString(obj["NumberOfCores"]) ?? "Unknown";
                model.NumOfStreams = Convert.ToString(obj["NumberOfLogicalProcessors"]) ?? "Unknown";
            }

            return model;
        }
    }
}
