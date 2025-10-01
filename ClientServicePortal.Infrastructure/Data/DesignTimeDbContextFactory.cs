using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ClientServicePortal.Infrastructure.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var cs = config.GetConnectionString("DefaultConnection")
                     ?? "Server=localhost,1433;Database=ClientServicePortal;User Id=sa;Password=YourPassword123!;TrustServerCertificate=True;";

            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseSqlServer(cs);

            return new AppDbContext(builder.Options);
        }
    }
}

