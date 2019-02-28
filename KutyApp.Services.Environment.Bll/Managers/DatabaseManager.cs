using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GeoAPI.Geometries;
using KutyApp.Services.Environment.Bll.Dtos;
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
        private IMapper Mapper { get; }

        public DatabaseManager(KutyAppServiceDbContext dbContext, ILocationManager locationManager, IMapper mapper)
        {
            DbContext = dbContext;
            LocationManager = locationManager;
            Mapper = mapper;
        }

        public async Task SeedDatabaseAsync()
        {
            // Create spatial factory - srid megadas nelku nem megy, csak azonos srid  -u adatok kozt kepes keresni stb.
            // lat: szel-> +-90 , long: hosz -> +-180
            //koordinatanak : hosz, szel -be kell megadni
            //maps: szel, hosz ba jeleniti meg
            var rnd = new Random();
            const string chars = "0123456789";

            var poik = new List<Poi>
            {
                new Poi
                {
                    Name = "Vérhalom u. 34",
                    EnvironmentTypes = EnvironmentType.Cosmetics,
                    Location = LocationManager.GeometryFactory.CreatePoint(new Coordinate(19.022939, 47.520469)),
                    ImageUrl = "https://via.placeholder.com/300",
                    Url = "https://via.placeholder.com/100",
                    PhoneNumber = new string(Enumerable.Repeat(chars, 7).Select(s => s[rnd.Next(s.Length)]).ToArray()),
                },
                new Poi
                {
                    Name = "Damjanich u. 22",
                    EnvironmentTypes = EnvironmentType.HealthCare,
                    Location = LocationManager.GeometryFactory.CreatePoint(new Coordinate(19.076712, 47.506991)),
                    ImageUrl = "https://via.placeholder.com/300",
                    Url = "https://via.placeholder.com/100",
                    PhoneNumber = new string(Enumerable.Repeat(chars, 7).Select(s => s[rnd.Next(s.Length)]).ToArray()),
                },
                new Poi
                {
                    Name = "Kovács u. 1",
                    EnvironmentTypes = EnvironmentType.HealthCare,
                    Location = LocationManager.GeometryFactory.CreatePoint(new Coordinate(19.804090, 47.171543)),
                    ImageUrl = "https://via.placeholder.com/300",
                    Url = "https://via.placeholder.com/100",
                    PhoneNumber = new string(Enumerable.Repeat(chars, 7).Select(s => s[rnd.Next(s.Length)]).ToArray()),
                },
                new Poi
                {
                    Name = "Lehon 24",
                    EnvironmentTypes = EnvironmentType.HealthCare,
                    Location = LocationManager.GeometryFactory.CreatePoint(new Coordinate(4.371546, 50.864859)),
                    ImageUrl = "https://via.placeholder.com/300",
                    Url = "https://via.placeholder.com/100",
                    PhoneNumber = new string(Enumerable.Repeat(chars, 7).Select(s => s[rnd.Next(s.Length)]).ToArray()),
                }
            };

            //var poik = Enumerable.Range(1, 50).Select(x => new Poi
            //{
            //    Name = $"Poi {x}",
            //    ImageUrl = "https://via.placeholder.com/300",
            //    Url = "https://via.placeholder.com/100",
            //    PhoneNumber = new string(Enumerable.Repeat(chars, 7).Select(s => s[rnd.Next(s.Length)]).ToArray()),
            //    OpeningTime = new TimeSpan(09, 30, 00),
            //    CloseTime = new TimeSpan(22, 00, 00),
            //    EnvironmentTypes = (EnvironmentType)(x % 6),
            //    Location = LocationManager.GeometryFactory.CreatePoint(new Coordinate(rnd.NextDouble() * 90.0, rnd.NextDouble() * 90.0))
            //}).ToList();

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
            List<string> manyToManyTables = new List<string> { "PetSittings", "UserFriendships", "UserPois", "AspNetUserRoles" };

            tableNames.ForEach(t => query += $"ALTER TABLE {t} nocheck constraint all;");
            tableNames.ForEach(t => query += $"DELETE FROM {t};"); //truncate csak drop constrainttal megy
            tableNames.Where(t => !manyToManyTables.Contains(t)).ToList().ForEach(t => query += $"DBCC CHECKIDENT ({t}, RESEED, 0);");
            tableNames.ForEach(t => query += $"ALTER TABLE {t} check constraint all;");

            await DbContext.Database.ExecuteSqlCommandAsync(query);
        }

        public async Task<DbVersionDto> GetDBVersionAsync()
        {
            var version = await DbContext.DbVerisons.OrderByDescending(d => d.Date).FirstOrDefaultAsync();
            return Mapper.Map<DbVersionDto>(version);
        }
    }
}
