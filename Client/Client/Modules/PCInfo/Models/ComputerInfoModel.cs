using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Modules.PCInfo.Models
{
    public class ComputerInfoModel
    {
        public string Manufacturer { get; set; } = string.Empty;
        public string PCModel { get; set; } = string.Empty;
        public string SystemType { get; set; } = string.Empty;
        public string CountOfCPU { get; set; } = string.Empty;
        public string SystemStart { get; set; } = string.Empty;
        public string StatusOfStart { get; set; } = string.Empty;
    }
}
