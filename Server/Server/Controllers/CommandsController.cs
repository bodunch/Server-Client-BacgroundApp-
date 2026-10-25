using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly CommandQueueService _commandQueue;

        public CommandsController(CommandQueueService commandQueue)
        {
            _commandQueue = commandQueue;
        }

        [HttpPost("send")]
        public IActionResult SendCommand([FromQuery] string clientId, [FromBody] CommandRequestDto request)
        {
            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(request?.CommandText))
            {
                return BadRequest("ClientId or CommandText is missing.");
            }

            _commandQueue.EnqueueCommand(clientId, request.CommandText);

            return Ok();
        }

        [HttpGet("get-pending")]
        public IActionResult GetPendingCommand()
        {
            if (!Request.Headers.TryGetValue("ClientId", out var headerValues) || string.IsNullOrEmpty(headerValues.FirstOrDefault()))
            {
                return BadRequest("ClientId header is missing.");
            }

            string clientId = headerValues.First()!;
            string? command = _commandQueue.DequeueCommand(clientId);

            if (string.IsNullOrEmpty(command))
            {
                return Ok();
            }

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
