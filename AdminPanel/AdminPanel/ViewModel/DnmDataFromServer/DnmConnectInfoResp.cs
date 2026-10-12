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
    public class DnmConnectInfoResp
    {
        private readonly RichTextBox _richTextBox;

        private string currentTimeStamp;

        public DnmConnectInfoResp(RichTextBox richTextBox)
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
                                if (connectData.Connection != null)
                                {
                                    foreach (var connect in connectData.Connection)
                                    {
                                        string logText =
                                             $"Local: {connect.LocalConnection} | " +
                                             $"Remote: {connect.RemoteConnection}";

                                        string color = DetermineCurrTextColor.ConnectDataColor(connect);

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

                                RichTextBoxHelper.AppendRedText(_richTextBox, "Data not found");
                                currentTimeStamp = dnmConnectInfo.TimeStamp;
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
