using Esri.ArcGISRuntime.Geometry;
using RoutingAlgorithmProject.Graph;
using System.Collections.Generic;

namespace RoutingAlgorithmProject.Utility
{
    public static class GeometryExtensions
    {

        public static Coordinates ToCoordinates(this MapPoint location)
        {
            return new Coordinates((float)location.Y, (float)location.X);
        }

        public static MapPoint ToMapPoint(this Coordinates location)
        {
            return new MapPoint(location.Longitude, location.Latitude, SpatialReferences.Wgs84);
        }

        public static Polyline ToPolyline(this List<Vertex> path, MapPoint start, MapPoint end)
        {
            var polyline = new PolylineBuilder(SpatialReferences.Wgs84);
            polyline.AddPoint(start);
            foreach (var v in path)
            {
                var mapPoint = v.Coordinates.ToMapPoint();
                polyline.AddPoint(mapPoint);
            }
            polyline.AddPoint(end);
            return polyline.ToGeometry();
        }
    }
}
