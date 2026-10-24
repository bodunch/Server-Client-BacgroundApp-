using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Model.ChartLogs
{
    public class AdapterLogPoint
    {
        public string Name { get; set; } = string.Empty;
        public double Received { get; set; }
        public double Sent { get; set; }
    }
}
