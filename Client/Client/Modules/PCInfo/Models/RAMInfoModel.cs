using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Modules.PCInfo.Models
{
    public class RAMInfoModel
    {
        public string Type { get; set; } = string.Empty;
        public string PartNumber { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
        public string MemoryCount { get; set; } = string.Empty;
    }
}
