using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AdminPanel.ViewModel.SendCommand
{
    public class SendCommandToServer
    {
        public async Task SendCommandPowerOff(string clientId, HttpClient httpClient)
        {
            try
            {
                var payload = new { CommandText = "pcoff" };

                HttpResponseMessage response = await httpClient.PostAsJsonAsync($"http://localhost:5000/api/commands/send?machineName={clientId}", payload);
            }
            catch { }
        }

        public async Task SendCommandReboot(string clientId, HttpClient httpClient)
        {
            try
            {
                var payload = new { CommandText = "pcreboot" };

                HttpResponseMessage response = await httpClient.PostAsJsonAsync($"http://localhost:5000/api/commands/send?machineName={clientId}", payload);
            }
            catch { }
        }

        public async Task SendCommandNetworOff(string clientId, HttpClient httpClient)
        {
            try
            {
                var payload = new { CommandText = "network" };

                HttpResponseMessage response = await httpClient.PostAsJsonAsync($"http://localhost:5000/api/commands/send?machineName={clientId}", payload);
            }
            catch { }
        }

        public async Task SendAdminCustomCommand(string clientId, HttpClient httpClient, string command)
        {
            if (string.IsNullOrEmpty(command)) return;

            try
            {
                var payload = new { CommandText = command };
                HttpResponseMessage response = await httpClient.PostAsJsonAsync($"http://localhost:5000/api/commands/send?machineName={clientId}", payload);
            }
            catch { }
        }
    }
}
