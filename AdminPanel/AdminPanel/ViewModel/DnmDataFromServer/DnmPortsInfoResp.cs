using AdminPanel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdminPanel.ViewModel.DnmDataFromServer
{
    public class DnmPortsInfoResp
    {
        private readonly StaticInfoTextBox _staticInfoTextBox;

        private string currentTimeStamp;

        public DnmPortsInfoResp(StaticInfoTextBox staticInfoTextBox)
        {
            _staticInfoTextBox = staticInfoTextBox;
        }

        public async Task TakeData(string Id, CancellationToken token)
        {
            using HttpClient client = new HttpClient();

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            while (!token.IsCancellationRequested)
            {
                try
                {
                    string url = "http://localhost:5000/api/clients/dynamicinfo/portsinfo";

                    using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

                    request.Headers.Add("ClientId", Id);

                    HttpResponseMessage response = await client.SendAsync(request, token);

                    if (response.IsSuccessStatusCode)
                    {
                        var dnmPortsInfo = await response.Content.ReadFromJsonAsync<DynamicDataFromServer>();

                        if (dnmPortsInfo != null)
                        {
                            var portsData = JsonSerializer.Deserialize<DnmPortsInfo>(dnmPortsInfo.JsonPayload, jsonOptions);

                            if (portsData != null && dnmPortsInfo.TimeStamp != currentTimeStamp)
                            {
                                var sb = new StringBuilder();
                                sb.AppendLine($"Computer: {portsData.ComputerName} | Time: {dnmPortsInfo.TimeStamp}");
                                sb.AppendLine(new string('-', 50));

                                if (portsData.Port != null)
                                {
                                    foreach (var port in portsData.Port)
                                    {
                                        sb.AppendLine($"Adress: {port.Adress,-20} | Port: {port.Port}");
                                    }
                                }

                                _staticInfoTextBox.DnmInfoText = sb.ToString() + Environment.NewLine;
                                currentTimeStamp = dnmPortsInfo.TimeStamp;
                            }
                        }
                        else
                        {
                            _staticInfoTextBox.DnmInfoText = "Data not found";
                        }
                    }
                    else
                    {
                        _staticInfoTextBox.DnmInfoText = $"HTTP Error: {(int)response.StatusCode}";
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _staticInfoTextBox.DnmInfoText = $"ERROR: {ex.Message}";
                }

                try
                {
                    await Task.Delay(5000, token);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }
    }
}
