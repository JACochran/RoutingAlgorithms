﻿using Esri.ArcGISRuntime.Geometry;
using OsmSharp.Tags;
using System;

namespace RoutingAlgorithmProject.Graph
{
    public class Edge
    {
        private float weight;
        private Vertex fromVertex;
        private Vertex toVertex;

        public Edge(Vertex fromVertex, Vertex toVertex)
        {
            this.fromVertex = fromVertex;
            this.toVertex = toVertex;
            this.weight = DistanceBetweenPoints(fromVertex.Coordinates, toVertex.Coordinates);
        }

        public float Weight
        {
            get
            {
                return weight;
            }
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

        /// <summary>
        /// Implementation of Haversine formula to find distance between two points on a globe
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns>distance in kilometers</returns>
        public static float DistanceBetweenPoints(MapPoint v1, MapPoint v2)
        {
            var R = 6371; // Radius of the earth in km
            var dLat = deg2rad(v2.Y - v1.Y);  // deg2rad below
            var dLon = deg2rad(v2.X - v1.X);
            var a =
              Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(deg2rad(v1.Y)) * Math.Cos(deg2rad(v2.Y)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c; // Distance in km
            return (float)d;
        }

        private static float deg2rad(double deg)
        {
            return (float)(deg * (Math.PI / 180));
        }

        public override string ToString()
        {
            return fromVertex.ToString() + "->" + toVertex.ToString();
        }
    }
}
