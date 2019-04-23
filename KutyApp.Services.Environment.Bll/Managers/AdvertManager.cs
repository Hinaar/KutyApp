﻿using System;
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
            var query = DbContext.Adverts.Include(a => a.Advertiser).AsQueryable();

            if (!string.IsNullOrWhiteSpace(dto.KeyWord))
            {
                var words = dto.KeyWord.Split(' ');
                query = query.Where(a => words.Any(w => a.Title.Contains(w, StringComparison.CurrentCultureIgnoreCase) || a.Description.Contains(w))); 
            }

            var adverts = await query.Include(a => a.Advertiser).OrderByDescending(a => a.CreateDate).ToListAsync();

            if (!string.IsNullOrWhiteSpace(dto.KeyWord))
                adverts = adverts.OrderByDescending(a => dto.KeyWord.Split(' ').Count(w => a.Title.Contains(w, StringComparison.CurrentCultureIgnoreCase) || a.Description.Contains(w, StringComparison.CurrentCultureIgnoreCase)))
                                 .ThenByDescending(a => a.CreateDate)
                                 .ToList();

            var mappedAdverts = Mapper.Map<List<AdvertDto>>(adverts);

            return mappedAdverts;
        }

        public async Task<List<AdvertDto>> ListMyLatestAdvertisementsAsync(SearchAdvertDto dto)
        {
            var query = DbContext.Adverts.Include(a => a.Advertiser).Where(a => a.AdvertiserId == KutyAppContext.CurrentUser.Id);

            if (!string.IsNullOrWhiteSpace(dto.KeyWord))
            {
                var words = dto.KeyWord.Split(' ');
                query = query.Where(a => words.Any(w => a.Title.Contains(w, StringComparison.CurrentCultureIgnoreCase) || a.Description.Contains(w, StringComparison.CurrentCultureIgnoreCase)));
            }

            var adverts = await query.Include(a => a.Advertiser).OrderByDescending(a => a.CreateDate).ToListAsync();

            if (!string.IsNullOrWhiteSpace(dto.KeyWord))
                adverts = adverts.OrderByDescending(a => dto.KeyWord.Split(' ').Count(w => a.Title.Contains(w, StringComparison.CurrentCultureIgnoreCase) || a.Description.Contains(w, StringComparison.CurrentCultureIgnoreCase)))
                                 .ThenByDescending(a => a.CreateDate)
                                 .ToList();

            var mappedAdverts = Mapper.Map<List<AdvertDto>>(adverts);

            return mappedAdverts;
        }

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
                advert = await DbContext.Adverts.Include(a => a.Advertiser).SingleOrDefaultAsync(a => a.Id == dto.Id);
                if (advert == null)
                    throw new Exception(ExceptionMessages.NotFound);

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
            var advert = await DbContext.Adverts.SingleOrDefaultAsync(a => a.Id == id);
            if (advert == null)
                throw new Exception(ExceptionMessages.NotFound);

            DbContext.Adverts.Remove(advert);

            await DbContext.SaveChangesAsync();
        }

        public async Task<AdvertDto> GetAdvertAsync(int id)
        {
            var advert = await DbContext.Adverts.Include(a => a.Advertiser).SingleOrDefaultAsync(a => a.Id == id);
            if (advert == null)
                throw new Exception(ExceptionMessages.NotFound);

            return Mapper.Map<AdvertDto>(advert);
        }

        //TODO favoritok
    }
}
