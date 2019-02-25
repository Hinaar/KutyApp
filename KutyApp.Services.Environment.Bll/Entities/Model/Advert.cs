using GeoAPI.Geometries;
using System;

namespace KutyApp.Services.Environment.Bll.Entities.Model
{
    public class Advert
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AdvertiserId { get; set; }
        public virtual User Advertiser { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public IPoint Location { get; set; }
    }
}