using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.Services;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly CommandQueueService _commandQueue;
        private readonly ILogger<CommandsController> _logger;
        public CommandsController(CommandQueueService commandQueue, ILogger<CommandsController> logger)
        {
            _commandQueue = commandQueue;
            _logger = logger;
        }

        [HttpPost("send")]
        public IActionResult SendCommand([FromQuery] string? machineName, [FromBody] CommandRequestDto request)
        {
            if (string.IsNullOrEmpty(machineName))
            {
                machineName = Environment.MachineName;
            }

            if (string.IsNullOrEmpty(request?.CommandText))
            {
                _logger.LogWarning("Спроба відправити порожню команду.");
                return BadRequest("CommandText is missing.");
            }

            _logger.LogInformation($"[Admin -> Server] Додано команду для ПК '{machineName}': {request.CommandText}");
            _commandQueue.EnqueueCommand(machineName, request.CommandText);

            return Ok();
        }

        [HttpGet("get-pending")]
        public IActionResult GetPendingCommand()
        {
            if (!Request.Headers.TryGetValue("ClientId", out var headerValues) || string.IsNullOrEmpty(headerValues.FirstOrDefault()))
            {
                _logger.LogInformation($"[Server -> Client] Віддано команду для ПК");
                return BadRequest("ClientId header is missing.");
            }

            string clientId = headerValues.First()!;
            string? command = _commandQueue.DequeueCommand(clientId);

            if (string.IsNullOrEmpty(command))
            {
                return Ok();
            }
            _logger.LogInformation($"[Server -> Client] Віддано команду для ПК '{clientId}': {command}");
            return Ok(new
            {
                CommandText = command
            });
        }
    }

    public class CommandRequestDto
    {
        public string CommandText { get; set; } = string.Empty;
    }
}
