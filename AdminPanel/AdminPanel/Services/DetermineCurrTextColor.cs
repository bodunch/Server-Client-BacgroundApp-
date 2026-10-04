using AdminPanel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace AdminPanel.Services
{
    public static class DetermineCurrTextColor
    {
        public static string CpuDataColor(DnmCpuInfo data)
        {
            return data switch
            {
                { ErrorCode: not "No errors" } => "red",
                _ => "black"
            };
        }
    }
}
