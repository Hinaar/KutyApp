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
using Microsoft.EntityFrameworkCore;

namespace KutyApp.Services.Environment.Bll.Managers
{
    public class AdvertManager : IAdvertManager
    {
        private KutyAppServiceDbContext DbContext { get; }
        private IMapper Mapper { get; }
        private IAuthManager AuthManager { get; }
        private IKutyAppContext KutyAppContext { get; }

        public AdvertManager(KutyAppServiceDbContext dbContext, IMapper mapper, IAuthManager authManager, IKutyAppContext kutyAppContext)
        {
            DbContext = dbContext;
            Mapper = mapper;
            AuthManager = authManager;
            KutyAppContext = kutyAppContext;
        }

        public async Task<List<AdvertDto>> ListLatestAdvertisementsAsync(SearchAdvertDto dto)
        {
            var query = DbContext.Adverts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(dto.KeyWord))
            {
                var words = dto.KeyWord.Split(' ');
                query = query.Where(a => words.Any(w => a.Title.Contains(w) || a.Description.Contains(w))); 
            }

            var adverts = await query.Include(a => a.Advertiser).OrderByDescending(a => a.CreateDate).ToListAsync();

            if (!string.IsNullOrWhiteSpace(dto.KeyWord))
                adverts = adverts.OrderByDescending(a => dto.KeyWord.Split(' ').Count(w => a.Title.Contains(w) || a.Description.Contains(w))).ThenByDescending(a => a.CreateDate).ToList();

            var mappedAdverts = Mapper.Map<List<AdvertDto>>(adverts);

            return mappedAdverts;
        }

        public async Task<List<AdvertDto>> ListMyLatestAdvertisementsAsync(SearchAdvertDto dto)
        {
            var query = DbContext.Adverts.Where(a => a.AdvertiserId == KutyAppContext.CurrentUser.Id);

            if (!string.IsNullOrWhiteSpace(dto.KeyWord))
            {
                var words = dto.KeyWord.Split(' ');
                query = query.Where(a => words.Any(w => a.Title.Contains(w) || a.Description.Contains(w)));
            }

            var adverts = await query.Include(a => a.Advertiser).OrderByDescending(a => a.CreateDate).ToListAsync();

            if (!string.IsNullOrWhiteSpace(dto.KeyWord))
                adverts = adverts.OrderByDescending(a => dto.KeyWord.Split(' ').Count(w => a.Title.Contains(w) || a.Description.Contains(w))).ThenByDescending(a => a.CreateDate).ToList();

            var mappedAdverts = Mapper.Map<List<AdvertDto>>(adverts);

            return mappedAdverts;
        }

        //public Task<List<AdvertDto>> ListNearestAdvertisementsAsync()
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<AdvertDto> AddOrEditAdvertAsync(AddOrEditAdvertDto dto)
        {
            Advert advert;
            if (!dto.Id.HasValue)
            {
                advert = Mapper.Map<Advert>(dto);
                advert.AdvertiserId = KutyAppContext.CurrentUser.Id;

                DbContext.Adverts.Add(advert);
            }
            else
            {
                advert = await DbContext.Adverts.SingleOrDefaultAsync(a => a.Id == dto.Id);
                if (advert == null)
                    throw new Exception("notfound");

                if (advert.Title != dto.Title)
                    advert.Title = dto.Title;

                if (advert.Description != dto.Description)
                    advert.Description = dto.Description;
            }

            await DbContext.SaveChangesAsync();

            return Mapper.Map<AdvertDto>(advert);
        }

        public async Task DeleteAdvertAsync(int id)
        {
            //TODO: favoritok torles
            var advert = await DbContext.Adverts.SingleOrDefaultAsync(a => a.Id == id);
            if (advert == null)
                throw new Exception("notdound");

            DbContext.Adverts.Remove(advert);

            await DbContext.SaveChangesAsync();
        }

        public async Task<AdvertDto> GetAdvertAsync(int id)
        {
            var advert = await DbContext.Adverts.SingleOrDefaultAsync(a => a.Id == id);
            if (advert == null)
                throw new Exception("notfound");

            return Mapper.Map<AdvertDto>(advert);
        }
    }
}
