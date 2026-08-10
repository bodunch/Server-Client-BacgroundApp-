using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Modules.PCInfo.Models
{
    public class SystemInfoModel
    {
        public string OperatingSystem { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string ComputerName { get; set; } = string.Empty;
        public string RegisteredUser { get; set; } = string.Empty;
        public string LastBootTime { get; set; } = string.Empty;
    }
}
