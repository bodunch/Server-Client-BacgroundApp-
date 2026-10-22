using Server.Data;
using Microsoft.EntityFrameworkCore;

namespace Server.Services
{
    public class DatabaseCleanupWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public DatabaseCleanupWorker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CleanupOldDataAsync();
                }
                catch (Exception ex)
                {
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }

        private async Task CleanupOldDataAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            string thresholdDate = DateTime.Now.AddMinutes(-5).ToString("yyyy-MM-dd HH:mm:ss");

            var oldCpuLogs = await context.DnmCpuInfo
                .Where(x => string.Compare(x.TimeStamp, thresholdDate) < 0)
                .ToListAsync();

            if (oldCpuLogs.Any())
            {
                context.DnmCpuInfo.RemoveRange(oldCpuLogs);
            }

            var oldRamLogs = await context.DnmRamInfo
                .Where(x => string.Compare(x.TimeStamp, thresholdDate) < 0)
                .ToListAsync();

            if (oldRamLogs.Any())
            {
                context.DnmRamInfo.RemoveRange(oldRamLogs);
            }

            var oldProcLogs = await context.DnmProcessesInfo
                .Where(x => string.Compare(x.TimeStamp, thresholdDate) < 0)
                .ToListAsync();

            if (oldProcLogs.Any())
            {
                context.DnmProcessesInfo.RemoveRange(oldProcLogs);
            }

            int deletedCount = await context.SaveChangesAsync();
        }
    }
}
