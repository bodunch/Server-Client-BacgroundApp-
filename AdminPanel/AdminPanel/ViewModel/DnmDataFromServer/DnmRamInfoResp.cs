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
    public class DnmRamInfoResp
    {
        private readonly StaticInfoTextBox _staticInfoTextBox;

        private string currentTimeStamp;

        public DnmRamInfoResp(StaticInfoTextBox staticInfoTextBox)
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
                    string url = "http://localhost:5000/api/clients/dynamicinfo/raminfo";

                    using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

                    request.Headers.Add("ClientId", Id);

                    HttpResponseMessage response = await client.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        var dnmRamInfo = await response.Content.ReadFromJsonAsync<DynamicDataFromServer>();

                        if (dnmRamInfo != null)
                        {
                            var ramData = JsonSerializer.Deserialize<DnmRamInfo>(dnmRamInfo.JsonPayload, jsonOptions);

                            if (ramData != null && dnmRamInfo.TimeStamp != currentTimeStamp)
                            {
                                _staticInfoTextBox.DnmInfoText += $"Computer name: {ramData.ComputerName} | " +
                                    $"Total memory: {ramData.TotalMem} | " +
                                    $"Free memory: {ramData.FreeMem} | " +
                                    Environment.NewLine;
                                    currentTimeStamp = dnmRamInfo.TimeStamp;
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
                catch(Exception ex)
                {
                    _staticInfoTextBox.DnmInfoText = $"ERROR: {ex.Message}";
                }

                await Task.Delay(5000);
            }
        }
    }
}
