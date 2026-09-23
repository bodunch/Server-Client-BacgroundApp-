using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Model
{
    public class DnmConnectInfo
    {
        public string ComputerName { get; set; } = string.Empty;
        public IList<ConnectionProperty>? Connection { get; set; }
    }

    public class ConnectionProperty
    {
        public string LocalConnection { get; set; } = string.Empty;
        public string RemoteConnection { get; set; } = string.Empty;
    }
}
