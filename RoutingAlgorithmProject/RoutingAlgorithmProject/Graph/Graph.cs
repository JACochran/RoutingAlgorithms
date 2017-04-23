using Esri.ArcGISRuntime.Geometry;
using System.Collections.Generic;
using System.Linq;

namespace RoutingAlgorithmProject.Graph
{
    /// <summary>
    /// Graph implementation
    /// stores vertices internally using a map of coordinate location to vertex.
    /// </summary>
    public class Graph
    {
        private HashSet<Vertex> vertexMap;

        public Graph() 
        {
          this.vertexMap = new HashSet<Vertex>();
        }

        /// <summary>
        /// if a vertex does not exist at location coords, add to the dictionary
        /// </summary>
        /// <param name="coords"></param>
        /// <returns>reference to vertex at location coords</returns>
        public Vertex AddVertex(MapPoint coords)
        {
            var vertex = this.GetVertex(coords);
            if (vertex == null)
            {
                vertex = new Vertex(coords);
                vertexMap.Add(vertex);
            }
            return vertex;
        }

        /// <summary>
        /// Adds an edge to the graph
        /// creates an edge in the neighbor list of each vertex
        /// </summary>
        /// <param name="e"></param>
        public void AddEdge(Edge e)
        {
            e.From.AddEdge(e.To, e);
            e.To.AddEdge(e.From, e);
        }

        /// <summary>
        /// Checks if the graph contains a vertex at location coords
        /// returns null if not found
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
        public Vertex GetVertex(MapPoint coordinate)
        {
            return vertexMap.FirstOrDefault(vertex => vertex.Coordinates.IsEqual(coordinate));
        }

        /// <summary>
        /// Returns all Verticies in the graph
        /// </summary>
        public List<Vertex> Verticies
        {
            get
            {
                return vertexMap.ToList();
            }
        }
    }
}
