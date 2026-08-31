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

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
