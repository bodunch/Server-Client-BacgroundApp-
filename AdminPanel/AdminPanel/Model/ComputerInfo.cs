using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Model
{
    public  class ComputerInfo
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Manufacturer { get; set; } = string.Empty;
        public string PCModel { get; set; } = string.Empty;
        public string SystemType { get; set; } = string.Empty;
        public string CountOfCpu { get; set; } = string.Empty;
        public string SystemStart { get; set; } = string.Empty;
        public string StatusOfStart { get; set; } = string.Empty;
    }
}
