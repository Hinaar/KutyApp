using KutyApp.Services.Environment.Bll.Entities.Enums;
using System;

namespace KutyApp.Services.Environment.Bll.Dtos
{
    public class PoiDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan? OpeningTime { get; set; }
        public TimeSpan? CloseTime { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public EnvironmentType EnvironmentTypes { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string PhoneNumber { get; set; }
        public double Distance { get; set; }
    }
}
