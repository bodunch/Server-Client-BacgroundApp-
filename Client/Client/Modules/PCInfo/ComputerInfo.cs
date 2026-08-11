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
    public class ComputerInfo
    {
        [SupportedOSPlatform("windows")]
        public ComputerInfoModel GetComputerInfo()
        {
            var model = new ComputerInfoModel();

            var searcherSystem = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");

            foreach(ManagementObject obj in searcherSystem.Get())
            {
                model.Manufacturer = Convert.ToString(obj["Manufacturer"]) ?? "Unknow";
                model.PCModel = Convert.ToString(obj["Model"]) ?? "Unknow";
                model.SystemType = Convert.ToString(obj["SystemType"]) ?? "Unknow";
                model.CountOfCPU = Convert.ToString(obj["NumberOfProcessors"]) ?? "Unknow";
                model.SystemStart = Convert.ToString(obj["BootupState"]) ?? "Unknow";
                model.StatusOfStart = Convert.ToString(obj["Status"]) ?? "Unknow";
            }
            return model;
        }
    }
}
