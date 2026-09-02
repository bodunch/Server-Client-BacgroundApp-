using System.Threading.Channels;

namespace Server.Data.DbQueue
{
    public class DatabaseQueueService
    {
        private readonly Channel<Func<AppDbContext, Task>> _channel =
            Channel.CreateUnbounded<Func<AppDbContext, Task>>(new UnboundedChannelOptions
            {
                SingleWriter = false,
                SingleReader = true
            });

        public void QueueWorkItem(Func<AppDbContext, Task> workItem)
        {
            if (workItem == null) throw new ArgumentNullException(nameof(workItem));
            _channel.Writer.TryWrite(workItem);
        }

        public IAsyncEnumerable<Func<AppDbContext, Task>> ReadAllAsync(CancellationToken cancellationToken) =>
            _channel.Reader.ReadAllAsync(cancellationToken);
    }
}
