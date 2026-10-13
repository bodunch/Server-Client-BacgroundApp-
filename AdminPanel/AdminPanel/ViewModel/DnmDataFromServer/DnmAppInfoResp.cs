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
    public class DnmAppInfoResp
    {
        private readonly RichTextBox _richTextBox;

        private string currentTimeStamp;

        public DnmAppInfoResp(RichTextBox richTextBox)
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
                                if (appData.Application != null)
                                {
                                    foreach (var app in appData.Application)
                                    {
                                        string logText = $"App name: {app.AppName} | " +
                                            $"Window title: {app.WindowTitle} " +
                                            $"| Id: {app.Id} " +
                                            $"| RAM: {app.RAM}";

                                        string color = DetermineCurrTextColor.AppDataColor(app);

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
                                currentTimeStamp = dnmAppInfo.TimeStamp;
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
