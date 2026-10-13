using AdminPanel.Model;
using AdminPanel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AdminPanel.ViewModel.DnmDataFromServer
{
    public class DnmPortsInfoResp
    {
        private readonly RichTextBox _richTextBox;

        private string currentTimeStamp;

        public DnmPortsInfoResp(RichTextBox richTextBox)
        {
            _richTextBox = richTextBox;
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
                                if (portsData.Port != null)
                                {
                                    foreach (var port in portsData.Port)
                                    {
                                        string logText =
                                             $"Adress: {port.Adress} | " +
                                             $"Port: {port.Port}";

                                        string color = DetermineCurrTextColor.PortsDataColor(port);

                                        switch (color)
                                        {
                                            case "red":
                                                RichTextBoxHelper.AppendRedText(_richTextBox, logText);
                                                break;

                                            case "yellow":
                                                RichTextBoxHelper.AppendYellowText(_richTextBox, logText);
                                                break;

                                            case "black":
                                                RichTextBoxHelper.AppendBlackText(_richTextBox, logText);
                                                break;

                                            default:
                                                RichTextBoxHelper.AppendBlackText(_richTextBox, logText);
                                                break;
                                        }
                                    }
                                }

                                currentTimeStamp = dnmPortsInfo.TimeStamp;
                            }
                        }
                        else
                        {
                            RichTextBoxHelper.AppendRedText(_richTextBox, "Data not found");
                        }
                    }
                    else
                    {
                        RichTextBoxHelper.AppendRedText(_richTextBox, $"HTTP Error: {(int)response.StatusCode}");
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    RichTextBoxHelper.AppendRedText(_richTextBox, $"ERROR: {ex.Message}");
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
