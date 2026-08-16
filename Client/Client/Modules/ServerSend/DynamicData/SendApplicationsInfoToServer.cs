using Client.Modules.PCInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Client.Modules.ServerSend.DynamicData
{
    public class SendApplicationsInfoToServer
    {
        public async Task SendDynamicDataToServerAsync(HttpClient client, CancellationToken stoppingToken)
        {
            var ApplicationInfoModeule = new ApplicationInfo();
            var data = ApplicationInfoModeule.GetApplicationInfo();

            await client.PostAsJsonAsync("http://localhost:5000/api/app", data, stoppingToken);
        }
    }
}
