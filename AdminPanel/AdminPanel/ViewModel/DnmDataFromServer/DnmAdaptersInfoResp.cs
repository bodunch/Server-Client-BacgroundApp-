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
    public class DnmAdaptersInfoResp
    {
        private readonly StaticInfoTextBox _staticInfoTextBox;

        private string currentTimeStamp;

        public DnmAdaptersInfoResp(StaticInfoTextBox staticInfoTextBox)
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
                    string url = "http://localhost:5000/api/clients/dynamicinfo/adaptersinfo";

                    using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

                    request.Headers.Add("ClientId", Id);

                    HttpResponseMessage response = await client.SendAsync(request, token);

                    if (response.IsSuccessStatusCode)
                    {
                        var dnmAdaptersInfo = await response.Content.ReadFromJsonAsync<DynamicDataFromServer>();

                        if (dnmAdaptersInfo != null)
                        {
                            var adapterData = JsonSerializer.Deserialize<DnmAdaptersInfo>(dnmAdaptersInfo.JsonPayload, jsonOptions);

                            if (adapterData != null && dnmAdaptersInfo.TimeStamp != currentTimeStamp)
                            {
                                var sb = new StringBuilder();
                                sb.AppendLine($"Computer: {adapterData.ComputerName} | Time: {dnmAdaptersInfo.TimeStamp}");
                                sb.AppendLine(new string('-', 50));

                                if (adapterData.Adapter != null)
                                {
                                    foreach (var adapter in adapterData.Adapter)
                                    {
                                        sb.AppendLine($"Name: {adapter.Name,-20} | Status: {adapter.Status} | Speed: {adapter.Speed} | Received: {adapter.Received} | Sent: {adapter.Sent}");
                                    }
                                }

                                _staticInfoTextBox.DnmInfoText = sb.ToString() + Environment.NewLine;
                                currentTimeStamp = dnmAdaptersInfo.TimeStamp;
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
