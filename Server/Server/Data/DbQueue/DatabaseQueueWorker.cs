using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Server.Data.DbQueue;

namespace Server.Data.DbQueue
{
    public class DatabaseQueueWorker : BackgroundService
    {
        private readonly DatabaseQueueService _taskQueue;
        private readonly IServiceScopeFactory _scopeFactory;

        public DatabaseQueueWorker(DatabaseQueueService taskQueue, IServiceScopeFactory scopeFactory)
        {
            _taskQueue = taskQueue;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var workItem in _taskQueue.ReadAllAsync(stoppingToken))
            {
                using var scope = _scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                try
                {
                    await workItem(dbContext);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка при фоновому збереженні в БД: {ex.Message}");
                }
            }
        }
    }
}
