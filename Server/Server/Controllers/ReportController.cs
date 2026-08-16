using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Text.Json;
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

    [Route("api/[controller]")]
    [ApiController]
    public class RamController : ControllerBase
    {
        [HttpPost]
        public IActionResult RevievRAMInfo([FromBody] JsonObject info)
        {
            Console.WriteLine(info);
            return Ok();
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class CurrcpuController : ControllerBase
    {
        [HttpPost]
        public IActionResult RevievCurrCPUInfo([FromBody] JsonObject info)
        {
            Console.WriteLine(info);
            return Ok();
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class CurrramController : ControllerBase
    {
        [HttpPost]
        public IActionResult RevievCurrRAMInfo([FromBody] JsonObject info)
        {
            Console.WriteLine(info);
            return Ok();
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ProcessesController : ControllerBase
    {
        [HttpPost]
        public IActionResult RevievProcesses([FromBody] JsonObject info)
        {
            Console.WriteLine(info);
            return Ok();
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class AdaptersController : ControllerBase
    {
        [HttpPost]
        public IActionResult RevievAdapters([FromBody] JsonObject info)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };

            string readableJson = info.ToJsonString(options);
            Console.WriteLine(readableJson);
            return Ok();
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ConnectionsController : ControllerBase
    {
        [HttpPost]
        public IActionResult RevievConnections([FromBody] JsonObject info)
        {
            Console.WriteLine(info);
            return Ok();
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class PortsController : ControllerBase
    {
        [HttpPost]
        public IActionResult RevievPorts([FromBody] JsonObject info)
        {
            Console.WriteLine(info);
            return Ok();
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class AppController : ControllerBase
    {
        [HttpPost]
        public IActionResult RevievApplications([FromBody] JsonObject info)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };

            string readableJson = info.ToJsonString(options);
            Console.WriteLine(readableJson);
            return Ok();
        }
    }
}
