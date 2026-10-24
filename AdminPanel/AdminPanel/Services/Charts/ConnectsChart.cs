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
    public class ConnectsChart
    {
        private readonly CartesianChart _chart;
        private static readonly ObservableCollection<double> _connectValues = new();
        private static readonly ObservableCollection<string> _connectNames = new();

        public ConnectsChart(CartesianChart chart)
        {
            _chart = chart;

            _chart.Series = new ISeries[]
            {
                new ColumnSeries<double>
                {
                    Values = _connectValues,
                    MaxBarWidth = 35
                }
            };

            _chart.XAxes = new Axis[]
            {
                new Axis
                {
                    Labels = _connectNames,
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

                    string endpoint = "api/charts/connections";
                    HttpResponseMessage response = await httpClient.GetAsync(endpoint, token);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                        var points = JsonSerializer.Deserialize<List<ConnectLogPoint>>(jsonResponse, options);

                        if (points != null && points.Any())
                        {
                            System.Windows.Application.Current.Dispatcher.Invoke(() =>
                            {
                                _connectValues.Clear();
                                _connectNames.Clear();

                                foreach (var pt in points)
                                {
                                    _connectValues.Add(pt.Value);
                                    _connectNames.Add(pt.Name);
                                }
                            });
                        }
                    }

                    try
                    {
                        await Task.Delay(10000, token);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                }
                catch { }
            }
        }

        public static void ClearChart()
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                _connectValues.Clear();
                _connectNames.Clear();
            });
        }
    }
}
