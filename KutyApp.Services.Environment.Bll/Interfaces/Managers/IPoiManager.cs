using KutyApp.Services.Environment.Bll.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KutyApp.Services.Environment.Bll.Interfaces
{
    public interface IPoiManager
    {
        Task<PoiDto> AddOrEditPoiAsync(AddOrEditPoiDto dto);
        Task DeletePoiAsync(int id);
        Task<List<PoiDto>> ListPoisAsync();
        Task<List<PoiDto>> ListClosestPoisAsync(SearchPoiDto search);
    }
}
