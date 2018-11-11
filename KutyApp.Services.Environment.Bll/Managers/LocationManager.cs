using GeoAPI.Geometries;
using KutyApp.Services.Environment.Bll.Interfaces;
using NetTopologySuite;

namespace KutyApp.Services.Environment.Bll.Managers
{
    public class LocationManager : ILocationManager
    {
        public IGeometryFactory GeometryFactory => NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
    }
}
