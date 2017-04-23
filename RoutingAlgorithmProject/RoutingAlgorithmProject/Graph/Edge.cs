using OsmSharp.Tags;
using System;

namespace RoutingAlgorithmProject.Graph
{
    public class Edge
    {
        public float weight;
        public TagsCollectionBase tags;
        private Vertex fromVertex;
        private Vertex toVertex;

        public Edge(Vertex fromVertex, Vertex toVertex)
        {
            this.fromVertex = fromVertex;
            this.toVertex = toVertex;
            this.weight = DistanceBetweenPoints(fromVertex.Coordinates, toVertex.Coordinates);
        }

        public Vertex From
        {
            get
            {
                return fromVertex;
            }
        }

        public Vertex To
        {
            get
            {
                return toVertex;
            }
        }

        public static float DistanceBetweenPoints(Coordinates v1, Coordinates v2)
        {
            var R = 6371; // Radius of the earth in km
            var dLat = deg2rad(v2.Latitude - v1.Latitude);  // deg2rad below
            var dLon = deg2rad(v2.Longitude - v1.Longitude);
            var a =
              Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(deg2rad(v1.Latitude)) * Math.Cos(deg2rad(v2.Latitude)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c; // Distance in km
            return (float)d;
        }

        private static float deg2rad(float? deg)
        {
            return (float)(deg * (Math.PI / 180));
        }
    }
}
