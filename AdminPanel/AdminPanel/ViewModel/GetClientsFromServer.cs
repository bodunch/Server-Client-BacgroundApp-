using AdminPanel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AdminPanel.ViewModel
{
    public class GetClientsFromServer
    {
        public async Task GetClients(MainWindow mainWindow)
        {
            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync("http://localhost:5000/api/clients");

                if (!response.IsSuccessStatusCode)
                {
                    mainWindow.textBox.Text = "Error with server";
                    return;
                }

                List<Clients>? clientsList = await response.Content.ReadFromJsonAsync<List<Clients>>();

                if (clientsList == null || clientsList.Count == 0)
                {
                    mainWindow.textBox.Text = "0 clients";
                    return;
                }

                string showText = "";
                foreach (var c in clientsList)
                {
                    showText += $"Client Id : {c.Id} | Machine Name : {c.MachineName} | First connected : {c.FirstConnected} | Last seen : {c.LastSeen}" + Environment.NewLine;
                }
                mainWindow.textBox.Text = showText;
            }
            catch (Exception ex)
            {
                mainWindow.textBox.Text = ex.Message;
            }
        }
    }
}
