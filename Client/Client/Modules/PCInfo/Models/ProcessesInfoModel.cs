using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Modules.PCInfo.Models
{
    public class ProcessesInfoModel
    {
        public IList<ProcessProperty>? Process { get; set; }
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
