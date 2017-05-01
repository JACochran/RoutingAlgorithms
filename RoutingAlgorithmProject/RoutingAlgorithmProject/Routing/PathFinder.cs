using Esri.ArcGISRuntime.Geometry;
using RoutingAlgorithmProject.Graph;
using System.Collections.Generic;
using System;

namespace RoutingAlgorithmProject.Routing
{
    public abstract class PathFinder 
    {
        protected RoutingGraph graph;

        public PathFinder(RoutingGraph graph)
        {
            this.graph = graph;
        }
        
        public RoutingGraph Graph
        {
            get
            {
                return this.graph;
            }

        }

        /// <summary>
        /// Finds the shortest path between two coordinates on a graph
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns> A list of edges from start to end</returns>
        public abstract List<Graph.Vertex> FindShortestPath(Vertex startNode, Vertex endNode, ref float pathLength);

        public abstract string GetAbbreivatedName();

        /// <summary>
        /// Finds the cloest vertex in the graph to a point
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Vertex FindClosestVertex(Coordinates point)
        {
            Vertex closest = null;
            var minDistance = float.MaxValue;
            foreach(var vertex in graph.Verticies)
            {
                var distance = Edge.GetMinimumDistance(point, vertex.Coordinates);
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

        protected List<Vertex> GetPathResult(Vertex destination, ref float pathLength)
        {
            List<Vertex> path = new List<Vertex>();
            pathLength = 0;
            for (var vertex = destination; vertex != null; vertex = vertex.Previous)
            {
                if(vertex.Previous != null)
                    pathLength += Edge.GetMinimumDistance(vertex.Coordinates, vertex.Previous.Coordinates);
                path.Insert(0,vertex);
            }
            return path;
        }

        internal void ResetGraph()
        {
            graph.ResetGraph();
        }
    }


}
