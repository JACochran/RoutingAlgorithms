using System;
using System.Collections.Generic;
using System.Linq;
using RoutingAlgorithmProject.Graph;

namespace RoutingAlgorithmProject.Routing
{
    class DijkstraPathFinder : PathFinder
    {
        public DijkstraPathFinder(RoutingGraph graph) : base(graph)
        {
        }
        
        /// <summary>
        /// Find shortest path from startNode to endNode using Dijkstra's algorithm
        /// </summary>
        /// <param name="startNode">start vertex in graph</param>
        /// <param name="endNode">destination vertex in graph</param>
        /// <param name="pathLength">length of shortest path</param>
        /// <returns></returns>
        public override List<Vertex> FindShortestPath(Vertex startNode, Vertex endNode, ref float pathLength)
        {
            List<Vertex> nodes = new List<Vertex>();          // list to store all vertices
            nodes.AddRange(this.graph.Verticies);             // add all vertices in graph
            startNode.CostFromStart = 0;                      // set start node distance to 0

            while (nodes.Count > 0)                           // loop through each vertex
            {
                Vertex u = MinDist(nodes);                    // find cloest node in vertex list
                if (u.Equals(endNode))                        // if node is destination node return path
                    return GetPathResult(endNode, ref pathLength);
                nodes.Remove(u);                              // remove node from list
                foreach (var neighbor in u.Neighbors)         // check each adjacent node
                {
                    var newCostFromStart = u.CostFromStart + neighbor.Value.Weight;      
                    if (newCostFromStart < neighbor.Key.CostFromStart)  // if new weight is less than old weight
                    {
                        neighbor.Key.CostFromStart = newCostFromStart;  // update weight
                        neighbor.Key.Previous = u;                      // set previous vertex
                    }
                }
            }
            return null;                                        // if a path is not found return null
        }

       

        public override string GetAbbreivatedName()
        {
            return "DIKL";
        }

        /// <summary>
        /// Finds vertex with minimum CostFromStart
        /// </summary>
        /// <param name="list">List of vertices</param>
        /// <returns> distance from start vertex </returns>
        protected override Vertex MinDist(List<Vertex> list)
        {
            Graph.Vertex closest = null;
            float minDistance = float.MaxValue;
            foreach (var vertex in list)    // loop through each vertex in list
            {
                if (vertex.CostFromStart < minDistance) // compare to minimum distance found
                {
                    minDistance = vertex.CostFromStart; // update minimum distance
                    closest = vertex;                   // update closest vertex
                }
            }
            return closest;                             // return closest vertex in list
        }
    }
}
