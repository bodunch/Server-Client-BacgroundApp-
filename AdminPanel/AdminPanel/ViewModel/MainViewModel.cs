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
    public class MainViewModel
    {
        public ObservableCollection<Clients> ClientsItem { get; set; } = new();

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
    }
}
