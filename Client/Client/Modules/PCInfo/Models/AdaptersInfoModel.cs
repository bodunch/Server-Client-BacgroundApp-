using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Modules.PCInfo.Models
{
    public class AdaptersInfoModel
    {
        public IList<AdapterProperty>? Adapter { get; set; }
    }

    public class AdapterProperty
    {
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Speed { get; set; } = string.Empty;
        public string Received { get; set; } = string.Empty;
        public string Sent { get; set; } = string.Empty;
    }
}
