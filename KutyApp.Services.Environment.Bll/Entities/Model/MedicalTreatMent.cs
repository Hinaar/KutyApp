using KutyApp.Services.Environment.Bll.Entities.Enums;
using System;

namespace KutyApp.Services.Environment.Bll.Entities.Model
{
    public class MedicalTreatment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public MedicalTreatmentType Type { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Place { get; set; }
        public string Tender { get; set; }
        public double Price { get; set; }
        public string Currency { get; set; }
        public int PetId { get; set; }
        public virtual Pet Pet { get; set; }
    }
}