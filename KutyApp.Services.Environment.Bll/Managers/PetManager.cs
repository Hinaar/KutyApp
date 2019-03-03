using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KutyApp.Services.Environment.Bll.Dtos;
using KutyApp.Services.Environment.Bll.Entities;
using KutyApp.Services.Environment.Bll.Entities.Model;
using KutyApp.Services.Environment.Bll.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KutyApp.Services.Environment.Bll.Managers
{
    public class PetManager : IPetManager
    {
        private KutyAppServiceDbContext DbContext { get; }
        private IMapper Mapper { get; }
        private IKutyAppContext KutyAppContext { get; }
        private IAuthManager AuthManager { get; }

        public PetManager(KutyAppServiceDbContext dbContext, IMapper mapper, IKutyAppContext kutyAppContext, IAuthManager authManager)
        {
            DbContext = dbContext;
            Mapper = mapper;
            KutyAppContext = kutyAppContext;
            AuthManager = authManager;
        }

        public async Task<PetDto> AddOrEditPetAsync(AddOrEditPetDto addOrEditPet)
        {
            Pet pet;
            if (!addOrEditPet.Id.HasValue)
            {
                pet = new Pet
                {
                    Name = addOrEditPet.Name,
                    Gender = addOrEditPet.Gender,
                    ChipNumber = addOrEditPet.ChipNumber,
                    Color = addOrEditPet.Color,
                    ImagePath = addOrEditPet.ImagePath,
                    Weight = addOrEditPet.Weight,
                    BirthDate = addOrEditPet.BirthDate,
                    Kind = addOrEditPet.Kind
                };

                pet.Habits = Mapper.Map<List<Habit>>(addOrEditPet.Habits ?? Enumerable.Empty<AddOrEditHabitDto>());
                pet.MedicalTreatments = Mapper.Map<List<MedicalTreatment>>(addOrEditPet.MedicalTreatments ?? Enumerable.Empty<AddOrEditMedicalTreatmentDto>());

                pet.OwnerId = KutyAppContext.CurrentUser.Id;

                DbContext.Pets.Add(pet);
            }
            else
            {
                pet = await DbContext.Pets.FirstOrDefaultAsync(d => d.Id == addOrEditPet.Id);
                if (pet == null)
                    throw new System.Exception("notfound");

                if (pet.Name != addOrEditPet.Name)
                    pet.Name = addOrEditPet.Name;

                if (pet.Gender != addOrEditPet.Gender)
                    pet.Gender = addOrEditPet.Gender;

                if (pet.ChipNumber != addOrEditPet.ChipNumber)
                    pet.ChipNumber = addOrEditPet.ChipNumber;

                if (pet.Color != addOrEditPet.Color)
                    pet.Color = addOrEditPet.Color;

                if (pet.ImagePath != addOrEditPet.ImagePath)
                    pet.ImagePath = addOrEditPet.ImagePath;

                if (pet.Weight != addOrEditPet.Weight)
                    pet.Weight = addOrEditPet.Weight;

                if (pet.BirthDate != addOrEditPet.BirthDate)
                    pet.BirthDate = addOrEditPet.BirthDate;

                if (pet.Kind != addOrEditPet.Kind)
                    pet.Kind = addOrEditPet.Kind;

                #region habits
                //delted habit
                foreach (var habit in pet.Habits.ToList().Where(h => !addOrEditPet.Habits.Any(dto => dto.Id == h.Id)))
                    pet.Habits.Remove(habit);

                //new habit
                foreach (var habitDto in addOrEditPet.Habits.Where(h => !h.Id.HasValue))
                    pet.Habits.Add(Mapper.Map<Habit>(habitDto));

                //edited habit
                foreach (var habitDto in addOrEditPet.Habits.Where(h => h.Id.HasValue))
                {
                    var originalHabit = pet.Habits.SingleOrDefault(h => h.Id == habitDto.Id);
                    if (originalHabit == null)
                        throw new System.Exception("notfound");

                    if (originalHabit.Title != habitDto.Title)
                        originalHabit.Title = habitDto.Title;

                    if (originalHabit.Description != habitDto.Description)
                        originalHabit.Description = habitDto.Description;

                    if (originalHabit.StartTime != habitDto.StartTime)
                        originalHabit.StartTime = habitDto.StartTime;

                    if (originalHabit.EndTime != habitDto.EndTime)
                        originalHabit.EndTime = habitDto.EndTime;

                    if (originalHabit.Amount != habitDto.Amount)
                        originalHabit.Amount = habitDto.Amount;

                    if (originalHabit.Unit != habitDto.Unit)
                        originalHabit.Unit = habitDto.Unit;
                }
                #endregion

                #region MedicalTreatments
                //deleted treatment
                foreach (var medicalTreatment in pet.MedicalTreatments.ToList().Where(m => !addOrEditPet.MedicalTreatments.Any(dto => dto.Id == m.Id)))
                    pet.MedicalTreatments.Remove(medicalTreatment);

                //new treatment
                foreach (var treatmentDto in addOrEditPet.MedicalTreatments.Where(h => !h.Id.HasValue))
                    pet.MedicalTreatments.Add(Mapper.Map<MedicalTreatment>(treatmentDto));

                //edited treatment
                foreach(var medicalTreatmentDto in addOrEditPet.MedicalTreatments.Where(m => m.Id.HasValue))
                {
                    var originalTreatment = pet.MedicalTreatments.SingleOrDefault(m => m.Id == medicalTreatmentDto.Id);
                    if (originalTreatment == null)
                        throw new System.Exception("notfound");

                    if (originalTreatment.Name != medicalTreatmentDto.Name)
                        originalTreatment.Name = medicalTreatmentDto.Name;

                    if (originalTreatment.Type != medicalTreatmentDto.Type)
                        originalTreatment.Type = medicalTreatmentDto.Type;

                    if (originalTreatment.Description != medicalTreatmentDto.Name)
                        originalTreatment.Description = medicalTreatmentDto.Name;

                    if (originalTreatment.Date != medicalTreatmentDto.Date)
                        originalTreatment.Date = medicalTreatmentDto.Date;

                    if (originalTreatment.Place != medicalTreatmentDto.Place)
                        originalTreatment.Place = medicalTreatmentDto.Place;

                    if (originalTreatment.Tender != medicalTreatmentDto.Tender)
                        originalTreatment.Tender = medicalTreatmentDto.Tender;

                    if (originalTreatment.Price != medicalTreatmentDto.Price)
                        originalTreatment.Price = medicalTreatmentDto.Price;

                    if (originalTreatment.Currency != medicalTreatmentDto.Currency)
                        originalTreatment.Currency = medicalTreatmentDto.Currency;
                }
                #endregion
            }

            await DbContext.SaveChangesAsync();

            return await GetPetAsync(pet.Id);
        }

        public async Task AddPetSitter(AddOrRemovePetSitterDto dto)
        {
            bool petExists = await DbContext.Pets.AnyAsync(p => p.Id == dto.PetId);

            var userId = await AuthManager.GetUserIdAsync(dto.UserName);

            if (petExists && !string.IsNullOrWhiteSpace(userId))
            {
                DbContext.PetSittings.Add(new PetSitting { PetId = dto.PetId, SitterId = userId });
                await DbContext.SaveChangesAsync();
            }
            else
                throw new System.Exception("notdound");
        }

        public async Task DeleteDogAsync(int id)
        {
            var pet = await DbContext.Pets.Include(p => p.Habits)
                                          .Include(p => p.MedicalTreatments)
                                          .Include(p => p.PetSittings)
                                          .FirstOrDefaultAsync(p => p.Id == id);

            if (pet == null)
                throw new System.Exception("notdound");

            if (pet.OwnerId != KutyAppContext.CurrentUser.Id)
                throw new System.Exception("no rights");

            DbContext.Pets.Remove(pet);

            await DbContext.SaveChangesAsync();
        }

        public async Task RemovePetSitter(AddOrRemovePetSitterDto dto)
        {
            var userId = await AuthManager.GetUserIdAsync(dto.UserName);

            var petSitting = await DbContext.PetSittings.Include(ps => ps.Pet).FirstOrDefaultAsync(ps => ps.PetId == dto.PetId && ps.SitterId == userId);
            if (petSitting != null && petSitting.Pet.OwnerId == KutyAppContext.CurrentUser.Id)
            {
                petSitting.Pet = null;
                DbContext.PetSittings.Remove(petSitting);
            }

            await DbContext.SaveChangesAsync();
        }

        public async Task<PetDto> GetPetAsync(int id)
        {
            var pet = await DbContext.Pets.Include(p => p.Habits)
                                          .Include(p => p.MedicalTreatments)
                                          .Include(p => p.PetSittings).ThenInclude(ps => ps.Sitter)
                                          .Include(p => p.Owner)
                                          .AsNoTracking()
                                          .FirstOrDefaultAsync(p => p.Id == id);

            if (pet == null)
                throw new System.Exception("notfound");

            var dto = Mapper.Map<PetDto>(pet);

            return dto;
        }

        public async Task<List<PetDto>> ListMyPetsAsync()
        {
            var pets = await DbContext.Pets.Where(p => p.OwnerId == KutyAppContext.CurrentUser.Id)
                                           .Include(p => p.Habits)
                                           .Include(p => p.MedicalTreatments)
                                           .Include(p => p.PetSittings).ThenInclude(ps => ps.Sitter)
                                           .Include(p => p.Owner)
                                           .AsNoTracking().ToListAsync();

            return Mapper.Map<List<PetDto>>(pets ?? Enumerable.Empty<Pet>());
        }

        public async Task<List<PetDto>> ListMySittedPetsAsync()
        {
            var pets = await DbContext.Pets.Where(p => p.PetSittings.Any(ps => ps.SitterId == KutyAppContext.CurrentUser.Id))
                                           .Include(p => p.Habits)
                                           .Include(p => p.MedicalTreatments)
                                           .Include(p => p.PetSittings).ThenInclude(ps => ps.Sitter)
                                           .Include(p => p.Owner)
                                           .AsNoTracking().ToListAsync();

            return Mapper.Map<List<PetDto>>(pets ?? Enumerable.Empty<Pet>());
        }
    }
}
