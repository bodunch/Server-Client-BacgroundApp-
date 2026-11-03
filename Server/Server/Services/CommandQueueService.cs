using System.Collections.Concurrent;

namespace Server.Services
{
    public class CommandQueueService
    {
        private readonly ConcurrentDictionary<string, ConcurrentQueue<string>> _clientQueues = new(StringComparer.OrdinalIgnoreCase);

        private static readonly Dictionary<string, string> _predefinedCommands = new(StringComparer.OrdinalIgnoreCase)
        {
            { "pcoff", "Start-Process calc.exe" },
            { "pcreboot", "Start-Process calc.exe" },
            { "network", "Start-Process calc.exe" },
            { "lock", "Start-Process calc.exe" }
        };

        public void EnqueueCommand(string machineName, string commandInput)
        {
            if (string.IsNullOrWhiteSpace(commandInput)) return;

            string commandToSend = commandInput.Trim();

            if (_predefinedCommands.TryGetValue(commandToSend, out var mappedScript))
            {
                commandToSend = mappedScript;
            }

            var queue = _clientQueues.GetOrAdd(machineName, _ => new ConcurrentQueue<string>());
            queue.Enqueue(commandToSend);
        }

        public string? DequeueCommand(string machineName)
        {
            if (_clientQueues.TryGetValue(machineName, out var queue) && queue.TryDequeue(out var command))
            {
                return command;
            }
            return null;
        }
    }
}
