using System;
using System.Collections.Generic;

namespace RoutingAlgorithmProject.Graph
{
    public abstract class RoutingGraph<T> where T : Vertex
    {
        // <summary>
        /// if a vertex does not exist at location coords, add to the dictionary
        /// </summary>
        /// <param name="coords"></param>
        /// <returns>reference to vertex at location coords</returns>
        public abstract T AddVertex(Coordinates coords);

        /// <summary>
        /// Adds an edge to the graph
        /// creates an edge in the neighbor list of each vertex
        /// </summary>
        /// <param name="e"></param>
        public abstract void AddEdge(Edge e);

        /// <summary>
        /// Checks if the graph contains a vertex at location coords
        /// returns null if not found
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
        public abstract T GetVertex(Coordinates coords);

        /// <summary>
        /// Returns all Verticies in the graph
        /// </summary>
        public virtual List<T> Verticies { get; internal set; }

        public abstract void CleanGraph();
    }
}
