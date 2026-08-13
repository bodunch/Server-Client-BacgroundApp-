using Client.Modules.PCInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Client.Modules.ServerSend.DynamicData
{
    public class SendCurrentRAMInfoToServer
    {
        public async Task SendStaticDataToServerAsync(HttpClient client, CancellationToken stoppingToken)
        {
            var CurrentRAMInfoModule = new CurrentRAMInfo();
            var data = CurrentRAMInfoModule.GetCurrentRAMInfo();

            await client.PostAsJsonAsync("http://localhost:5000/api/currram", data, stoppingToken);
        }
    }
}
