using KutyApp.Client.Services.ClientConsumer.Dtos;
using Refit;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace KutyApp.Client.Services.ClientConsumer.Interfaces
{
    public interface IEnvironmentApiService
    {
        #region account
        [Post("/login")]
        Task<string> LoginAsync([Body] LoginDto dto);

        [Post("/register")]
        Task<string> RegisterAsync([Body] RegisterDto dto);

        [Get("/ping")]
        Task PingAsync();

        [Get("/authping")]
        [Headers("Authorization: Bearer")]
        Task AuthPingAsync();
        #endregion

        #region advert
        [Post("/api/advertisement/latest")]
        [Headers("Authorization: Bearer")]
        Task<List<AdvertDto>> ListLatestAdvertisementsAsync(SearchAdvertDto dto);

        [Post("/api/advertisement/mylatest")]
        [Headers("Authorization: Bearer")]
        Task<List<AdvertDto>> ListMyLatestAdvertisementsAsync(SearchAdvertDto dto);

        [Post("/api/advertisement")]
        [Headers("Authorization: Bearer")]
        Task<List<AdvertDto>> AddOrEditAdvertisementAsync(AddOrEditAdvertDto dto);

        [Delete("/api/advertisement/{id}")]
        [Headers("Authorization: Bearer")]
        Task DeleteAdvertisementAsync(int id);

        [Get("/api/advertisement/{id}")]
        [Headers("Authorization: Bearer")]
        Task<AdvertDto> GetAdvertAsync(int id);
        #endregion

        #region data
        [Post("/api/data/image")]
        [Headers("Authorization: Bearer")]
        Task<string> UploadImageAsync([Body] StreamPart file);

        [Get("/api/data/image")]
        [Headers("Authorization: Bearer")]
        Task<HttpContent> GetImageAsync(string path);
        #endregion
        #region database
        [Get("/api/database/dbversion")]
        [Headers("Authorization: Bearer")]
        Task<DbVersionDto> GetDbVersionAsync();
        #endregion

        #region pet
        [Multipart]
        [Post("/api/pet/complex")]
        [Headers("Authorization: Bearer")]
        Task<PetDto> AddOrEditComplexPetAsync([Query]AddOrEditPetDto dto, StreamPart file);

        [Post("/api/pet")]
        [Headers("Authorization: Bearer")]
        Task<PetDto> AddOrEditPetAsync([Body]AddOrEditPetDto dto);

        [Delete("/api/pet/{id}")]
        [Headers("Authorization: Bearer")]
        Task DeletePetAsync(int id);

        [Post("/api/pet/addPetSitter")]
        [Headers("Authorization: Bearer")]
        Task AddPetSitterAsync([Body] AddOrRemovePetSitterDto dto);

        [Post("/api/pet/removePetSitter")]
        [Headers("Authorization: Bearer")]
        Task RemovePetSitterAsync([Body] AddOrRemovePetSitterDto dto);

        [Get("/api/pet/availableSitters")]
        [Headers("Authorization: Bearer")]
        Task ListAvailableSittersAsync(string username);

        [Get("/api/pet/{id}")]
        [Headers("Authorization: Bearer")]
        Task<PetDto> GetPetAsync(int id);

        [Get("/api/pet/myPets")]
        [Headers("Authorization: Bearer")]
        Task<List<PetDto>> GetMyPetsAsync();

        [Get("/api/pet/mySittedPets")]
        [Headers("Authorization: Bearer")]
        Task<List<PetDto>> GetMySittedPetsAsync();
        #endregion

        #region poi
        [Get("/api/poi/list")]
        [Headers("Authorization: Bearer")]
        Task<List<PoiDto>> GetPoisAsync();

        [Post("/api/poi/listclosest")]
        [Headers("Authorization: Bearer")]
        Task<List<PoiDto>> GetClosestPoisAsync([Body] SearchPoiDto search);

        [Delete("/api/poi/{poiId}")]
        [Headers("Authorization: Bearer")]
        Task DeletePoiAsync(int poiId);

        [Post("/api/poi")]
        [Headers("Authorization: Bearer")]
        Task<PoiDto> AddOrEditPoiAsync([Body] AddOrEditPoiDto dto);
        #endregion
    }
}
