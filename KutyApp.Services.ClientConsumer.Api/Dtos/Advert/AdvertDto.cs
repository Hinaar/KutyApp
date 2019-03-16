//using GeoAPI.Geometries;
using System;
using System.Collections.Generic;
using System.Text;

namespace KutyApp.Client.Services.ClientConsumer.Dtos
{
    public class AdvertDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        //public string AdvertiserId { get; set; }
        public UserDto Advertiser { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        //public IPoint Location { get; set; }
        public double Distance { get; set; }
    }
}
