using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using KutyApp.Client.Services.ClientConsumer.Dtos;
using Refit;

namespace KutyApp.Client.Services.ClientConsumer.Interfaces
{
    public interface IEnvironmentApiService
    {
        [Get("/api/poi/list")]
        Task<List<PoiDto>> GetPoisAsync();

        [Post("/api/poi/listclosest")]
        Task<List<PoiDto>> GetClosestPoisAsync([Body] SearchPoiDto search);

        [Delete("/api/poi/{poiId}")]
        Task DeletePoiAsync(int poiId);

        [Post("/api/poi")]
        Task<PoiDto> AddOrEditPoiAsync([Body] AddOrEditPoiDto dto);
    }
}
