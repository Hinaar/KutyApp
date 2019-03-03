using GeoAPI.Geometries;
using KutyApp.Services.Environment.Bll.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KutyApp.Services.Environment.Bll.Interfaces
{
    public interface IAdvertManager
    {
        Task<List<AdvertDto>> ListLatestAdvertisementsAsync(SearchAdvertDto dto);
        Task<List<AdvertDto>> ListMyLatestAdvertisementsAsync(SearchAdvertDto dto);
        //Task<List<AdvertDto>> ListNearestAdvertisementsAsync();
        Task<AdvertDto> AddOrEditAdvertAsync(AddOrEditAdvertDto dto);
        Task DeleteAdvertAsync(int id);
        Task<AdvertDto> GetAdvertAsync(int id);
    }
}
