//using System.Collections.Generic;
//using System.Linq;

//namespace RoutingAlgorithmProject.Graph
//{
//    /// <summary>
//    /// Graph implementation
//    /// stores vertices internally using a map of coordinate location to vertex.
//    /// </summary>
//    public class Graph : RoutingGraph<Vertex>
//    {
//        private Dictionary<Coordinates, Vertex> vertexMap;

//        public Graph()
//        {
//            this.vertexMap = new Dictionary<Coordinates, Vertex>();
//        }

//        /// <summary>
//        /// if a vertex does not exist at location coords, add to the dictionary
//        /// </summary>
//        /// <param name="coords"></param>
//        /// <returns>reference to vertex at location coords</returns>
//        public override Vertex AddVertex(Coordinates coords)
//        {
//            var vertex = this.GetVertex(coords);
//            if (vertex == null)
//            {
//                vertex = new Vertex(coords);
//                vertexMap.Add(coords, vertex);
//            }
//            return vertex;
//        }

//        /// <summary>
//        /// Adds an edge to the graph
//        /// creates an edge in the neighbor list of each vertex
//        /// </summary>
//        /// <param name="e"></param>
//        public override void AddEdge(Edge e)
//        {
//            e.From.AddEdge(e.To, e);
//            e.To.AddEdge(e.From, e);
//        }

//        /// <summary>
//        /// Checks if the graph contains a vertex at location coords
//        /// returns null if not found
//        /// </summary>
//        /// <param name="coords"></param>
//        /// <returns></returns>
//        public override Vertex GetVertex(Coordinates coords)
//        {
//            if (vertexMap.ContainsKey(coords))
//                return vertexMap[coords];
//            else
//                return null;
//        }

//        /// <summary>
//        /// Returns all Verticies in the graph
//        /// </summary>
//        public List<Vertex> Verticies
//        {
//            get
//            {
//                return vertexMap.Values.ToList();
//            }
//        }


//    }
//}
