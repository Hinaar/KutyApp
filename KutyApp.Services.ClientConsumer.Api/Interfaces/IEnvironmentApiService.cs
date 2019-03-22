using KutyApp.Client.Services.ClientConsumer.Dtos;
using KutyApp.Services.Environment.Bll.Dtos;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KutyApp.Client.Services.ClientConsumer.Interfaces
{
    public interface IEnvironmentApiService
    {
        [Get("/api/pet/myPets")]
        [Headers("Authorization: Bearer")]
        Task<List<PetDto>> GetMyPetsAsync();

        [Get("/api/poi/list")]
        Task<List<PoiDto>> GetPoisAsync();

        [Post("/api/poi/listclosest")]
        Task<List<PoiDto>> GetClosestPoisAsync([Body] SearchPoiDto search);

        [Delete("/api/poi/{poiId}")]
        Task DeletePoiAsync(int poiId);

        [Post("/api/poi")]
        Task<PoiDto> AddOrEditPoiAsync([Body] AddOrEditPoiDto dto);

        [Get("/api/database/dbversion")]
        Task <object> GetAppVersion();

        [Multipart]
        [Post("/api/pet/complex")]
        [Headers("Authorization: Bearer")]
        Task<List<PetDto>> AddOrEditComplexPet([Query]AddOrEditPetDto dto, StreamPart file);

        [Post("/login")]
        Task<string> LoginAsync([Body] LoginDto dto);

        [Post("/register")]
        Task<string> RegisterAsync([Body] RegisterDto dto);
    }
}
