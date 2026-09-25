using AdminPanel.Model;
using AdminPanel.ViewModel.DnmDataFromServer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using AdminPanel.ViewModel;
using System.Net.Http.Headers;
using System.Diagnostics.Contracts;

namespace AdminPanel.ViewModel
{
    public class MainViewModel 
    {
        public ObservableCollection<Clients> ClientsItem { get; set; } = new();

        public StaticInfoTextBox staticInfoTextBox { get; set; } = new();

        public string SelectedClientId { get; private set; }

        public MainViewModel()
        {
            _ = StartPollingAsync();
        }

        private async Task StartPollingAsync()
        {
            using HttpClient client = new HttpClient();

            while (true)
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync("http://localhost:5000/api/clients");

                    if (response.IsSuccessStatusCode)
                    {
                        var clientsList = await response.Content.ReadFromJsonAsync<List<Clients>>();

                        if (clientsList != null)
                        {
                            ClientsItem.Clear();

                            foreach (var c in clientsList)
                            {
                                if (!ClientsItem.Any(existing => existing.Id == c.Id))
                                {
                                    ClientsItem.Add(new Clients { Id = c.Id, MachineName = c.MachineName });
                                }
                            }
                        }
                    }
                }
                catch
                {

                }

                await Task.Delay(5000);
            }
        }

        private CancellationTokenSource? _currentCts;

        public async Task ClientInfo(string Id)
        {
            SelectedClientId = Id;

            using HttpClient client = new HttpClient();

            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/api/clients/staticinfo/system");
                request.Headers.Add("ClientId", Id);

                HttpResponseMessage response = await client.SendAsync(request);

                var systemInfo = await response.Content.ReadFromJsonAsync<SystemInfo>();

                if (systemInfo != null)
                {
                    staticInfoTextBox.SystemInfoText = $"OS: {systemInfo.OperatingSystem}\n" +
                                 $"Version: {systemInfo.Version}\n" +
                                 $"Pc Name: {systemInfo.ComputerName}\n" +
                                 $"User: {systemInfo.RegisteredUser}\n" +
                                 $"Last Boot Time: {systemInfo.LastBootTime}";
                }
                else
                {
                    staticInfoTextBox.SystemInfoText = "Data not found!";
                }
            }
            catch
            {

            }

            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/api/clients/staticinfo/computer");
                request.Headers.Add("ClientId", Id);

                HttpResponseMessage response = await client.SendAsync(request);

                var copmuterInfo = await response.Content.ReadFromJsonAsync<ComputerInfo>();

                if (copmuterInfo != null)
                {
                    staticInfoTextBox.ComputerInfoText = $"Manufacturer: {copmuterInfo.Manufacturer}\n" +
                                 $"PC Model: {copmuterInfo.PCModel}\n" +
                                 $"System Type: {copmuterInfo.SystemType}\n" +
                                 $"Count of Cpu: {copmuterInfo.CountOfCpu}\n" +
                                 $"System Start at: {copmuterInfo.SystemStart}" +
                                 $"Status of Start: {copmuterInfo.StatusOfStart}";
                }
                else
                {
                    staticInfoTextBox.ComputerInfoText = "Data not found!";
                }
            }
            catch
            {

            }

            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/api/clients/staticinfo/cpu");
                request.Headers.Add("ClientId", Id);

                HttpResponseMessage response = await client.SendAsync(request);

                var cpuInfo = await response.Content.ReadFromJsonAsync<CpuInfo>();

                if (cpuInfo != null)
                {
                    staticInfoTextBox.CpuInfoText = $"CPU Name: {cpuInfo.CPUName}\n" +
                                 $"Manufacturer: {cpuInfo.Manufacturer}\n" +
                                 $"Num of Cores: {cpuInfo.NumOfCores}\n" +
                                 $"Num of Streams: {cpuInfo.NumOfStreams}";
                }
                else
                {
                    staticInfoTextBox.CpuInfoText = "Data not found!";
                }
            }
            catch
            {

            }

            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/api/clients/staticinfo/ram");
                request.Headers.Add("ClientId", Id);

                HttpResponseMessage response = await client.SendAsync(request);

                var ramInfo = await response.Content.ReadFromJsonAsync<RamInfo>();

                if (ramInfo != null)
                {
                    staticInfoTextBox.RamInfoText = $"RAM Type: {ramInfo.Type}\n" +
                                 $"Part Number: {ramInfo.PartNumber}\n" +
                                 $"Frequency: {ramInfo.Frequency}\n" +
                                 $"Memory Count: {ramInfo.MemoryCount}";
                }
                else
                {
                    staticInfoTextBox.RamInfoText = "Data not found!";
                }
            }
            catch
            {

            }
        }

        public async Task DetermineDataType(string Id, string DataType)
        {
            _currentCts?.Cancel();
            _currentCts = new CancellationTokenSource();
            var token = _currentCts.Token;

            staticInfoTextBox.DnmInfoText = string.Empty;

            switch (DataType)
            {
                case "CpuInfo":
                    DnmCpuInfoResp dnmCpu = new DnmCpuInfoResp(staticInfoTextBox);
                    _ = dnmCpu.TakeData(Id, token);
                    break;

                case "RamInfo":
                    DnmRamInfoResp dnmRam = new DnmRamInfoResp(staticInfoTextBox);
                    _ = dnmRam.TakeData(Id, token);
                    break;

                case "ProcessInfo":
                    DnmProcInfoResp dnmProc = new DnmProcInfoResp(staticInfoTextBox);
                    _ = dnmProc.TakeData(Id, token);
                    break;

                case "PortsInfo":
                    DnmPortsInfoResp dnmPorts = new DnmPortsInfoResp(staticInfoTextBox);
                    _ = dnmPorts.TakeData(Id, token);
                    break;

                case "ConnectionsInfo":
                    DnmConnectInfoResp dnmConnect = new DnmConnectInfoResp(staticInfoTextBox);
                    _ = dnmConnect.TakeData(Id, token);
                    break;

                case "ApplicationsInfo":
                    DnmAppInfoResp dnmApp = new DnmAppInfoResp(staticInfoTextBox);
                    _ = dnmApp.TakeData(Id, token);
                    break;

                case "AdaptersInfo":
                    DnmAdaptersInfoResp dnmAdapter = new DnmAdaptersInfoResp(staticInfoTextBox);
                    _ = dnmAdapter.TakeData(Id, token);
                    break;

                default:
                    break;
            }
        }
    }
}
