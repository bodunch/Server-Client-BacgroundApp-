using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Modules.PCInfo.Models
{
    public class CurrentCPUInfoModel
    {
        public string LoadCPU { get; set; } = string.Empty;
        public string ErrorCode { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
