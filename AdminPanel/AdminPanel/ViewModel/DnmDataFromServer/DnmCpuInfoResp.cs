using AdminPanel.Model;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace AdminPanel.ViewModel.DnmDataFromServer
{
    public class DnmCpuInfoResp
    {
        private readonly StaticInfoTextBox _staticInfoTextBox;

        private string currentTimeStamp;

        public DnmCpuInfoResp(StaticInfoTextBox staticInfoTextBox)
        {
            _staticInfoTextBox = staticInfoTextBox;
        }

        public async Task TakeData(string Id)
        {
            using HttpClient client = new HttpClient();

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            while (true)
            {
                try
                {
                    string url = "http://localhost:5000/api/clients/dynamicinfo/cpuinfo";

                    using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

                    request.Headers.Add("ClientId", Id);

                    HttpResponseMessage response = await client.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        var dnmCpuInfo = await response.Content.ReadFromJsonAsync<DynamicDataFromServer>();

                        if (dnmCpuInfo != null)
                        {
                            
                            var cpuData = JsonSerializer.Deserialize<DnmCpuInfo>(dnmCpuInfo.JsonPayload, jsonOptions);

                            if(cpuData != null && dnmCpuInfo.TimeStamp != currentTimeStamp)
                            {
                                _staticInfoTextBox.DnmCpuInfoText += $"Computer name: {cpuData.ComputerName} | " +
                                    $"Load CPU: {cpuData.LoadCPU} | " +
                                    $"Error code: {cpuData.ErrorCode} | " +
                                    $"Status: {cpuData.Status} | " +
                                    $"Time stamp: {dnmCpuInfo.TimeStamp} | " +
                                    Environment.NewLine;
                                currentTimeStamp = dnmCpuInfo.TimeStamp;
                            }
                        }
                        else
                        {
                            _staticInfoTextBox.DnmCpuInfoText = "Data not found";
                        }
                    }
                    else
                    {
                        _staticInfoTextBox.DnmCpuInfoText = $"HTTP Error: {(int)response.StatusCode}";
                    }
                }
                catch (Exception ex)
                {
                    _staticInfoTextBox.DnmCpuInfoText = $"ERROR: {ex.Message}";
                }

                await Task.Delay(5000);
            }
        }
    }
    
}
