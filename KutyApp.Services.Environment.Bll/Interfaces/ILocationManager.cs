using GeoAPI.Geometries;

namespace KutyApp.Services.Environment.Bll.Interfaces
{
    public interface ILocationManager
    {
        IGeometryFactory GeometryFactory { get; }
    }
}
