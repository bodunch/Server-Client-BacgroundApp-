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
    public class DnmConnectInfoResp
    {
        private readonly StaticInfoTextBox _staticInfoTextBox;

        private string currentTimeStamp;

        public DnmConnectInfoResp(StaticInfoTextBox staticInfoTextBox)
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
                    string url = "http://localhost:5000/api/clients/dynamicinfo/connectinfo";

                    using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

                    request.Headers.Add("ClientId", Id);

                    HttpResponseMessage response = await client.SendAsync(request, token);

                    if (response.IsSuccessStatusCode)
                    {
                        var dnmConnectInfo = await response.Content.ReadFromJsonAsync<DynamicDataFromServer>();

                        if (dnmConnectInfo != null)
                        {
                            var connectData = JsonSerializer.Deserialize<DnmConnectInfo>(dnmConnectInfo.JsonPayload, jsonOptions);

                            if (connectData != null && dnmConnectInfo.TimeStamp != currentTimeStamp)
                            {
                                var sb = new StringBuilder();
                                sb.AppendLine($"Computer: {connectData.ComputerName} | Time: {dnmConnectInfo.TimeStamp}");
                                sb.AppendLine(new string('-', 50));

                                if (connectData.Connection != null)
                                {
                                    foreach (var connect in connectData.Connection)
                                    {
                                        sb.AppendLine($"Local connection: {connect.LocalConnection,-20} | Remote connection: {connect.RemoteConnection}");
                                    }
                                }

                                _staticInfoTextBox.DnmInfoText = sb.ToString() + Environment.NewLine;
                                currentTimeStamp = dnmConnectInfo.TimeStamp;
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
