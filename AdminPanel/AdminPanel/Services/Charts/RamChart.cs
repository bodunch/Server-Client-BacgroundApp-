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
    public class RamChart
    {
        private readonly CartesianChart _chart;
        private static readonly ObservableCollection<double> _ramValues = new();
        private static readonly ObservableCollection<string> _timeLabels = new();

        public RamChart(CartesianChart chart)
        {
            _chart = chart;

            _chart.Series = new ISeries[]
            {
                new LineSeries<double>
                {
                    Values = _ramValues,
                    Fill = null,
                    LineSmoothness = 0.65
                }
            };

            _chart.XAxes = new Axis[]
            {
                new Axis
                {
                    Labels = _timeLabels,
                    LabelsRotation = 15
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

                    string endpoint = "api/charts/ram";

                    HttpResponseMessage response = await httpClient.GetAsync(endpoint, token);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();

                        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                        var points = JsonSerializer.Deserialize<List<RamLogPoint>>(jsonResponse, options);

                        if (points != null && points.Any())
                        {
                            System.Windows.Application.Current.Dispatcher.Invoke(() =>
                            {
                                _ramValues.Clear();
                                _timeLabels.Clear();

                                foreach (var pt in points)
                                {
                                    _ramValues.Add(pt.Value);

                                    string shortTime = pt.Time.Length >= 19 ? pt.Time.Substring(11, 8) : pt.Time;
                                    _timeLabels.Add(shortTime);
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
                _ramValues.Clear();
                _timeLabels.Clear();
            });
        }
    }
}
