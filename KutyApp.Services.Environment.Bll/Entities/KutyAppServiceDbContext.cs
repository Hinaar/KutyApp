using KutyApp.Services.Environment.Bll.Entities.Configurations;
using KutyApp.Services.Environment.Bll.Entities.Model;
using Microsoft.EntityFrameworkCore;

namespace KutyApp.Services.Environment.Bll.Entities
{
    public class KutyAppServiceDbContext : DbContext
    {
        public DbSet<Poi> Pois { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PoiConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public KutyAppServiceDbContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
