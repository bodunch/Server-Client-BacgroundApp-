using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Data.DbQueue;
using Server.Data.Entities;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetClientsInfo()
        {
            var clients = await _context.Clients.ToListAsync();

            return Ok(clients);
        }

        [HttpGet("staticinfo/system")]
        public async Task<IActionResult> GetStaticSystemInfo()
        {
            if (!Request.Headers.TryGetValue("ClientId", out var headerValues) || !int.TryParse(headerValues.FirstOrDefault(), out int clientId))
            {
                return BadRequest("ClientId header is missing or invalid.");
            }

            var systemInfo = _context.SystemInfo.FirstOrDefault(s => s.ClientId == clientId);

            if (systemInfo == null)
            {
                return NotFound("System info for this client was not found.");
            }

            return Ok(systemInfo);
        }

        [HttpGet("staticinfo/computer")]
        public async Task<IActionResult> GetStaticComputerInfo()
        {
            if (!Request.Headers.TryGetValue("ClientId", out var headerValues) || !int.TryParse(headerValues.FirstOrDefault(), out int clientId))
            {
                return BadRequest("ClientId header is missing or invalid.");
            }

            var computerInfo = _context.ComputerInfo.FirstOrDefault(s => s.ClientId == clientId);

            if (computerInfo == null)
            {
                return NotFound("Computer info for this client was not found.");
            }

            return Ok(computerInfo);
        }

        [HttpGet("staticinfo/cpu")]
        public async Task<IActionResult> GetStaticCpuInfo()
        {
            if (!Request.Headers.TryGetValue("ClientId", out var headerValues) || !int.TryParse(headerValues.FirstOrDefault(), out int clientId))
            {
                return BadRequest("ClientId header is missing or invalid.");
            }

            var cpuInfo = _context.CpuInfo.FirstOrDefault(s => s.ClientId == clientId);

            if (cpuInfo == null)
            {
                return NotFound("Cpu info for this client was not found.");
            }

            return Ok(cpuInfo);
        }

        [HttpGet("staticinfo/ram")]
        public async Task<IActionResult> GetStaticRamInfo()
        {
            if (!Request.Headers.TryGetValue("ClientId", out var headerValues) || !int.TryParse(headerValues.FirstOrDefault(), out int clientId))
            {
                return BadRequest("ClientId header is missing or invalid.");
            }

            var ramInfo = _context.RamInfo.FirstOrDefault(s => s.ClientId == clientId);

            if (ramInfo == null)
            {
                return NotFound("Ram info for this client was not found.");
            }

            return Ok(ramInfo);
        }

        [HttpGet("dynamicinfo/cpuinfo")]
        public async Task<IActionResult> GetDynamicCpuInfo()
        {
            if (!Request.Headers.TryGetValue("ClientId", out var headerValues) || !int.TryParse(headerValues.FirstOrDefault(), out int clientId))
            {
                return BadRequest("ClientId header is missing or invalid.");
            }

            var cpuInfo = await _context.DnmCpuInfo
                .Where(s => s.ClientId == clientId)
                .OrderByDescending(s => s.Id)
                .FirstOrDefaultAsync();

            if (cpuInfo == null)
            {
                return NotFound("Cpu Info not found");
            }

            return Ok(cpuInfo);
        }

        [HttpGet("dynamicinfo/raminfo")]
        public async Task<IActionResult> GetDynamicRamInfo()
        {
            if (!Request.Headers.TryGetValue("ClientId", out var headerValues) || !int.TryParse(headerValues.FirstOrDefault(), out int clientId))
            {
                return BadRequest("ClientId header is missing or invalid.");
            }

            var ramInfo = await _context.DnmRamInfo
                .Where(s => s.ClientId == clientId)
                .OrderByDescending(s => s.Id)
                .FirstOrDefaultAsync();

            if (ramInfo == null)
            {
                return NotFound("Ram Info not found");
            }

            return Ok(ramInfo);
        }

        [HttpGet("dynamicinfo/procinfo")]
        public async Task<IActionResult> GetDynamicProcessInfo()
        {
            if (!Request.Headers.TryGetValue("ClientId", out var headerValues) || !int.TryParse(headerValues.FirstOrDefault(), out int clientId))
            {
                return BadRequest("ClientId header is missing or invalid.");
            }

            var procInfo = await _context.DnmProcessesInfo
                .Where(s => s.ClientId == clientId)
                .OrderByDescending(s => s.Id)
                .FirstOrDefaultAsync();

            if (procInfo == null)
            {
                return NotFound("Pocesses Info not found");
            }

            return Ok(procInfo);
        }

        [HttpGet("dynamicinfo/portsinfo")]
        public async Task<IActionResult> GetDynamicPortsInfo()
        {
            if (!Request.Headers.TryGetValue("ClientId", out var headerValues) || !int.TryParse(headerValues.FirstOrDefault(), out int clientId))
            {
                return BadRequest("ClientId header is missing or invalid.");
            }

            var procInfo = await _context.DnmPortsInfo
                .Where(s => s.ClientId == clientId)
                .OrderByDescending(s => s.Id)
                .FirstOrDefaultAsync();

            if (procInfo == null)
            {
                return NotFound("Ports Info not found");
            }

            return Ok(procInfo);
        }

        [HttpGet("dynamicinfo/connectinfo")]
        public async Task<IActionResult> GetDynamicConnectionsInfo()
        {
            if (!Request.Headers.TryGetValue("ClientId", out var headerValues) || !int.TryParse(headerValues.FirstOrDefault(), out int clientId))
            {
                return BadRequest("ClientId header is missing or invalid.");
            }

            var procInfo = await _context.DnmConnectionsInfo
                .Where(s => s.ClientId == clientId)
                .OrderByDescending(s => s.Id)
                .FirstOrDefaultAsync();

            if (procInfo == null)
            {
                return NotFound("Connections Info not found");
            }

            return Ok(procInfo);
        }

        [HttpGet("dynamicinfo/appinfo")]
        public async Task<IActionResult> GetDynamicAppInfo()
        {
            if (!Request.Headers.TryGetValue("ClientId", out var headerValues) || !int.TryParse(headerValues.FirstOrDefault(), out int clientId))
            {
                return BadRequest("ClientId header is missing or invalid.");
            }

            var procInfo = await _context.DnmAppInfo
                .Where(s => s.ClientId == clientId)
                .OrderByDescending(s => s.Id)
                .FirstOrDefaultAsync();

            if (procInfo == null)
            {
                return NotFound("Applications Info not found");
            }

            return Ok(procInfo);
        }

        [HttpGet("dynamicinfo/adaptersinfo")]
        public async Task<IActionResult> GetDynamicAdaptersInfo()
        {
            if (!Request.Headers.TryGetValue("ClientId", out var headerValues) || !int.TryParse(headerValues.FirstOrDefault(), out int clientId))
            {
                return BadRequest("ClientId header is missing or invalid.");
            }

            var procInfo = await _context.DnmAdaptersInfo
                .Where(s => s.ClientId == clientId)
                .OrderByDescending(s => s.Id)
                .FirstOrDefaultAsync();

            if (procInfo == null)
            {
                return NotFound("Adapters Info not found");
            }

            return Ok(procInfo);
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class SystemController : ControllerBase
    {
        private readonly DatabaseQueueService _dbQueue;

        public SystemController(DatabaseQueueService dbQueue, AppDbContext context)
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

                var staticInfoLine = db.SystemInfo.FirstOrDefault(s => s.ClientId == clientId);

                if (staticInfoLine == null)
                {
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

                }
                else
                {
                    staticInfoLine.OperatingSystem = operatingSystem;
                    staticInfoLine.Version = version;
                    staticInfoLine.ComputerName = computerName;
                    staticInfoLine.RegisteredUser = registeredUser;
                    staticInfoLine.LastBootTime = lastBootTime;
                }

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

            string machineName = info["machineName"]?.ToString() ?? info["MachineName"]?.ToString() ?? info["ComputerName"]?.ToString() ?? info["pcName"]?.ToString();

            string manufacturer = info["Manufacturer"]?.ToString() ?? "";
            string pcModel = info["PCModel"]?.ToString() ?? "";
            string systemType = info["SystemType"]?.ToString() ?? "";
            string countOfCpu = info["CountOfCPU"]?.ToString() ?? "";
            string systemStart = info["SystemStart"]?.ToString() ?? "";
            string statusOfStart = info["StatusOfStart"]?.ToString() ?? "";

            _dbQueue.QueueWorkItem(async db =>
            {
                int clientId = ClientHelper.GetOrAddClient(db, machineName);

                var staticInfoLine = db.ComputerInfo.FirstOrDefault(s => s.ClientId == clientId);

                if (staticInfoLine == null)
                {
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
                }
                else
                {
                    staticInfoLine.Manufacturer = manufacturer;
                    staticInfoLine.PCModel = pcModel;
                    staticInfoLine.SystemType = systemType;
                    staticInfoLine.CountOfCpu = countOfCpu;
                    staticInfoLine.SystemStart = systemStart;
                    staticInfoLine.StatusOfStart = statusOfStart;
                }

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

            string machineName = info["machineName"]?.ToString() ?? info["MachineName"]?.ToString() ?? info["ComputerName"]?.ToString() ?? info["pcName"]?.ToString();

            string cpuName = info["CPUName"]?.ToString() ?? "";
            string manufacturer = info["Manufacturer"]?.ToString() ?? "";
            string munOfCores = info["NumOfCores"]?.ToString() ?? "";
            string numOfStreams = info["NumOfStreams"]?.ToString() ?? "";

            _dbQueue.QueueWorkItem(async db =>
            {
                int clientId = ClientHelper.GetOrAddClient(db, machineName);

                var staticInfoLine = db.CpuInfo.FirstOrDefault(s => s.ClientId == clientId);

                if (staticInfoLine == null)
                {
                    var cpuInfo = new CpuInfoEntity
                    {
                        ClientId = clientId,
                        CPUName = cpuName,
                        Manufacturer = manufacturer,
                        NumOfCores = munOfCores,
                        NumOfStreams = numOfStreams,
                    };

                    db.CpuInfo.Add(cpuInfo);
                }
                else
                {
                    staticInfoLine.CPUName = cpuName;
                    staticInfoLine.Manufacturer = manufacturer;
                    staticInfoLine.NumOfCores = munOfCores;
                    staticInfoLine.NumOfStreams = numOfStreams;
                }

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

            string machineName = info["machineName"]?.ToString() ?? info["MachineName"]?.ToString() ?? info["ComputerName"]?.ToString() ?? info["pcName"]?.ToString();

            string type = info["Type"]?.ToString() ?? "";
            string partNumber = info["PartNumber"]?.ToString() ?? "";
            string frequency = info["Frequency"]?.ToString() ?? "";
            string memoryCount = info["MemoryCount"]?.ToString() ?? "";

            _dbQueue.QueueWorkItem(async db =>
            {
                int clientId = ClientHelper.GetOrAddClient(db, machineName);

                var staticInfoLine = db.RamInfo.FirstOrDefault(s => s.ClientId == clientId);

                if (staticInfoLine == null)
                {
                    var ramInfo = new RamInfoEntity
                    {
                        ClientId = clientId,
                        Type = type,
                        PartNumber = partNumber,
                        Frequency = frequency,
                        MemoryCount = memoryCount
                    };

                    db.RamInfo.Add(ramInfo);
                    
                }
                else
                {
                    staticInfoLine.Type = type;
                    staticInfoLine.PartNumber = partNumber;
                    staticInfoLine.Frequency = frequency;
                    staticInfoLine.MemoryCount = memoryCount;
                }
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

            string machineName = info["machineName"]?.ToString() ?? info["MachineName"]?.ToString() ?? info["ComputerName"]?.ToString() ?? info["pcName"]?.ToString();

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
        private readonly DatabaseQueueService _dbQueue;

        public CurrramController(DatabaseQueueService dbQueue)
        {
            _dbQueue = dbQueue;
        }

        [HttpPost]
        public IActionResult RevievCurrRAMInfo([FromBody] JsonObject info)
        {
            if (info == null)
                return BadRequest();

            string machineName = info["machineName"]?.ToString() ?? info["MachineName"]?.ToString() ?? info["ComputerName"]?.ToString() ?? info["pcName"]?.ToString();

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

                var CurrRamInfo = new DynamicRamInfoEntity
                {
                    ClientId = clientId,
                    JsonPayload = jsonPayload,
                    TimeStamp = currentTime
                };

                db.DnmRamInfo.Add(CurrRamInfo);
                await db.SaveChangesAsync();
            });

            return Ok();
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ProcessesController : ControllerBase
    {
        private readonly DatabaseQueueService _dbQueue;

        public ProcessesController(DatabaseQueueService dbQueue)
        {
            _dbQueue = dbQueue;
        }

        [HttpPost]
        public IActionResult RevievProcesses([FromBody] JsonObject info)
        {
            if (info == null)
                return BadRequest();

            string machineName = info["machineName"]?.ToString() ?? info["MachineName"]?.ToString() ?? info["ComputerName"]?.ToString() ?? info["pcName"]?.ToString();

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

                var CurrProcessesInfo = new DynamicProcessesInfoEntity
                {
                    ClientId = clientId,
                    JsonPayload = jsonPayload,
                    TimeStamp = currentTime
                };

                db.DnmProcessesInfo.Add(CurrProcessesInfo);
                await db.SaveChangesAsync();
            });

            return Ok();
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class AdaptersController : ControllerBase
    {
        private readonly DatabaseQueueService _dbQueue;

        public AdaptersController(DatabaseQueueService dbQueue)
        {
            _dbQueue = dbQueue;
        }

        [HttpPost]
        public IActionResult RevievAdapters([FromBody] JsonObject info)
        {
            if (info == null)
                return BadRequest();

            string machineName = info["machineName"]?.ToString() ?? info["MachineName"]?.ToString() ?? info["ComputerName"]?.ToString() ?? info["pcName"]?.ToString();

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

                var CurrAdaptersInfo = new DynamicAdaptersInfoEntity
                {
                    ClientId = clientId,
                    JsonPayload = jsonPayload,
                    TimeStamp = currentTime
                };

                db.DnmAdaptersInfo.Add(CurrAdaptersInfo);
                await db.SaveChangesAsync();
            });

            return Ok();
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ConnectionsController : ControllerBase
    {
        private readonly DatabaseQueueService _dbQueue;

        public ConnectionsController(DatabaseQueueService dbQueue)
        {
            _dbQueue = dbQueue;
        }

        [HttpPost]
        public IActionResult RevievConnections([FromBody] JsonObject info)
        {
            if (info == null)
                return BadRequest();

            string machineName = info["machineName"]?.ToString() ?? info["MachineName"]?.ToString() ?? info["ComputerName"]?.ToString() ?? info["pcName"]?.ToString();

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

                var CurrConnectionsInfo = new DynamicConnectionsInfoEntity
                {
                    ClientId = clientId,
                    JsonPayload = jsonPayload,
                    TimeStamp = currentTime
                };

                db.DnmConnectionsInfo.Add(CurrConnectionsInfo);
                await db.SaveChangesAsync();
            });

            return Ok();
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class PortsController : ControllerBase
    {
        private readonly DatabaseQueueService _dbQueue;

        public PortsController(DatabaseQueueService dbQueue)
        {
            _dbQueue = dbQueue;
        }

        [HttpPost]
        public IActionResult RevievPorts([FromBody] JsonObject info)
        {
            if (info == null)
                return BadRequest();

            string machineName = info["machineName"]?.ToString() ?? info["MachineName"]?.ToString() ?? info["ComputerName"]?.ToString() ?? info["pcName"]?.ToString();

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

                var CurrPortsInfo = new DynamicPortsInfoEntity
                {
                    ClientId = clientId,
                    JsonPayload = jsonPayload,
                    TimeStamp = currentTime
                };

                db.DnmPortsInfo.Add(CurrPortsInfo);
                await db.SaveChangesAsync();
            });

            return Ok();
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class AppController : ControllerBase
    {
        private readonly DatabaseQueueService _dbQueue;

        public AppController(DatabaseQueueService dbQueue)
        {
            _dbQueue = dbQueue;
        }

        [HttpPost]
        public IActionResult RevievApplications([FromBody] JsonObject info)
        {
            if (info == null)
                return BadRequest();

            string machineName = info["machineName"]?.ToString() ?? info["MachineName"]?.ToString() ?? info["ComputerName"]?.ToString() ?? info["pcName"]?.ToString();

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

                var CurrAppInfo = new DynamicApplicationsInfoEntity
                {
                    ClientId = clientId,
                    JsonPayload = jsonPayload,
                    TimeStamp = currentTime
                };

                db.DnmAppInfo.Add(CurrAppInfo);
                await db.SaveChangesAsync();
            });

            return Ok();
        }
    }
}
