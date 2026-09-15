using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Model
{
    public class SystemInfo
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string OperatingSystem { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string ComputerName { get; set; } = string.Empty;
        public string RegisteredUser { get; set; } = string.Empty;
        public string LastBootTime { get; set; } = string.Empty;
    }
}
