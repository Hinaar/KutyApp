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

        public DbSet<Dog> Dogs { get; set; }

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
            builder.Entity<Dog>().HasKey(d => d.Id);
            builder.Entity<Dog>().Property(d => d.Name).IsRequired();
            builder.Entity<Dog>().Property(d => d.Gender).IsRequired();
            //base.OnModelCreating(modelBuilder);

            builder.Entity<Dog>()
                .HasData(new Dog { Id = 1, Name = "TestDog", BirthDate = new DateTime(2015,08,08), ChipNumber = "MA0165858003D", Color = "brown", Gender = Enums.Gender.Male, Weight = 13.2});
        }
    }
}
