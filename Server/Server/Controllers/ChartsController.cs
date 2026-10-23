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
            if(!Request.Headers.TryGetValue("ClientId", out var headerValues) || !int.TryParse(headerValues.FirstOrDefault(), out int clientId))
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
                        .Select(p => new {
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
    }
}
