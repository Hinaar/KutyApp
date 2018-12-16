using AutoMapper;
using GeoAPI.Geometries;
using KutyApp.Services.Environment.Bll.Dtos;
using KutyApp.Services.Environment.Bll.Entities.Model;
using KutyApp.Services.Environment.Bll.Interfaces;
using NetTopologySuite;

namespace KutyApp.Services.Environment.Bll.Mapping
{
    public class KutyAppServiceProfile : Profile
    {
        private ILocationManager LocationManager { get; }

        public KutyAppServiceProfile()
        {
            //Poi
            CreateMap<AddOrEditPoiDto, Poi>().ForMember(p => p.Location, m => m.ResolveUsing<LocationResolver>());
            CreateMap<Poi, AddOrEditPoiDto>().ForMember(d => d.Latitude, m => m.MapFrom(p => p.Location.Coordinate.Y))
                                             .ForMember(d => d.Longitude, m => m.MapFrom(p => p.Location.Coordinate.X));
            CreateMap<Poi, PoiDto>().ForMember(d => d.Latitude, m => m.MapFrom(p => p.Location.Coordinate.Y))
                                    .ForMember(d => d.Longitude, m => m.MapFrom(p => p.Location.Coordinate.X));
        }
    }
}
