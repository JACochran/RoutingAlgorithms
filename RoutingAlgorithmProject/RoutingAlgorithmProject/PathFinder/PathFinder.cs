using Esri.ArcGISRuntime.Geometry;
using RoutingAlgorithmProject.Graph;
using System.Collections.Generic;

namespace RoutingAlgorithmProject.PathFinder
{
    public abstract class PathFinder
    {
        protected Graph.Graph graph;

        public PathFinder(Graph.Graph graph)
        {
            this.graph = graph;
        }
        /// <summary>
        /// Finds the shortest path between two coordinates on a graph
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns> A list of edges from start to end</returns>
        public abstract List<Graph.Edge> FindShortestPath(MapPoint start, MapPoint end);

        /// <summary>
        /// Finds the cloest vertex in the graph to a point
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        protected Graph.Vertex FindClosestVertex(MapPoint point)
        {
            Graph.Vertex closest = null;
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

        protected double GetMinimumDistance(MapPoint start, MapPoint end)
        {
            return GeometryEngine.GeodesicDistance(start, end, LinearUnits.Meters);
        }
           
    }


}
