using Microsoft.EntityFrameworkCore;
using Server.Data.Entities;

namespace Server.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<ClientsEntity> Clients { get; set; }
        public DbSet<SystemInfoEntity> SystemInfo { get; set; }
        public DbSet<ComputerInfoEntity> ComputerInfo { get; set; }
        public DbSet<CpuInfoEntity> CpuInfo { get; set; }
        public DbSet<RamInfoEntity> RamInfo { get; set; }
        public DbSet<DynamicCpuInfoEntity> DnmCpuInfo { get; set; }
        public DbSet<DynamicRamInfoEntity> DnmRamInfo { get; set; }
        public DbSet<DynamicProcessesInfoEntity> DnmProcessesInfo { get; set; }
        public DbSet<DynamicAdaptersInfoEntity> DnmAdaptersInfo { get; set; }
        public DbSet<DynamicConnectionsInfoEntity> DnmConnectionsInfo { get; set; }
        public DbSet<DynamicPortsInfoEntity> DnmPortsInfo { get; set; }
        public DbSet<DynamicApplicationsInfoEntity> DnmAppInfo { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
