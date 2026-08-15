using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Modules.PCInfo.Models
{
    public class PortInfoModel
    {
        public IList<PortProperty>? Port { get; set; }
    }

    public class PortProperty
    {
        public string Adress{ get; set; } = string.Empty;
        public string Port { get; set; } = string.Empty;
    }
}
