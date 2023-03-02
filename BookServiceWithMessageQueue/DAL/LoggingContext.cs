using Microsoft.EntityFrameworkCore;
using LoggingService.Models;

namespace LoggingService.DAL
{
    public class LoggingContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public LoggingContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Log>()
                .ToTable(nameof(Log));
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("Development"));
        }
    }
}
