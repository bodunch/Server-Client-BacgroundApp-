using Client.Modules.PCInfo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Client.Modules.PCInfo
{
    public class ConnectionsInfo
    {
        public ConnectionsInfoModel GetConnectionsInfo()
        {
            var model = new ConnectionsInfoModel()
            {
                Connection = new List<ConnectionProperty>()
            };

            foreach (var net in IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpConnections())
            {
                if(net.State == TcpState.Established)
                {
                    string localConnection = net.LocalEndPoint.ToString();
                    string remoteConnection = net.RemoteEndPoint.ToString();

                    var connectionPropery = new ConnectionProperty()
                    {
                        LocalConnection = localConnection,
                        RemoteConnection = remoteConnection
                    };
                    
                    model.Connection.Add(connectionPropery);
                }
            }

            return model;
        }
    }
}
