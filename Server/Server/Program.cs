using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Data.DbQueue;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllers();
            builder.Services.AddSingleton<DatabaseQueueService>();
            builder.Services.AddHostedService<DatabaseQueueWorker>();

            var app = builder.Build();

            //using controllers and their paths 
            app.MapControllers();

            app.Run("http://localhost:5000");
        }
    }
}
