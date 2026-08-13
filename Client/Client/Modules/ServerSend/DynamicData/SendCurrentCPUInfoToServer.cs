using Client.Modules.PCInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Client.Modules.ServerSend.DynamicData
{
    public class SendCurrentCPUInfoToServer
    {
        public async Task SendStaticDataToServerAsync(HttpClient client, CancellationToken stoppingToken)
        {
            var CurrentCPUInfoModule = new CurrentCPUInfo();
            var data = CurrentCPUInfoModule.GetCurrentCPUInfo();

            await client.PostAsJsonAsync("http://localhost:5000/api/currcpu", data, stoppingToken);
        }
    }
}
