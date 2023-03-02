using Microsoft.EntityFrameworkCore;
using RetailService.Models;

namespace RetailService.DAL
{
    public class RetailContext : DbContext
    {
        public RetailContext(DbContextOptions<RetailContext> options) : base(options)
        {

        }
        public DbSet<Retail> Retails { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Retail>()
                .ToTable(nameof(Retail));
        }
    }
}
