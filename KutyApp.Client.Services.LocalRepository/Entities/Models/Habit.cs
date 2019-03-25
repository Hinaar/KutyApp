using System;

namespace KutyApp.Client.Services.LocalRepository.Entities.Models
{
    public class Habit
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double? StartTime { get; set; }
        public double? EndTime { get; set; }
        public double Amount { get; set; }
        public string Unit { get; set; }
        public int PetId { get; set; }
        public virtual Pet Pet { get; set; }
    }
}