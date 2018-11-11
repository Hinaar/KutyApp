using KutyApp.Client.Services.LocalRepository.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KutyApp.Client.Services.LocalRepository.Interfaces
{
    public interface IPetRepository
    {
        Task<List<Dog>> GetDogsAsync();
        Task<Dog> GetDogByIdAsync(int id);
        Task<Dog> AddOrEditDogAsync(Dog dog);
        Task DeleteDogAsync(int id);
    }
}
