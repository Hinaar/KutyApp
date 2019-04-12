using KutyApp.Client.Services.ClientConsumer.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KutyApp.Client.Services.LocalRepository.Interfaces
{
    public interface IPetRepository
    {
        //Task<List<Dog>> GetDogsAsync();
        //Task<Dog> GetDogByIdAsync(int id);
        //Task<Dog> AddOrEditDogAsync(Dog dog);
        //Task DeleteDogAsync(int id);

        Task<List<PetDto>> GetMyPetsAsync();
        Task<PetDto> GetDetailedPetByIdAsync(int petId);
        Task DeletePetsAsync();
        Task SaveMyPetsAsync(List<PetDto> dtos);
    }
}
