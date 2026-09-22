using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Model
{
    public class DynamicDataFromServer
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string JsonPayload { get; set; } = string.Empty;
        public string TimeStamp { get; set; } = string.Empty;
    }
}
