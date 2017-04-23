using RoutingAlgorithmProject.Routing.Models;
using System.Collections.Generic;
using System.Linq;

namespace RoutingAlgorithmProject.Graph
{
    public class AStarGraph : RoutingGraph<AStarVertex>
    { 
        private Dictionary<Coordinates, AStarVertex> vertexMap;

        public AStarGraph()
        {
            this.vertexMap = new Dictionary<Coordinates, AStarVertex>();
        }

        /// <summary>
        /// if a vertex does not exist at location coords, add to the dictionary
        /// </summary>
        /// <param name="coords"></param>
        /// <returns>reference to vertex at location coords</returns>
        public override AStarVertex AddVertex(Coordinates coords)
        {
            var vertex = this.GetVertex(coords);
            if (vertex == null)
            {
                vertex = new AStarVertex(coords, double.MaxValue, double.MaxValue);
                vertexMap.Add(coords, vertex);
            }
            return vertex;
        }

        /// <summary>
        /// Adds an edge to the graph
        /// creates an edge in the neighbor list of each vertex
        /// </summary>
        /// <param name="e"></param>
        public override void AddEdge(Edge e)
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
        public override AStarVertex GetVertex(Coordinates coords)
        {
            if (vertexMap.ContainsKey(coords))
                return vertexMap[coords];
            else
                return null;
        }

        /// <summary>
        /// Returns all Verticies in the graph
        /// </summary>
        public override List<AStarVertex> Verticies
        {
            get
            {
                return vertexMap.Values.ToList();
            }
        }
    }
}
