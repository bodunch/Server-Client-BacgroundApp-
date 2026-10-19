using AdminPanel.Model;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using AdminPanel.Services;
using System.Windows.Controls;

namespace AdminPanel.ViewModel.DnmDataFromServer
{
    public class DnmCpuInfoResp
    {
        private readonly RichTextBox _richTextBox;

        private string currentTimeStamp;

        public DnmCpuInfoResp(RichTextBox richTextBox)
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
                    string url = "http://localhost:5000/api/clients/dynamicinfo/cpuinfo";

                    using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

                    request.Headers.Add("ClientId", Id);

                    HttpResponseMessage response = await client.SendAsync(request, token);

                    if (response.IsSuccessStatusCode)
                    {
                        var dnmCpuInfo = await response.Content.ReadFromJsonAsync<DynamicDataFromServer>();

                        if (dnmCpuInfo != null)
                        {
                            
                            var cpuData = JsonSerializer.Deserialize<DnmCpuInfo>(dnmCpuInfo.JsonPayload, jsonOptions);

                            if(cpuData != null && dnmCpuInfo.TimeStamp != currentTimeStamp)
                            {
                                string logText = $"Computer name: {cpuData.ComputerName} | " +
                                    $"Load CPU: {cpuData.LoadCPU} | " +
                                    $"Error code: {cpuData.ErrorCode} | " +
                                    $"Status: {cpuData.Status} | " +
                                    $"Time stamp: {dnmCpuInfo.TimeStamp} | ";

                                string color = DetermineCurrTextColor.CpuDataColor(cpuData);
                                switch(color)
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
                                
                                currentTimeStamp = dnmCpuInfo.TimeStamp;
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
