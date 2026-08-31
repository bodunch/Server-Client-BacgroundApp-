using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;
using Server.Data;
using Server.Data.Entities;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SystemController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult RevievSystemInfo([FromBody] JsonObject info)
        {
            if (info == null)
                return BadRequest();

            string rawMachineName = info["machineName"]?.ToString() ?? info["MachineName"]?.ToString() ?? info["ComputerName"]?.ToString(); 
            string machineName = string.IsNullOrWhiteSpace(rawMachineName) ? "Office-PC" : rawMachineName;

            int clientId = ClientHelper.GetOrAddClient(_context, machineName);

            var systemInfo = new SystemInfoEntity
            {
                ClientId = clientId,
                OperatingSystem = info["OperatingSystem"]?.ToString() ?? "",
                Version = info["Version"]?.ToString() ?? "",
                ComputerName = info["ComputerName"]?.ToString() ?? "",
                RegisteredUser = info["RegisteredUser"]?.ToString() ?? "",
                LastBootTime = info["LastBootTime"]?.ToString() ?? ""
            };

            _context.SystemInfo.Add(systemInfo);
            _context.SaveChanges();

            return Ok();
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ComputerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ComputerController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult RevievComputerInfo([FromBody] JsonObject info)
        {
            if (info == null)
                return BadRequest();

            string rawMachineName = info["machineName"]?.ToString() ?? info["MachineName"]?.ToString() ?? info["ComputerName"]?.ToString();
            string machineName = string.IsNullOrWhiteSpace(rawMachineName) ? "Office-PC" : rawMachineName;

            int clientId = ClientHelper.GetOrAddClient(_context, machineName);

            var computerInfo = new ComputerInfoEntity
            {
                ClientId = clientId,
                Manufacturer = info["Manufacturer"]?.ToString() ?? "",
                PCModel = info["PCModel"]?.ToString() ?? "",
                SystemType = info["SystemType"]?.ToString() ?? "",
                CountOfCpu = info["CountOfCPU"]?.ToString() ?? "",
                SystemStart = info["SystemStart"]?.ToString() ?? "",
                StatusOfStart = info["StatusOfStart"]?.ToString() ?? "",
            };

            _context.ComputerInfo.Add(computerInfo);
            _context.SaveChanges();

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
