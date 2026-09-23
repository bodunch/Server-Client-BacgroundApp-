using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Model
{
    public class DnmAppInfo
    {
        public string ComputerName { get; set; } = string.Empty;
        public IList<ApplicationProperty>? Application { get; set; }
    }

    public class ApplicationProperty
    {
        public string AppName { get; set; } = string.Empty;
        public string WindowTitle { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public string RAM { get; set; } = string.Empty;
    }
}
