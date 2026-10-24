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
    public class AppChart
    {
        private readonly CartesianChart _chart;
        private static readonly ObservableCollection<double> _appValues = new();
        private static readonly ObservableCollection<string> _appNames = new();

        public AppChart(CartesianChart chart)
        {
            _chart = chart;

            _chart.Series = new ISeries[]
            {
                new ColumnSeries<double>
                {
                    Values = _appValues,
                    MaxBarWidth = 25
                }
            };

            _chart.XAxes = new Axis[]
            {
                new Axis
                {
                    Labels = _appNames,
                    LabelsRotation = 45,
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

                    string endpoint = "api/charts/app";
                    HttpResponseMessage response = await httpClient.GetAsync(endpoint, token);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                        var points = JsonSerializer.Deserialize<List<AppLogPoint>>(jsonResponse, options);

                        if (points != null && points.Any())
                        {
                            System.Windows.Application.Current.Dispatcher.Invoke(() =>
                            {
                                _appValues.Clear();
                                _appNames.Clear();

                                foreach (var pt in points)
                                {
                                    _appValues.Add(pt.Value);
                                    _appNames.Add(pt.Name);
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
                _appValues.Clear();
                _appNames.Clear();
            });
        }
    }
}
