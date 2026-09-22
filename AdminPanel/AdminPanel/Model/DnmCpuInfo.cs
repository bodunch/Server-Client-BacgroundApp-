using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Model
{
    public class DnmCpuInfo
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string ComputerName { get; set; } = string.Empty;
        public string LoadCPU { get; set; } = string.Empty;
        public string ErrorCode { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
