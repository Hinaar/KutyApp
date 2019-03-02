using KutyApp.Services.Environment.Bll.Entities.Enums;
using KutyApp.Services.Environment.Bll.Entities.Model;
using System;
using System.Collections.Generic;

namespace KutyApp.Services.Environment.Bll.Dtos
{
    public class PetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ChipNumber { get; set; }
        public Gender Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Color { get; set; }
        public double? Weight { get; set; }
        public string ImagePath { get; set; }
        public string Kind { get; set; }
        public int? Age
        {
            get => BirthDate.HasValue ?
                (DateTime.Now.Year - BirthDate.Value.Year - 1) +
                        (((DateTime.Now.Month > BirthDate.Value.Month) ||
                        ((DateTime.Now.Month == BirthDate.Value.Month) && (DateTime.Now.Day >= BirthDate.Value.Day))) ? 1 : 0) :
                0;
            private set { }
        }
        public UserDto Owner { get; set; }
        public bool IsMyPet { get; set; } = false;
        public List<HabitDto> Habits { get; set; }
        public List<MedicalTreatmentDto> MedicalTreatments { get; set; }
        public List<UserDto> Sitters { get; set; }
        //public ICollection<AddOrPetSittingDto> PetSittings { get; set; }
    }
}
