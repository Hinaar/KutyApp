using KutyApp.Services.Environment.Bll.Entities.Configurations;
using KutyApp.Services.Environment.Bll.Entities.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KutyApp.Services.Environment.Bll.Entities
{
    public class KutyAppServiceDbContext : IdentityDbContext<User>
    {
        public DbSet<Advert> Adverts { get; set; }
        public DbSet<DbVersion> DbVerisons { get; set; }
        public DbSet<Habit> Habits { get; set; }
        public DbSet<MedicalTreatment> MedicalTreatments { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Poi> Pois { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<PetSitting> PetSittings { get; set; }
        public DbSet<UserFriendship> UserFriendships { get; set; }
        public DbSet<UserPoi> UserPois { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new AdvertConfiguration());
            modelBuilder.ApplyConfiguration(new DbVersionConfiguration());
            modelBuilder.ApplyConfiguration(new HabitConfiguration());
            modelBuilder.ApplyConfiguration(new MedicalTreatmentConfiguration());
            modelBuilder.ApplyConfiguration(new PetConfiguration());
            modelBuilder.ApplyConfiguration(new PetSittingsConfiguration());
            modelBuilder.ApplyConfiguration(new PoiConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserFriendshipConfiguration());
            modelBuilder.ApplyConfiguration(new UserPoiConfiguration());
        }

        public KutyAppServiceDbContext(DbContextOptions<KutyAppServiceDbContext> options) : base(options) { }

        public KutyAppServiceDbContext() : base()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
