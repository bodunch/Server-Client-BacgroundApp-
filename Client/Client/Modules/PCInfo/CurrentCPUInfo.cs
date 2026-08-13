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
    public class CurrentCPUInfo
    {
        [SupportedOSPlatform("windows")]
        public CurrentCPUInfoModel GetCurrentCPUInfo()
        {
            var model = new CurrentCPUInfoModel();

            var searcherCPU = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");

            foreach (ManagementObject obj in searcherCPU.Get())
            {
                model.LoadCPU = Convert.ToString(obj["LoadPercentage"]) + "%" ?? "Unknown";
                string errorCode = Convert.ToString(obj["LastErrorCode"]);
                model.ErrorCode = (errorCode == "" || errorCode == null) ? "No errors" : errorCode;
                model.Status = Convert.ToString(obj["Status"]) ?? "Unknown";
            }

            return model;
        }
    }
}
