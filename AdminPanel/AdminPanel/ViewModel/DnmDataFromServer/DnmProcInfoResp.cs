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
    public class DnmProcInfoResp
    {
        private readonly RichTextBox _richTextBox;

        private string currentTimeStamp;

        public DnmProcInfoResp(RichTextBox richTextBox)
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
                                RichTextBoxHelper.AppendBlackText(_richTextBox, new string('-', 100));

                                if (procData.Process != null)
                                {
                                    foreach (var proc in procData.Process)
                                    {
                                        string logText =
                                            $"Name: {proc.Name} | " +
                                            $"Id: {proc.Id} | " +
                                            $"RAM: {proc.RAM} | " +
                                            $"SHA256: {proc.SHA256} | " +
                                            $"Start time: {proc.StartTime} | " +
                                            $"Path: {proc.Path}";

                                        string color = DetermineCurrTextColor.ProcDataColor(proc);

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

                                RichTextBoxHelper.AppendBlackText(_richTextBox, Environment.NewLine);

                                currentTimeStamp = dnmProcInfo.TimeStamp;
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
