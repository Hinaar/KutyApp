using System;

namespace KutyApp.Client.Services.ClientConsumer.Dtos
{
    public class HabitDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public double Amount { get; set; }
        public string Unit { get; set; }
        public int PetId { get; set; }
    }
}