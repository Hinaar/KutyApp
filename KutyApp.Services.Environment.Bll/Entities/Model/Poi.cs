using GeoAPI.Geometries;
using KutyApp.Services.Environment.Bll.Entities.Enums;
using System;
using System.Collections.Generic;

namespace KutyApp.Services.Environment.Bll.Entities.Model
{
    public class Poi
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan? OpeningTime { get; set; }
        public TimeSpan? CloseTime { get; set; }
        public IPoint Location { get; set; }
        public EnvironmentType EnvironmentTypes { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<UserPoi> FavouredByUsers { get; set; }

        public Poi()
        {
            FavouredByUsers = new HashSet<UserPoi>();
        }
    }
}
