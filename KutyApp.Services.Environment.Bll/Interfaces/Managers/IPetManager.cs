using KutyApp.Services.Environment.Bll.Dtos;
using KutyApp.Services.Environment.Bll.Entities.Model;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KutyApp.Services.Environment.Bll.Interfaces
{
    public interface IPetManager
    {
        Task<PetDto> AddOrEditPetAsync(AddOrEditPetDto addOrEditPet);
        Task DeleteDogAsync(int id);
        Task AddPetSitter(AddOrRemovePetSitterDto dto);
        Task RemovePetSitter(AddOrRemovePetSitterDto dto);
        Task<PetDto> GetPetAsync(int id);
        Task<List<PetDto>> ListMyPetsAsync();
        Task<List<PetDto>> ListMySittedPetsAsync();
        Task<PetDto> AddOrEditComplexPetAsync(AddOrEditPetDto dto, IFormFile file);
        Task<List<UserDto>> ListAvailableSittersAsync(string username);
        Task<List<UserDto>> ListMyPetSittersAsync();
    }
}
