using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AdminPanel.ViewModel
{
    public class GetClientsFromServer
    {
        public async Task GetClients(MainWindow mainWindow)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync("http://localhost:5000/api/system");

            string result = await response.Content.ReadAsStringAsync();

            mainWindow.textBox.Text = result;
        }
    }
}
