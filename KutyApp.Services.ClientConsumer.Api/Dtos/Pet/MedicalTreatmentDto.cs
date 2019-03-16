﻿using KutyApp.Client.Services.ClientConsumer.Enums;
using System;

namespace KutyApp.Client.Services.ClientConsumer.Dtos
{
    public class MedicalTreatmentDto
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
    }
}