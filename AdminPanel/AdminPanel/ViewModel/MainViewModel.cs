using AdminPanel.Model;
using AdminPanel.Services.Charts;
using AdminPanel.ViewModel;
using AdminPanel.ViewModel.DnmDataFromServer;
using LiveChartsCore.SkiaSharpView.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;

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

        public async Task DetermineDataType(string Id, string DataType, RichTextBox targetRichTextBox, CartesianChart _CartesianChart)
        {
            _currentCts?.Cancel();
            _currentCts = new CancellationTokenSource();
            var token = _currentCts.Token;

            staticInfoTextBox.DnmInfoText = string.Empty;
            targetRichTextBox.Document.Blocks.Clear();

            CpuChart.ClearChart();
            RamChart.ClearChart();
            ProcessesChart.ClearChart();

            switch (DataType)
            {
                case "CpuInfo":
                    DnmCpuInfoResp dnmCpu = new DnmCpuInfoResp(targetRichTextBox);
                    _ = dnmCpu.TakeData(Id, token);

                    CpuChart cpuChart = new CpuChart(_CartesianChart);
                    _ = cpuChart.TakeDataForChart(Id, token);

                    break;

                case "RamInfo":
                    DnmRamInfoResp dnmRam = new DnmRamInfoResp(targetRichTextBox);
                    _ = dnmRam.TakeData(Id, token);

                    RamChart ramChart = new RamChart(_CartesianChart);
                    _ = ramChart.TakeDataForChart(Id, token);
                    break;

                case "ProcessInfo":
                    DnmProcInfoResp dnmProc = new DnmProcInfoResp(targetRichTextBox);
                    _ = dnmProc.TakeData(Id, token);

                    ProcessesChart processChart = new ProcessesChart(_CartesianChart);
                    _ = processChart.TakeDataForChart(Id, token);
                    break;

                case "PortsInfo":
                    DnmPortsInfoResp dnmPorts = new DnmPortsInfoResp(targetRichTextBox);
                    _ = dnmPorts.TakeData(Id, token);
                    break;

                case "ConnectionsInfo":
                    DnmConnectInfoResp dnmConnect = new DnmConnectInfoResp(targetRichTextBox);
                    _ = dnmConnect.TakeData(Id, token);
                    break;

                case "ApplicationsInfo":
                    DnmAppInfoResp dnmApp = new DnmAppInfoResp(targetRichTextBox);
                    _ = dnmApp.TakeData(Id, token);
                    break;

                case "AdaptersInfo":
                    DnmAdaptersInfoResp dnmAdapter = new DnmAdaptersInfoResp(targetRichTextBox);
                    _ = dnmAdapter.TakeData(Id, token);
                    break;

                default:
                    break;
            }
        }
    }
}
