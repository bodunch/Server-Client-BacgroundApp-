using Client.Modules.PCInfo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Client.Modules.PCInfo
{
    public class PortInfo
    {
        public PortInfoModel GetPortInfo()
        {
            var model = new PortInfoModel()
            {
                Port = new List<PortProperty>()
            };

            model.ComputerName = Environment.MachineName;

            foreach (var net in IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners())
            {
                string adress = net.Address.ToString() ?? "Unknown";
                string port = net.Port.ToString() ?? "Unknown";

                var portProperty = new PortProperty()
                {
                    Adress = adress,
                    Port = port
                };

                model.Port.Add(portProperty);
            } 

            return model;
        }
    }
}
