using Client.Modules.PCInfo;
using Client.Modules.PCInfo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Client.Modules.ServerSend.StaticData
{
    public class SendSystemInfoToServer
    {
        public async Task SendStaticDataToServerAsync(IHttpClientFactory httpClientFactory, CancellationToken stoppingToken)
        {
            var systemInfoModule = new SystemInfo();
            var data = systemInfoModule.GetSystemInfo();

            using var client = httpClientFactory.CreateClient();

            await client.PostAsJsonAsync("http://localhost:5000/api/system", data, stoppingToken);
        }
    }
}
