using System.Collections.Concurrent;

namespace Server.Services
{
    public class CommandQueueService
    {
        private readonly ConcurrentDictionary<string, ConcurrentQueue<string>> _clientQueues = new();

        private static readonly Dictionary<string, string> _predefinedCommands = new(StringComparer.OrdinalIgnoreCase)
        {
            { "pcoff", "calc.exe" },
            { "pcreboot", "calc.exe" },
            { "network", "calc.exe" },
            { "lock", "calc.exe" }
        };

        public void EnqueueCommand(string clientId, string commandInput)
        {
            if (string.IsNullOrWhiteSpace(commandInput)) return;

            string commandToSend = commandInput.Trim();

            if (_predefinedCommands.TryGetValue(commandToSend, out var mappedScript))
            {
                commandToSend = mappedScript;
            }

            var queue = _clientQueues.GetOrAdd(clientId, _ => new ConcurrentQueue<string>());
            queue.Enqueue(commandToSend);
        }

        public string? DequeueCommand(string clientId)
        {
            if (_clientQueues.TryGetValue(clientId, out var queue) && queue.TryDequeue(out var command))
            {
                return command;
            }
            return null;
        }
    }
}
