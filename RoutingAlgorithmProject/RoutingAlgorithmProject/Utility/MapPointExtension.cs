using Esri.ArcGISRuntime.Geometry;
using RoutingAlgorithmProject.Graph;

namespace RoutingAlgorithmProject.Utility
{
    public static class MapPointExtension
    {

        public static Coordinates ToCoordinates(this MapPoint location)
        {
            return new Coordinates((float)location.Y, (float)location.X);
        }

        public static MapPoint ToMapPoint(this Coordinates location)
        {
            return new MapPoint(location.Longitude, location.Latitude, SpatialReferences.Wgs84);
        }
    }
}
