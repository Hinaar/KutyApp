using KutyApp.Client.Services.ClientConsumer.Enums;
using System;

namespace KutyApp.Client.Services.ClientConsumer.Dtos
{
    public class AddOrEditPoiDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public TimeSpan? OpeningTime { get; set; }
        public TimeSpan? CloseTime { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public EnvironmentType EnvironmentTypes { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string PhoneNumber { get; set; }
    }
}
