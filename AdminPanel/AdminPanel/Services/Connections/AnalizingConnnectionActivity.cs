using AdminPanel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.Connections
{
    public class AnalizingConnnectionActivity
    {
        private static readonly int[] AllowedRemotePorts = { 80, 443, 53, 8080, 8443 };

        public static string EvaluateConnections(string Local, string Remote)
        {
            if (Local == null || Remote == null)
            {
                return "black";
            }

            int suspiciousCount = 0;

            int remotePort = ExtractPort(Remote);
            string remoteIp = ExtractIp(Remote);

            bool isExternal = !remoteIp.StartsWith("127.") &&
                              !remoteIp.StartsWith("192.168.") &&
                              !remoteIp.StartsWith("10.") &&
                              !remoteIp.StartsWith("::");

            if (isExternal)
            {
                if (!AllowedRemotePorts.Contains(remotePort))
                {
                    suspiciousCount++;
                }
            }

            if (suspiciousCount > 5)
            {
                return "red";
            }
            else if (suspiciousCount > 0)
            {
                return "yellow";
            }

            return "black";
        }

        private static int ExtractPort(string remoteConnStr)
        {
            var parts = remoteConnStr.Split(':');
            if (parts.Length > 1 && int.TryParse(parts.Last(), out int port))
            {
                return port;
            }
            return 0;
        }

        private static string ExtractIp(string remoteConnStr)
        {
            var clean = remoteConnStr.Replace("Remote:", "").Trim();
            var lastColonIndex = clean.LastIndexOf(':');
            if (lastColonIndex > 0)
            {
                return clean.Substring(0, lastColonIndex);
            }
            return clean;
        }
    }
}
