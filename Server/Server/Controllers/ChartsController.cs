using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using System.Text.Json.Nodes;
using Server.Data.Entities;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ChartsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("cpu")]
        public async Task<IActionResult> GetCpuChartData()
        {
            if (!Request.Headers.TryGetValue("ClientId", out var headerValues) || !int.TryParse(headerValues.FirstOrDefault(), out int clientId))
            {
                return BadRequest();
            }

            var cpuLogs = await _context.DnmCpuInfo
                .Where(s => s.ClientId == clientId)
                .OrderBy(s => s.Id)
                .ToListAsync();

            if (cpuLogs.Count < 12)
            {
                return BadRequest();
            }

            var chartPoints = new List<object>();

            foreach (var log in cpuLogs)
            {
                try
                {
                    var jsonNode = JsonNode.Parse(log.JsonPayload);

                    string loadStr = jsonNode?["loadCPU"]?.ToString()?.Replace("%", "").Trim() ?? "0";

                    if (double.TryParse(loadStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double loadValue))
                    {
                        chartPoints.Add(new
                        {
                            Time = log.TimeStamp,
                            Value = loadValue
                        });
                    }
                }
                catch { }
            }

            return Ok(chartPoints);
        }

        [HttpGet("ram")]
        public async Task<IActionResult> GetRamChartData()
        {
            if (!Request.Headers.TryGetValue("ClientId", out var headerValues) || !int.TryParse(headerValues.FirstOrDefault(), out int clientId))
            {
                return BadRequest();
            }

            var ramLogs = await _context.DnmRamInfo
                .Where(s => s.ClientId == clientId)
                .OrderBy(s => s.Id)
                .ToListAsync();

            if (ramLogs.Count < 12)
            {
                return BadRequest();
            }

            var chartPoints = new List<object>();

            foreach (var log in ramLogs)
            {
                try
                {
                    var jsonNode = JsonNode.Parse(log.JsonPayload);

                    string loadStr = jsonNode?["freeMem"]?.ToString()?.Trim() ?? "0";

                    if (double.TryParse(loadStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double loadValue))
                    {
                        chartPoints.Add(new
                        {
                            Time = log.TimeStamp,
                            Value = loadValue
                        });
                    }
                }
                catch { }
            }

            return Ok(chartPoints);
        }

        [HttpGet("processes")]
        public async Task<IActionResult> GetProcessesChartData()
        {
            if (!Request.Headers.TryGetValue("ClientId", out var headerValues) || !int.TryParse(headerValues.FirstOrDefault(), out int clientId))
            {
                return BadRequest();
            }

            var procLogs = await _context.DnmProcessesInfo
                .Where(s => s.ClientId == clientId)
                .OrderBy(s => s.Id)
                .ToListAsync();

            if (!procLogs.Any())
            {
                return BadRequest();
            }

            var chartPoints = new List<object>();

            try
            {
                var latestLog = procLogs.Last();
                var jsonNode = JsonNode.Parse(latestLog.JsonPayload);
                var processesArray = jsonNode?["process"]?.AsArray();

                if (processesArray != null)
                {
                    var topProcesses = processesArray
                        .Select(p => new
                        {
                            Name = p?["name"]?.ToString() ?? "Unknown",
                            RamValue = ParseRamToMegabytes(p?["ram"]?.ToString() ?? "0")
                        })
                        .OrderByDescending(x => x.RamValue)
                        .Take(10);

                    foreach (var p in topProcesses)
                    {
                        chartPoints.Add(new
                        {
                            Name = p.Name,
                            Value = p.RamValue
                        });
                    }
                }
            }
            catch { }

            return Ok(chartPoints);
        }

        [HttpGet("ports")]
        public async Task<IActionResult> GetPortsChartData()
        {
            if (!Request.Headers.TryGetValue("ClientId", out var headerValues) || !int.TryParse(headerValues.FirstOrDefault(), out int clientId))
            {
                return BadRequest();
            }

            var portLogs = await _context.DnmPortsInfo
                .Where(s => s.ClientId == clientId)
                .OrderBy(s => s.Id)
                .ToListAsync();

            if (!portLogs.Any())
            {
                return BadRequest();
            }

            var chartPoints = new List<object>();

            var latestPort = portLogs.Last();
            var jsonNode = JsonNode.Parse(latestPort.JsonPayload);
            var portsArray = jsonNode?["port"]?.AsArray();

            if (portsArray != null)
            {
                var groupedPorts = portsArray
                    .GroupBy(p => p?["adress"]?.ToString() ?? "Unknown")
                    .Select(g => new {
                        Address = g.Key,
                        Count = g.Count()
                    });

                foreach (var group in groupedPorts)
                {
                    chartPoints.Add(new
                    {
                        Name = group.Address,
                        Value = group.Count
                    });
                }
            }
            return Ok(chartPoints);
        }

        [HttpGet("connections")]
        public async Task<IActionResult> GetConnectionsChartData()
        {
            if (!Request.Headers.TryGetValue("ClientId", out var headerValues) || !int.TryParse(headerValues.FirstOrDefault(), out int clientId))
            {
                return BadRequest();
            }

            var connectLogs = await _context.DnmConnectionsInfo
                .Where(s => s.ClientId == clientId)
                .OrderBy(s => s.Id)
                .ToListAsync();

            if (!connectLogs.Any())
            {
                return BadRequest();
            }

            var chartPoints = new List<object>();

            var latestConnect = connectLogs.Last();
            var jsonNode = JsonNode.Parse(latestConnect.JsonPayload);
            var connectArray = jsonNode?["connection"]?.AsArray();

            if (connectArray != null)
            {
                var groupedConnects = connectArray
                    .GroupBy(p => p?["localConnection"]?.ToString() ?? "Unknown")
                    .Select(g => new
                    {
                        Address = g.Key,
                        Count = g.Count()
                    });

                foreach (var group in groupedConnects)
                {
                    chartPoints.Add(new
                    {
                        Name = group.Address,
                        Value = group.Count
                    });
                }
            }
            return Ok(chartPoints);
        }

        [HttpGet("app")]
        public async Task<IActionResult> GetAppChartData()
        {
            if (!Request.Headers.TryGetValue("ClientId", out var headerValues) || !int.TryParse(headerValues.FirstOrDefault(), out int clientId))
            {
                return BadRequest();
            }

            var appLogs = await _context.DnmAppInfo
                .Where(s => s.ClientId == clientId)
                .OrderBy(s => s.Id)
                .ToListAsync();

            if (!appLogs.Any())
            {
                return BadRequest();
            }

            var chartPoints = new List<object>();

            try
            {
                var latestLog = appLogs.Last();
                var jsonNode = JsonNode.Parse(latestLog.JsonPayload);
                var appArray = jsonNode?["application"]?.AsArray();

                if (appArray != null)
                {
                    var topApp = appArray
                        .Select(p => new
                        {
                            Name = p?["appName"]?.ToString() ?? "Unknown",
                            RamValue = ParseRamToMegabytes(p?["ram"]?.ToString() ?? "0")
                        })
                        .OrderByDescending(x => x.RamValue)
                        .Take(10);

                    foreach (var p in topApp)
                    {
                        chartPoints.Add(new
                        {
                            Name = p.Name,
                            Value = p.RamValue
                        });
                    }
                }
            }
            catch { }

            return Ok(chartPoints);
        }

        [HttpGet("adapters")]
        public async Task<IActionResult> GetAdaptersChartData()
        {
            if (!Request.Headers.TryGetValue("ClientId", out var headerValues) || !int.TryParse(headerValues.FirstOrDefault(), out int clientId))
            {
                return BadRequest();
            }

            var adapterLogs = await _context.DnmAdaptersInfo
                .Where(s => s.ClientId == clientId)
                .OrderBy(s => s.Id)
                .ToListAsync();

            if (!adapterLogs.Any())
            {
                return BadRequest();
            }

            var chartPoints = new List<object>();

            var latestLog = adapterLogs.Last();
            var jsonNode = JsonNode.Parse(latestLog.JsonPayload);
            var adaptersArray = jsonNode?["adapter"]?.AsArray();

            if (adaptersArray != null)
            {
                foreach (var ad in adaptersArray)
                {
                    string name = ad?["name"]?.ToString() ?? "Unknown";
                    double received = ParseTrafficToMegabytes(ad?["received"]?.ToString() ?? "0");
                    double sent = ParseTrafficToMegabytes(ad?["sent"]?.ToString() ?? "0");

                    chartPoints.Add(new
                    {
                        Name = name,
                        Received = received,
                        Sent = sent
                    });
                }
            }
            return Ok(chartPoints);
        }

        private double ParseRamToMegabytes(string ramStr)
        {
            if (string.IsNullOrEmpty(ramStr)) return 0;

            string cleanStr = ramStr.Replace(',', '.');

            cleanStr = new string(cleanStr.Where(c => char.IsDigit(c) || c == '.' || c == '-').ToArray());

            if (double.TryParse(cleanStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double result))
            {
                return result;
            }
            return 0;
        }

        private double ParseTrafficToMegabytes(string trafficStr)
        {
            if (string.IsNullOrEmpty(trafficStr)) return 0;

            string cleanStr = trafficStr.Replace(',', '.');
            double multiplier = 1.0;

            if (cleanStr.Contains("KB", StringComparison.OrdinalIgnoreCase))
            {
                multiplier = 0.001; 
            }
            else if (cleanStr.Contains("GB", StringComparison.OrdinalIgnoreCase))
            {
                multiplier = 1000.0;// Переводимо гігабайти в мегабайти
            }

            cleanStr = new string(cleanStr.Where(c => char.IsDigit(c) || c == '.').ToArray());

            if (double.TryParse(cleanStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double result))
            {
                return Math.Round(result * multiplier, 2);
            }
            return 0;
        }
    }
}
