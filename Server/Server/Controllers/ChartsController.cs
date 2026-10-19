using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using System.Text.Json.Nodes;

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
                .Take(10) 
                .ToListAsync();

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
    }
}
