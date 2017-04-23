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

        public static Polyline ToPolyline(this List<Edge> edges)
        {
            var polyline = new PolylineBuilder(SpatialReferences.Wgs84);
            foreach (var edge in edges)
            {
                var startPoint = edge.To.Coordinates.ToMapPoint();
                var endPoint = edge.From.Coordinates.ToMapPoint();
                polyline.AddPoint(startPoint);
                polyline.AddPoint(endPoint);
            }
            return polyline.ToGeometry();
        }
    }
}
