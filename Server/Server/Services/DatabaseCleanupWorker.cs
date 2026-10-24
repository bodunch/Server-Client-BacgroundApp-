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

            var oldPortLogs = await context.DnmPortsInfo
                .Where(x => string.Compare(x.TimeStamp, thresholdDate) < 0)
                .ToListAsync();

            if (oldPortLogs.Any())
            {
                context.DnmPortsInfo.RemoveRange(oldPortLogs);
            }

            var oldConnectLogs = await context.DnmConnectionsInfo
                .Where(x => string.Compare(x.TimeStamp, thresholdDate) < 0)
                .ToListAsync();

            if (oldConnectLogs.Any())
            {
                context.DnmConnectionsInfo.RemoveRange(oldConnectLogs);
            }

            var oldAppLogs = await context.DnmAppInfo
                .Where(x => string.Compare(x.TimeStamp, thresholdDate) < 0)
                .ToListAsync();

            if (oldAppLogs.Any())
            {
                context.DnmAppInfo.RemoveRange(oldAppLogs);
            }

            var oldAdaptersLogs = await context.DnmAdaptersInfo
                .Where(x => string.Compare(x.TimeStamp, thresholdDate) < 0)
                .ToListAsync();

            if (oldAdaptersLogs.Any())
            {
                context.DnmAdaptersInfo.RemoveRange(oldAdaptersLogs);
            }

            int deletedCount = await context.SaveChangesAsync();
        }
    }
}
