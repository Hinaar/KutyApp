using System;
using System.Collections.Generic;
using System.Text;

namespace KutyApp.Services.Environment.Bll.Dtos
{
    public class AddOrEditAdvertDto
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        //public string AdvertiserId { get; set; }
        //public UserDto Advertiser { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
    }
}
