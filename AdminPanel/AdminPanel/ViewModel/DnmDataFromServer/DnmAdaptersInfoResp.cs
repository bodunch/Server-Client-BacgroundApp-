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
    public class DnmAdaptersInfoResp
    {
        private readonly RichTextBox _richTextBox;

        private string currentTimeStamp;

        public DnmAdaptersInfoResp(RichTextBox richTextBox)
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
                                if (adapterData.Adapter != null)
                                {
                                    foreach (var adapter in adapterData.Adapter)
                                    {
                                        string logText = $"Name: {adapter.Name} |" +
                                            $" Status: {adapter.Status} | " +
                                            $"Speed: {adapter.Speed} | " +
                                            $"Received: {adapter.Received} | " +
                                            $"Sent: {adapter.Sent}";

                                        string color = DetermineCurrTextColor.AdapterDataColor(adapter);

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

                                currentTimeStamp = dnmAdaptersInfo.TimeStamp;
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
