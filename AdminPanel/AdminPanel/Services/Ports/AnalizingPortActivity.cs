using AdminPanel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.Ports
{
    public class AnalizingPortActivity
    {
        private static readonly int[] DangerousPorts = { 3389, 445, 23, 5900, 5901, 5985, 5986, 22, 137, 138, 139, 21, 69, 6379, 4444, 31337, 12345, 12346, 6666, 6667 };

        public static string EvaluatePorts(string Adress, string Port)
        {
            if (Port == null || !Port.Any())
            {
                return "black";
            }

            if (int.TryParse(Port, out int portNumber))
            {
                if (DangerousPorts.Contains(portNumber))
                {
                    string addr = Adress?.Trim() ?? string.Empty;

                    bool isLocalhost = addr.StartsWith("127.0.0.1") || addr.StartsWith("::1");

                    if (!isLocalhost)
                    {
                        return "red";
                    }
                }
            }

            if (!string.IsNullOrEmpty(Adress) && Adress.Contains("0.0.0.0"))
            {
                return "yellow";
            }

            return "black";
        }
    }
}
