using AdminPanel.Model;
using AdminPanel.Model.ChartLogs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AdminPanel.Services.Charts
{
    public class CpuChart
    {
        private readonly TextBox _textBox;

        public CpuChart(TextBox textBox)
        {
            _textBox = textBox;
        }

        public async Task TakeDataForChart(string clientId)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                _textBox.Text = "Client not found";
                return;
            }

            try
            {
                using var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri("http://localhost:5000/");
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Add("ClientId", clientId);

                string endpoint = "api/charts/cpu";

                HttpResponseMessage response = await httpClient.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var points = JsonSerializer.Deserialize<List<CpuLogPoint>>(jsonResponse, options);

                    if (points != null && points.Any())
                    {
                        var sb = new StringBuilder();

                        foreach (var pt in points)
                        {
                            sb.AppendLine($"[Час: {pt.Time}] — Навантаження: {pt.Value}%");
                        }

                        _textBox.Text = sb.ToString();
                    }
                    else
                    {
                        _textBox.Text = "У базі даних поки немає збережених логів для цього запиту.";
                    }
                }
                else
                {
                    _textBox.Text = $"Помилка сервера: {(int)response.StatusCode} {response.ReasonPhrase}";
                }
            }
            catch { }
        }
    }
}
