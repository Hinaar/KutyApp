using KutyApp.Services.Environment.Bll.Entities.Enums;
using System;
using System.Collections.Generic;

namespace KutyApp.Services.Environment.Bll.Entities.Model
{
    public class Pet
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
        public ICollection<Habit> Habits { get; set; }
        public ICollection<MedicalTreatment> MedicalTreatments { get; set; }
        public ICollection<PetSitting> PetSittings { get; set; }
        public string OwnerId { get; set; }
        public virtual User Owner { get; set; }

        public Pet()
        {
            PetSittings = new HashSet<PetSitting>();
            Habits = new HashSet<Habit>();
            MedicalTreatments = new HashSet<MedicalTreatment>();
        }
    }
}