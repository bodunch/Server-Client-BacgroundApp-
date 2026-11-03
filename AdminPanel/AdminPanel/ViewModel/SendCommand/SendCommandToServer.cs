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
        public async Task SendCommandPowerOff(string machineName, HttpClient httpClient)
        {
            await SendRequestAsync(machineName, "pcoff", httpClient);
        }

        public async Task SendCommandReboot(string machineName, HttpClient httpClient)
        {
            await SendRequestAsync(machineName, "pcreboot", httpClient);
        }

        public async Task SendCommandNetworOff(string machineName, HttpClient httpClient)
        {
            await SendRequestAsync(machineName, "network", httpClient);
        }

        public async Task SendAdminCustomCommand(string machineName, HttpClient httpClient, string command)
        {
            if (string.IsNullOrEmpty(command)) return;
            await SendRequestAsync(machineName, command, httpClient);
        }

        private async Task SendRequestAsync(string machineName, string commandText, HttpClient httpClient)
        {
            if (string.IsNullOrEmpty(machineName))
            {
                machineName = Environment.MachineName;
            }

            try
            {
                var payload = new { CommandText = commandText };
                HttpResponseMessage response = await httpClient.PostAsJsonAsync($"http://localhost:5000/api/commands/send?machineName={machineName}", payload);

                if (!response.IsSuccessStatusCode)
                {
                    string errorDetails = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Сервер відхилив команду (Статус {response.StatusCode}):\n{errorDetails}", "Помилка сервера", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[Admin] Команда '{commandText}' успішно надіслана для ПК: {machineName}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка підключення до сервера: {ex.Message}", "Помилка мережі", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
