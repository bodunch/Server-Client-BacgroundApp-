using AdminPanel.Model.ChartLogs;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdminPanel.Services.Charts
{
    public class AdaptersChart
    {
        private readonly CartesianChart _chart;
        private static readonly ObservableCollection<double> _receivedValues = new();
        private static readonly ObservableCollection<double> _sentValues = new();
        private static readonly ObservableCollection<string> _adapterNames = new();

        public AdaptersChart(CartesianChart chart)
        {
            _chart = chart;

            _chart.Series = new ISeries[]
            {
                new ColumnSeries<double>
                {
                    Values = _receivedValues,
                    Name = "Received (MB)",
                    MaxBarWidth = 20
                },
                new ColumnSeries<double>
                {
                    Values = _sentValues,
                    Name = "Sent (MB)",
                    MaxBarWidth = 20
                }
            };

            _chart.XAxes = new Axis[]
            {
                new Axis
                {
                    Labels = _adapterNames,
                    LabelsRotation = 15,
                    TextSize = 12
                }
            };
        }

        public async Task TakeDataForChart(string clientId, CancellationToken token)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                return;
            }

            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost:5000/");

            while (!token.IsCancellationRequested)
            {
                try
                {
                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Add("ClientId", clientId);

                    string endpoint = "api/charts/adapters";
                    HttpResponseMessage response = await httpClient.GetAsync(endpoint, token);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                        var points = JsonSerializer.Deserialize<List<AdapterLogPoint>>(jsonResponse, options);

                        if (points != null && points.Any())
                        {
                            System.Windows.Application.Current.Dispatcher.Invoke(() =>
                            {
                                _receivedValues.Clear();
                                _sentValues.Clear();
                                _adapterNames.Clear();

                                foreach (var pt in points)
                                {
                                    _receivedValues.Add(pt.Received);
                                    _sentValues.Add(pt.Sent);
                                    _adapterNames.Add(pt.Name);
                                }
                            });
                        }
                    }
                }
                catch { }

                try
                {
                    await Task.Delay(10000, token);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        public static void ClearChart()
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                _receivedValues.Clear();
                _sentValues.Clear();
                _adapterNames.Clear();
            });
        }
    }
}
