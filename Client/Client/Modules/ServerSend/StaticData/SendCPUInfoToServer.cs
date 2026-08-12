using Client.Modules.PCInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Client.Modules.ServerSend.StaticData
{
    public class SendCPUInfoToServer
    {
        public async Task SendStaticDataToServerAsync(HttpClient client, CancellationToken stoppingToken)
        {
            var CPUInfoModule = new CPUInfo();
            var data = CPUInfoModule.GetCPUInfo();

            await client.PostAsJsonAsync("http://localhost:5000/api/cpu", data, stoppingToken);
        }
    }
}
