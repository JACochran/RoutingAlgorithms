﻿using Esri.ArcGISRuntime.Geometry;
using RoutingAlgorithmProject.Graph;
using System.Collections.Generic;

namespace RoutingAlgorithmProject.PathFinder
{
    public abstract class PathFinder<T> where T : Vertex
    {
        protected RoutingGraph<T> graph;

        public PathFinder(RoutingGraph<T> graph)
        {
            this.graph = graph;
        }
        
        /// <summary>
        /// Finds the shortest path between two coordinates on a graph
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns> A list of edges from start to end</returns>
        public abstract List<Graph.Vertex> FindShortestPath(Coordinates start, Coordinates end);

        /// <summary>
        /// Finds the cloest vertex in the graph to a point
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        protected T FindClosestVertex(Coordinates point)
        {
            T closest = null;
            var minDistance = double.MaxValue;
            foreach(var vertex in graph.Verticies)
            {
                var distance = GetMinimumDistance(point, vertex.Coordinates);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = vertex;
                }
            }
            return closest;
        }

        /// <summary>
        /// Returns the edge from v to u
        /// </summary>
        /// <param name="v"></param>
        /// <param name="u"></param>
        /// <returns></returns>
        internal static Edge FindEdge(Vertex v, Vertex u)
        {
            Edge e = null;
            foreach (var neighbor in v.Neighbors)
            {
                if (neighbor.Key.Equals(u))
                {
                    e = neighbor.Value;
                    break;
                }
            }
            return e;
        }

        protected double GetMinimumDistance(Coordinates start, Coordinates end)
        {
            return GeometryEngine.GeodesicDistance(new MapPoint(start.Longitude, start.Latitude, SpatialReferences.Wgs84), new MapPoint(end.Longitude, end.Latitude, SpatialReferences.Wgs84), LinearUnits.Meters);
        }
           
    }


}
