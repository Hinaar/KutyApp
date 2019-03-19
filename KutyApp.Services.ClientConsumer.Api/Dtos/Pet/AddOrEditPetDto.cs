using KutyApp.Client.Services.ClientConsumer.Enums;
using System;
using System.Collections.Generic;

namespace KutyApp.Client.Services.ClientConsumer.Dtos
{
    public class AddOrEditPetDto
    {
        public int? Id { get; set; }
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
        public List<AddOrEditHabitDto> Habits { get; set; }
        public List<AddOrEditMedicalTreatmentDto> MedicalTreatments { get; set; }
        //public ICollection<AddOrPetSittingDto> PetSittings { get; set; }
    }
}
