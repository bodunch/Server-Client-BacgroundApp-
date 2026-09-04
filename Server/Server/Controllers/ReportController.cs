using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Data.DbQueue;
using Server.Data.Entities;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Nodes;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemController : ControllerBase
    {
        private readonly DatabaseQueueService _dbQueue;

        public SystemController(DatabaseQueueService dbQueue)
        {
            _dbQueue = dbQueue;
        }

        [HttpPost]
        public IActionResult RevievSystemInfo([FromBody] JsonObject info)
        {
            if (info == null)
                return BadRequest();

            string rawMachineName = info["machineName"]?.ToString() ?? info["MachineName"]?.ToString() ?? info["ComputerName"]?.ToString(); 
            string machineName = string.IsNullOrWhiteSpace(rawMachineName) ? "Office-PC" : rawMachineName;

            string operatingSystem = info["OperatingSystem"]?.ToString() ?? "";
            string version = info["Version"]?.ToString() ?? "";
            string computerName = info["ComputerName"]?.ToString() ?? "";
            string registeredUser = info["RegisteredUser"]?.ToString() ?? "";
            string lastBootTime = info["LastBootTime"]?.ToString() ?? "";

            _dbQueue.QueueWorkItem(async db =>
            {
                int clientId = ClientHelper.GetOrAddClient(db, machineName);

                var systemInfo = new SystemInfoEntity
                {
                    ClientId = clientId,
                    OperatingSystem = operatingSystem,
                    Version = version,
                    ComputerName = computerName,
                    RegisteredUser = registeredUser,
                    LastBootTime = lastBootTime
                };

                db.SystemInfo.Add(systemInfo);
                await db.SaveChangesAsync();
            });

            return Ok();
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ComputerController : ControllerBase
    {
        private readonly DatabaseQueueService _dbQueue;

        public ComputerController(DatabaseQueueService dbQueue)
        {
            _dbQueue = dbQueue;
        }

        [HttpPost]
        public IActionResult RevievComputerInfo([FromBody] JsonObject info)
        {
            if (info == null)
                return BadRequest();

            string rawMachineName = info["machineName"]?.ToString() ?? info["MachineName"]?.ToString() ?? info["ComputerName"]?.ToString();
            string machineName = string.IsNullOrWhiteSpace(rawMachineName) ? "Office-PC" : rawMachineName;

            string manufacturer = info["Manufacturer"]?.ToString() ?? "";
            string pcModel = info["PCModel"]?.ToString() ?? "";
            string systemType = info["SystemType"]?.ToString() ?? "";
            string countOfCpu = info["CountOfCPU"]?.ToString() ?? "";
            string systemStart = info["SystemStart"]?.ToString() ?? "";
            string statusOfStart = info["StatusOfStart"]?.ToString() ?? "";

            _dbQueue.QueueWorkItem(async db =>
            {
                int clientId = ClientHelper.GetOrAddClient(db, machineName);

                var copmuterInfo = new ComputerInfoEntity
                {
                    ClientId = clientId,
                    Manufacturer = manufacturer,  
                    PCModel = pcModel,
                    SystemType = systemType,
                    CountOfCpu = countOfCpu,
                    SystemStart = systemStart,
                    StatusOfStart = statusOfStart
                };

                db.ComputerInfo.Add(copmuterInfo);
                await db.SaveChangesAsync();
            });

            return Ok();
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class CpuController : ControllerBase
    {
        private readonly DatabaseQueueService _dbQueue;

        public CpuController(DatabaseQueueService dbQueue)
        {
            _dbQueue = dbQueue;
        }

        [HttpPost]
        public IActionResult RevievCPUInfo([FromBody] JsonObject info)
        {
            if (info == null)
                return BadRequest();

            string rawMachineName = info["machineName"]?.ToString() ?? info["MachineName"]?.ToString() ?? info["ComputerName"]?.ToString();
            string machineName = string.IsNullOrWhiteSpace(rawMachineName) ? "Office-PC" : rawMachineName;

            string cpuName = info["CPUName"]?.ToString() ?? "";
            string manufacturer = info["Manufacturer"]?.ToString() ?? "";
            string munOfCores = info["NumOfCores"]?.ToString() ?? "";
            string numOfStreams = info["NumOfStreams"]?.ToString() ?? "";

            _dbQueue.QueueWorkItem(async db =>
            {
                int clientId = ClientHelper.GetOrAddClient(db, machineName);

                var cpuInfo = new CpuInfoEntity
                {
                    ClientId = clientId,
                    CPUName = cpuName,
                    Manufacturer = manufacturer,
                    NumOfCores = munOfCores,
                    NumOfStreams = numOfStreams,
                };

                db.CpuInfo.Add(cpuInfo);
                await db.SaveChangesAsync();
            });

            return Ok();
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class RamController : ControllerBase
    {
        private readonly DatabaseQueueService _dbQueue;

        public RamController(DatabaseQueueService dbQueue)
        {
            _dbQueue = dbQueue;
        }

        [HttpPost]
        public IActionResult RevievRAMInfo([FromBody] JsonObject info)
        {
            if (info == null)
                return BadRequest();

            string rawMachineName = info["machineName"]?.ToString() ?? info["MachineName"]?.ToString() ?? info["ComputerName"]?.ToString();
            string machineName = string.IsNullOrWhiteSpace(rawMachineName) ? "Office-PC" : rawMachineName;

            string type = info["Type"]?.ToString() ?? "";
            string partNumber = info["PartNumber"]?.ToString() ?? "";
            string frequency = info["Frequency"]?.ToString() ?? "";
            string memoryCount = info["MemoryCount"]?.ToString() ?? "";

            _dbQueue.QueueWorkItem(async db =>
            {
                int clientId = ClientHelper.GetOrAddClient(db, machineName);

                var ramInfo = new RamInfoEntity
                {
                    ClientId = clientId,
                    Type = type,
                    PartNumber = partNumber,
                    Frequency = frequency,
                    MemoryCount = memoryCount
                };

                db.RamInfo.Add(ramInfo);
                await db.SaveChangesAsync();
            });

            return Ok();
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class CurrcpuController : ControllerBase
    {
        private readonly DatabaseQueueService _dbQueue;

        public CurrcpuController(DatabaseQueueService dbQueue)
        {
            _dbQueue = dbQueue;
        }

        [HttpPost]
        public IActionResult RevievCurrCPUInfo([FromBody] JsonObject info)
        {
            if (info == null)
                return BadRequest();

            string rawMachineName = info["machineName"]?.ToString() ?? info["MachineName"]?.ToString() ?? info["ComputerName"]?.ToString();
            string machineName = string.IsNullOrWhiteSpace(rawMachineName) ? "Office-PC" : rawMachineName;

            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = false
            };

            string jsonPayload = info.ToJsonString(options);
            string currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            _dbQueue.QueueWorkItem(async db =>
            {
                int clientId = ClientHelper.GetOrAddClient(db, machineName);

                var CurrCpuInfo = new DynamicCpuInfoEntity
                {
                    ClientId = clientId,
                    JsonPayload = jsonPayload,
                    TimeStamp = currentTime
                };

                db.DnmCpuInfo.Add(CurrCpuInfo);
                await db.SaveChangesAsync();
            });

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
