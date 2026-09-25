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
    public class DnmProcInfoResp
    {
        private readonly StaticInfoTextBox _staticInfoTextBox;

        private string currentTimeStamp;

        public DnmProcInfoResp(StaticInfoTextBox staticInfoTextBox)
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
                    string url = "http://localhost:5000/api/clients/dynamicinfo/procinfo";

                    using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

                    request.Headers.Add("ClientId", Id);

                    HttpResponseMessage response = await client.SendAsync(request, token);

                    if (response.IsSuccessStatusCode)
                    {
                        var dnmProcInfo = await response.Content.ReadFromJsonAsync<DynamicDataFromServer>();

                        if (dnmProcInfo != null)
                        {
                            var procData = JsonSerializer.Deserialize<DnmProcessesInfo>(dnmProcInfo.JsonPayload, jsonOptions);

                            if (procData != null && dnmProcInfo.TimeStamp != currentTimeStamp)
                            {
                                var sb = new StringBuilder();
                                sb.AppendLine($"Computer: {procData.ComputerName} | Time: {dnmProcInfo.TimeStamp}");
                                sb.AppendLine(new string('-', 50));

                                if (procData.Process != null)
                                {
                                    foreach (var proc in procData.Process)
                                    {
                                        sb.AppendLine($"Name: {proc.Name,-20} | Id: {proc.Id,-6} | Ram: {proc.RAM,-10} | Start time: {proc.StartTime} | Path: {proc.Path}");
                                    }
                                }

                                _staticInfoTextBox.DnmInfoText = sb.ToString() + Environment.NewLine;
                                currentTimeStamp = dnmProcInfo.TimeStamp;
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
