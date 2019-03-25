using KutyApp.Client.Services.LocalRepository.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace KutyApp.Client.Services.LocalRepository.Entities
{
    public class PetDbContext : DbContext
    {
        private readonly string dbPath;

        //public DbSet<Dog> Dogs { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<MedicalTreatment> MedicalTreatments { get; set; }
        public DbSet<Habit> Habits { get; set; }

        public PetDbContext(string path) : base()
        {
            dbPath = path;

            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={dbPath}");
            //base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Pet>().HasKey(d => d.Id);
            builder.Entity<Pet>().HasMany(p => p.Habits).WithOne(h => h.Pet);
            builder.Entity<Pet>().HasMany(p => p.MedicalTreatments).WithOne(m => m.Pet);

            builder.Entity<Habit>().HasKey(h => h.Id);
            builder.Entity<Habit>().HasOne(h => h.Pet).WithMany(p => p.Habits);

            builder.Entity<MedicalTreatment>().HasKey(m => m.Id);
            builder.Entity<MedicalTreatment>().HasOne(m => m.Pet).WithMany(p => p.MedicalTreatments);
            //base.OnModelCreating(modelBuilder);

            //builder.Entity<Dog>()
            //    .HasData(new Dog { Id = 1, Name = "TestDog", BirthDate = new DateTime(2015,08,08), ChipNumber = "MA0165858003D", Color = "brown", Gender = Enums.Gender.Male, Weight = 13.2});
        }
    }
}
