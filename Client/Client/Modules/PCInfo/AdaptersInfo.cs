using Client.Modules.PCInfo.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Client.Modules.PCInfo
{
    public class AdaptersInfo
    {
        public AdaptersInfoModel GetAdaptersInfo()
        {
            var model = new AdaptersInfoModel()
            {
                Adapter = new List<AdapterProperty>()
            };

            foreach (var net in NetworkInterface.GetAllNetworkInterfaces())
            {
                string name = net.Name ?? "Unknown";
                string? stat;
                if (net.OperationalStatus == OperationalStatus.Up)
                    stat = Convert.ToString(net.OperationalStatus);
                else 
                    stat = "No connection";
                string speed = (net.Speed / 1_000_000).ToString() + "MB/s" ?? "Unknown";
                string received = (Math.Round(net.GetIPStatistics().BytesReceived / 1024.0 / 1024.0, 2)).ToString() + "MB" ?? "Unknown";
                string sent = (Math.Round(net.GetIPStatistics().BytesSent / 1024.0 / 1024.0, 2)).ToString() + "MB" ?? "Unknown";

                var adaptersProperty = new AdapterProperty()
                {
                    Name = name,
                    Status = stat!,
                    Speed = speed,
                    Received = received,
                    Sent = sent
                };

                model.Adapter.Add(adaptersProperty);
            }

            return model;
        }
    }
}
