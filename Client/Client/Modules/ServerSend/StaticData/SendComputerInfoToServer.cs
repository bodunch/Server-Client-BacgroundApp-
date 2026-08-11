using Client.Modules.PCInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Client.Modules.ServerSend.StaticData
{
    public class SendComputerInfoToServer
    {
        public async Task SendStaticDataToServerAsync(HttpClient client, CancellationToken stoppingToken)
        {
            var computerInfoModule = new ComputerInfo();
            var data = computerInfoModule.GetComputerInfo();

            await client.PostAsJsonAsync("http://localhost:5000/api/computer", data, stoppingToken);
        }
    }
}
