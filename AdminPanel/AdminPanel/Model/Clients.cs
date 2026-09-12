using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Model
{
    public class Clients
    {
        public int Id { get; set; }
        public string MachineName { get; set; } = string.Empty;
        public string FirstConnected { get; set; } = string.Empty;
        public string LastSeen { get; set; } = string.Empty;
    }
}
