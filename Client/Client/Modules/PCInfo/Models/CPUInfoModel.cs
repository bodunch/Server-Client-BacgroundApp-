using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Modules.PCInfo.Models
{
    public class CPUInfoModel
    {
        public string CPUName { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public string NumOfCores { get; set; } = string.Empty;
        public string NumOfStreams { get; set; } = string.Empty;
    }
}
