using AdminPanel.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.ViewModel
{
    public class MainViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        public ObservableCollection<Clients> ClientsItem { get; set; } = new();

        private string _systemInfoText = "Виберіть ПК";
        public string SystemInfoText
        {
            get => _systemInfoText;
            set
            {
                _systemInfoText = value;
                OnPropertyChanged(nameof(SystemInfoText));
            }
        }

        public MainViewModel()
        {
            _ = StartPollingAsync();
        }
        private async Task StartPollingAsync()
        {
            using HttpClient client = new HttpClient();

            while (true)
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync("http://localhost:5000/api/clients");

                    if (response.IsSuccessStatusCode)
                    {
                        var clientsList = await response.Content.ReadFromJsonAsync<List<Clients>>();

                        if (clientsList != null)
                        {
                            ClientsItem.Clear();

                            foreach (var c in clientsList)
                            {
                                if (!ClientsItem.Any(existing => existing.Id == c.Id))
                                {
                                    ClientsItem.Add(new Clients { Id = c.Id, MachineName = c.MachineName });
                                }
                            }
                        }
                    }
                }
                catch
                {

                }

                await Task.Delay(5000);
            }
        }

        public async Task ClientInfo(string Id)
        {
            using HttpClient client = new HttpClient();

            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/api/clients/staticinfo/system");
                request.Headers.Add("ClientId", Id);

                HttpResponseMessage response = await client.SendAsync(request);

                var systemInfo = await response.Content.ReadFromJsonAsync<SystemInfo>();

                if (systemInfo != null)
                {
                    SystemInfoText = $"ОС: {systemInfo.OperatingSystem}\n" +
                                 $"Версія: {systemInfo.Version}\n" +
                                 $"Ім'я ПК: {systemInfo.ComputerName}\n" +
                                 $"Користувач: {systemInfo.RegisteredUser}\n" +
                                 $"Остання завантаження: {systemInfo.LastBootTime}";
                }
                else
                {
                    SystemInfoText = "Дані не знайдені.";
                }
            }
            catch
            {

            }
        }
        public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
    }
}
