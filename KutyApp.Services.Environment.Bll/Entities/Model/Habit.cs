using System;

namespace KutyApp.Services.Environment.Bll.Entities.Model
{
    public class Habit
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public double Amount { get; set; }
        public string Unit { get; set; }
        public int PetId { get; set; }
        public virtual Pet Pet { get; set; }
    }
}