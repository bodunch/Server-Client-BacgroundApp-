using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Modules.PCInfo.Models
{
    public class ApplicationInfoModel
    {
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
