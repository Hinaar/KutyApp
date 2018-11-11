using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoAPI.Geometries;
using KutyApp.Services.Environment.Bll.Entities;
using KutyApp.Services.Environment.Bll.Entities.Enums;
using KutyApp.Services.Environment.Bll.Entities.Model;
using KutyApp.Services.Environment.Bll.Interfaces;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;

namespace KutyApp.Services.Environment.Bll.Managers
{
    public class DatabaseManager : IDatabaseManager
    {
        private KutyAppServiceDbContext DbContext { get; }
        private ILocationManager LocationManager { get; }

        public DatabaseManager(KutyAppServiceDbContext dbContext, ILocationManager locationManager)
        {
            DbContext = dbContext;
            LocationManager = locationManager;
        }

        public async Task SeedDatabaseAsync()
        {
            // Create spatial factory - srid megadas nelku nem megy, csak azonos srid  -u adatok kozt kepes keresni stb.

            var rnd = new Random();
            const string chars = "0123456789";
            var poik = Enumerable.Range(1, 50).Select(x => new Poi
            {
                Name = $"Poi {x}",
                ImageUrl = "https://via.placeholder.com/300",
                Url = "https://via.placeholder.com/100",
                PhoneNumber = new string(Enumerable.Repeat(chars, 7).Select(s => s[rnd.Next(s.Length)]).ToArray()),
                OpeningTime = new TimeSpan(09, 30, 00),
                CloseTime = new TimeSpan(22, 00, 00),
                EnvironmentTypes = (EnvironmentType)(x % 6),
                Location = LocationManager.GeometryFactory.CreatePoint(new Coordinate(rnd.NextDouble() * 90.0, rnd.NextDouble() * 90.0))
            }).ToList();

            DbContext.Pois.AddRange(poik);

            await DbContext.SaveChangesAsync();
        }

        public async Task TruncateDatabaseAsync()
        {
            string query = string.Empty;
            List<string> tableNames = DbContext.Model.GetEntityTypes()
                .Select(t => t.Relational().TableName)
                .Where(n => n != "Logs")
                .Distinct()
                .ToList();
            List<string> manyToManyTables = new List<string> { };

            tableNames.ForEach(t => query += $"ALTER TABLE {t} nocheck constraint all;");
            tableNames.ForEach(t => query += $"DELETE FROM {t};"); //truncate csak drop constrainttal megy
            tableNames.Where(t => !manyToManyTables.Contains(t)).ToList().ForEach(t => query += $"DBCC CHECKIDENT ({t}, RESEED, 0);");
            tableNames.ForEach(t => query += $"ALTER TABLE {t} check constraint all;");

            await DbContext.Database.ExecuteSqlCommandAsync(query);
        }
    }
}
