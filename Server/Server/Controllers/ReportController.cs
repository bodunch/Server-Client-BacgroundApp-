using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemController : ControllerBase
    {
        [HttpPost]
        public IActionResult RevievSystemInfo([FromBody] JsonObject info)
        {
            Console.WriteLine(info);
            return Ok();
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ComputerController : ControllerBase
    {
        [HttpPost]
        public IActionResult RevievComputerInfo([FromBody] JsonObject info)
        {
            Console.WriteLine(info);
            return Ok();
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class CpuController : ControllerBase
    {
        [HttpPost]
        public IActionResult RevievCPUInfo([FromBody] JsonObject info)
        {
            Console.WriteLine(info);
            return Ok();
        }
    }
}
