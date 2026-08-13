using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Modules.PCInfo.Models
{
    public class CurrentRAMInfoModel
    {
        public string TotalMem { get; set; } = string.Empty;
        public string FreeMem { get; set;} = string.Empty;
    }
}
