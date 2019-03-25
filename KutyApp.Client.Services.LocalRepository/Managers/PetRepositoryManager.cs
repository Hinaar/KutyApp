using AutoMapper;
using KutyApp.Client.Services.ClientConsumer.Dtos;
using KutyApp.Client.Services.LocalRepository.Entities;
using KutyApp.Client.Services.LocalRepository.Entities.Configuration;
using KutyApp.Client.Services.LocalRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KutyApp.Client.Services.LocalRepository.Managers
{
    public class PetRepositoryManager : IPetRepository
    {
        private readonly PetDbContext dbContext;
        private readonly IMapper mapper;
        public PetRepositoryManager(string dbPath)
        {
            dbContext = new PetDbContext(dbPath);
            mapper = new MapperConfiguration(c => c.AddProfile<KutyAppLocalRepositoryProfile>()).CreateMapper();
        }

        public async Task DeletePetsAsync()
        {
            var pets = await dbContext.Pets.Include(p => p.MedicalTreatments).Include(p => p.Habits).ToListAsync();
            dbContext.RemoveRange(pets);

            await dbContext.SaveChangesAsync();
        }

        public async Task<PetDto> GetDetailedPetByIdAsync(int petId)
        {
            var pet = await dbContext.Pets.Include(p => p.Habits).Include(p => p.MedicalTreatments)
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(p => p.Id == petId);

            return mapper.Map<PetDto>(pet);
        }

        public async Task<List<PetDto>> GetMyPetsAsync()
        {
             var pets = await dbContext.Pets.AsNoTracking().ToListAsync();
             return mapper.Map<List<PetDto>>(pets);
        }

        //TODO: delete
        //public async Task<Dog> AddOrEditDogAsync(Dog dog)
        //{
        //    Dog pet;

        //    if (dog.Id == 0)
        //    {
        //        pet = new Dog
        //        {
        //            Name = dog.Name,
        //            Gender = dog.Gender,
        //            ChipNumber = dog.ChipNumber,
        //            Color = dog.Color,
        //            ImagePath = dog.ImagePath,
        //            Weight = dog.Weight,
        //            BirthDate = dog.BirthDate
        //        };
        //        dbContext.Dogs.Add(pet);
        //    }

        //    else
        //    {
        //        //var tracking = dbContext.Update(dog);
        //        pet = await dbContext.Dogs.FirstOrDefaultAsync(d => d.Id == dog.Id);

        //        if (pet.Name != dog.Name)
        //            pet.Name = dog.Name;

        //        if (pet.Gender != dog.Gender)
        //            pet.Gender = dog.Gender;

        //        if (pet.ChipNumber != dog.ChipNumber)
        //            pet.ChipNumber = dog.ChipNumber;

        //        if (pet.Color != dog.Color)
        //            pet.Color = dog.Color;

        //        if (pet.ImagePath != dog.ImagePath)
        //            pet.ImagePath = dog.ImagePath;

        //        if (pet.Weight != dog.Weight)
        //            pet.Weight = dog.Weight;

        //        if (pet.BirthDate != dog.BirthDate)
        //            pet.BirthDate = dog.BirthDate;
        //    }

        //    await dbContext.SaveChangesAsync();
        //    return pet;
        //}

        //public async Task DeleteDogAsync(int id)
        //{
        //    var dog = await dbContext.Dogs.FindAsync(id);
        //    if (dog != null)
        //    {
        //        dbContext.Dogs.Remove(dog);
        //        await dbContext.SaveChangesAsync();
        //    }
        //}

        //public async Task<Dog> GetDogByIdAsync(int id)
        //{
        //    var dog = await dbContext.Dogs.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
        //    return dog;
        //}

        //public async Task<List<Dog>> GetDogsAsync()
        //{
        //    var dogs = await dbContext.Dogs.AsNoTracking().ToListAsync();
        //    return dogs;
        //}
    }
}
