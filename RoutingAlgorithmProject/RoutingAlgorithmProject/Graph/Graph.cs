using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutingAlgorithmProject.Graph
{
    public class Graph
    {
        private Dictionary<Coordinates, Vertex> vertexMap;

        public Graph() 
        {
          this.vertexMap = new Dictionary<Coordinates, Vertex>();
        }

        /// <summary>
        /// if a vertex does not exist at location coords, add to the dictionary
        /// </summary>
        /// <param name="coords"></param>
        /// <returns>reference to vertex at location coords</returns>
        public Vertex AddVertex(Coordinates coords)
        {
            var vertex = this.GetVertex(coords);
            if (vertex == null)
            {
                vertex = new Vertex(coords);
                vertexMap.Add(coords, vertex);
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

        public Vertex GetVertex(Coordinates coords)
        {
            if (vertexMap.ContainsKey(coords))
                return vertexMap[coords];
            else
                return null;
        }

        public List<Vertex> Verticies
        {
            get
            {
                return vertexMap.Values.ToList();
            }
        }
    }
}
