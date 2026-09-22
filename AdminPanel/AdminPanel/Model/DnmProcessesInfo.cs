using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Model
{
    public class DnmProcessesInfo
    {
        public string ComputerName { get; set; } = string.Empty;
        public List<ProcessProperty>? Process { get; set; }
    }

    public class ProcessProperty
    {
        public string Name { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public string RAM { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
    }
}
