using AutoMapper;
using GeoAPI.Geometries;
using KutyApp.Services.Environment.Bll.Dtos;
using KutyApp.Services.Environment.Bll.Entities.Model;
using KutyApp.Services.Environment.Bll.Interfaces;

namespace KutyApp.Services.Environment.Bll.Mapping
{
    public class LocationResolver : IValueResolver<AddOrEditPoiDto, Poi, IPoint>
    {
        private ILocationManager LocationManager { get; }

        public LocationResolver(ILocationManager locationManager)
        {
            LocationManager = locationManager;
        }

        public IPoint Resolve(AddOrEditPoiDto source, Poi destination, IPoint destMember, ResolutionContext context) =>
            LocationManager.GeometryFactory.CreatePoint(new Coordinate(source.Latitude, source.Longitude));
    }
}
