using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.Adapters
{
    public class AnalizingAdaptersActivity
    {
        private static readonly string[] ForbiddenAdapters = {"vpn", "tap", "tun", "radmin vpn", "hamachi", "openvpn", "wireguard", "tailscale", "zerotier", "netbird", "twingate", "nordvpn", "expressvpn", "protonvpn", "cyberghost", "surfshark", "psiphon", "lantern", "shadowsocks" };

        public static string EvaluateAdapters(string Name, string Status, string Speed, string Received, string Sent)
        {
            string trimName = Name.ToLower().Trim() ?? string.Empty;
            string status = Status?.Trim() ?? string.Empty;

            foreach (var forbidden in ForbiddenAdapters)
            {
                if (trimName.Contains(forbidden) && status.Equals("Up", StringComparison.OrdinalIgnoreCase))
                {
                    return "yellow";
                }
            }

            return "black";
        }
    }
}
