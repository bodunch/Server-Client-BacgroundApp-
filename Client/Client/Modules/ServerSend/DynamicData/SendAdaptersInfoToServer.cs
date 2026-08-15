using Client.Modules.PCInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Client.Modules.ServerSend.DynamicData
{
    public class SendAdaptersInfoToServer
    {
        public async Task SendDynamicDataToServerAsync(HttpClient client, CancellationToken stoppingToken)
        {
            var AdaptersInfoModeule = new AdaptersInfo();
            var data = AdaptersInfoModeule.GetAdaptersInfo();

            await client.PostAsJsonAsync("http://localhost:5000/api/adapters", data, stoppingToken);
        }
    }
}
