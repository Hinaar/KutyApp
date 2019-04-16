using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GeoAPI.Geometries;
using Geolocation;
using KutyApp.Services.Environment.Bll.Dtos;
using KutyApp.Services.Environment.Bll.Entities;
using KutyApp.Services.Environment.Bll.Entities.Model;
using KutyApp.Services.Environment.Bll.Interfaces;
using KutyApp.Services.Environment.Bll.Resources;
using Microsoft.EntityFrameworkCore;

namespace KutyApp.Services.Environment.Bll.Managers
{
    public class PoiManager : IPoiManager
    {
        private KutyAppServiceDbContext DbContext { get; }
        private IMapper Mapper { get; }
        private ILocationManager LocationManager { get; }

        public PoiManager(KutyAppServiceDbContext dbContext, IMapper mapper, ILocationManager locationManager)
        {
            DbContext = dbContext;
            Mapper = mapper;
            LocationManager = locationManager;
        }

        public async Task<PoiDto> AddOrEditPoiAsync(AddOrEditPoiDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException();

            if (string.IsNullOrEmpty(dto.Name))
                throw new Exception(ExceptionMessages.Required);

            //TODO: visszaadni poit
            if (dto.Id == null)
                return await AddPoiAsync(dto);
            else
                return await EditPoiAsync(dto);
        }

        private async Task<PoiDto> EditPoiAsync(AddOrEditPoiDto dto)
        {
            Poi poi = await DbContext.Pois.FirstOrDefaultAsync(p => p.Id == dto.Id);
            if (poi == null)
                throw new Exception(ExceptionMessages.NotFound);

            Poi editedPoi = Mapper.Map<Poi>(dto);

            //TODO: propertynkent vizsgani aztan elmenteni

            if (poi.Name != editedPoi.Name)
                poi.Name = editedPoi.Name;

            if (poi.Url != editedPoi.Url)
                poi.Url = editedPoi.Url;

            if (poi.PhoneNumber != editedPoi.PhoneNumber)
                poi.PhoneNumber = editedPoi.PhoneNumber;

            if (poi.OpeningTime != editedPoi.OpeningTime)
                poi.OpeningTime = editedPoi.OpeningTime;

            if (poi.CloseTime != editedPoi.CloseTime)
                poi.CloseTime = editedPoi.CloseTime;

            if (!poi.Location.EqualsTopologically(editedPoi.Location))
                poi.Location = editedPoi.Location;

            await DbContext.SaveChangesAsync();

            return Mapper.Map<PoiDto>(poi);
        }

        private async Task<PoiDto> AddPoiAsync(AddOrEditPoiDto dto)
        {
            Poi poi = Mapper.Map<Poi>(dto);

            DbContext.Pois.Add(poi);

            await DbContext.SaveChangesAsync();

            return Mapper.Map<PoiDto>(poi);
        }

        public async Task DeletePoiAsync(int id)
        {
            Poi poiToDelete = await DbContext.Pois.FirstOrDefaultAsync(p => p.Id == id);

            if (poiToDelete == null)
                throw new Exception(ExceptionMessages.NotFound);

            DbContext.Pois.Remove(poiToDelete);

            await DbContext.SaveChangesAsync();
        }

        public async Task<List<PoiDto>> ListPoisAsync()
        {
            return Mapper.Map<List<PoiDto>>(await DbContext.Pois.ToListAsync());
        }

        public async Task<List<PoiDto>> ListClosestPoisAsync(SearchPoiDto search)
        {
            if (search == null)
                throw new ArgumentNullException(nameof(search));

            IQueryable<Poi> query = DbContext.Pois;

            //TODO: null-t nem engedni
            IPoint location = null;

            //order by distance 
            if (search.Longitude.HasValue && search.Latitude.HasValue)
            {
                location = LocationManager.GeometryFactory.CreatePoint(new GeoAPI.Geometries.Coordinate(search.Longitude.Value, search.Latitude.Value));
                //TODO: valszeg ez nem marad szimplan query
                query = query.OrderBy(p => p.Location.Distance(location));
            }

            var result = await query.Select(x => new { Poi = x, Distance = location != null ? x.Location.Distance(location) : 0 }).ToListAsync();

            var pois = Mapper.Map<List<PoiDto>>(result.Select(r => r.Poi).ToList());
            pois.ForEach(t => t.Distance = GeoCalculator.GetDistance(location.Y, location.X, t.Latitude, t.Longitude, 1, DistanceUnit.Kilometers));

            return pois;
        }
    }
}
