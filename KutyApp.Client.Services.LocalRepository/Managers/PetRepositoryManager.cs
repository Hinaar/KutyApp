using KutyApp.Client.Services.LocalRepository.Entities;
using KutyApp.Client.Services.LocalRepository.Entities.Models;
using KutyApp.Client.Services.LocalRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KutyApp.Client.Services.LocalRepository.Managers
{
    public class PetRepositoryManager : IPetRepository
    {
        private readonly PetDbContext dbContext;
        public PetRepositoryManager(string dbPath)
        {
            dbContext = new PetDbContext(dbPath);
        }

        public async Task<Dog> AddOrEditDogAsync(Dog dog)
        {
            if (dog.Id == 0)
            {
                var tracking = await dbContext.Dogs.AddAsync(dog);
                await dbContext.SaveChangesAsync();
                return dog;
            }

            else
            {
                //TODO: propertynkent
                var tracking = dbContext.Update(dog);
                try
                {
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception e)
                {

                    throw;
                }
                return dog;
            }
        }

        public async Task DeleteDogAsync(int id)
        {
            var dog = await dbContext.Dogs.FindAsync(id);
            if (dog != null)
            {
                dbContext.Dogs.Remove(dog);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<Dog> GetDogByIdAsync(int id)
        {
            var dog = await dbContext.Dogs.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
            return dog;
        }

        public async Task<List<Dog>> GetDogsAsync()
        {
            var dogs = await dbContext.Dogs.AsNoTracking().ToListAsync();
            return dogs;
        }
    }
}
