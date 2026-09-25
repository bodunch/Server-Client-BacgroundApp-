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
    public class DnmAppInfoResp
    {
        private readonly StaticInfoTextBox _staticInfoTextBox;

        private string currentTimeStamp;

        public DnmAppInfoResp(StaticInfoTextBox staticInfoTextBox)
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
                    string url = "http://localhost:5000/api/clients/dynamicinfo/appinfo";

                    using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

                    request.Headers.Add("ClientId", Id);

                    HttpResponseMessage response = await client.SendAsync(request, token);

                    if (response.IsSuccessStatusCode)
                    {
                        var dnmAppInfo = await response.Content.ReadFromJsonAsync<DynamicDataFromServer>();

                        if (dnmAppInfo != null)
                        {
                            var appData = JsonSerializer.Deserialize<DnmAppInfo>(dnmAppInfo.JsonPayload, jsonOptions);

                            if (appData != null && dnmAppInfo.TimeStamp != currentTimeStamp)
                            {
                                var sb = new StringBuilder();
                                sb.AppendLine($"Computer: {appData.ComputerName} | Time: {dnmAppInfo.TimeStamp}");
                                sb.AppendLine(new string('-', 50));

                                if (appData.Application != null)
                                {
                                    foreach (var app in appData.Application)
                                    {
                                        sb.AppendLine($"App name: {app.AppName,-20} | Window title: {app.WindowTitle} | Id: {app.Id} | RAM: {app.RAM}");
                                    }
                                }

                                _staticInfoTextBox.DnmInfoText = sb.ToString() + Environment.NewLine;
                                currentTimeStamp = dnmAppInfo.TimeStamp;
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
