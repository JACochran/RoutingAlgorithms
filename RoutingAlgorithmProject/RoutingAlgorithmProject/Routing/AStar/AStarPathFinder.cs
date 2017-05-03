using System;
using System.Collections.Generic;
using System.Linq;
using RoutingAlgorithmProject.Graph;
using System.Collections.ObjectModel;

namespace RoutingAlgorithmProject.Routing
{
    class AStarPathFinder : PathFinder
    {
        public AStarPathFinder(RoutingGraph graph) : base(graph)
        {
        }

        public override List<Vertex> FindShortestPath(Vertex startNode, Vertex endNode, ref float pathLength)
        {
            HashSet<Vertex> closedList = new HashSet<Vertex>();         // set of nodes that have been evaluated
            List<Vertex> openList = new List<Vertex>();                 // list of nodes to evaluate
            startNode.CostFromStart = 0;                                // set start node g(x) = 0
            startNode.EstimatedCostToEnd = Edge.GetMinimumDistance(startNode.Coordinates, endNode.Coordinates); // set h(x) 
            openList.Add(startNode);                                    // add start node to open list
            while (openList.Count > 0)                                  // loop until openList is empty
            {
                Vertex currentNode = MinDist(openList);                 // find cloest node in open list
                if (currentNode.Equals(endNode))                        // if node is destination node return path
                    return GetPathResult(endNode, ref pathLength);      // remove node from open list
                openList.Remove(currentNode);                           
                closedList.Add(currentNode);                            // add node to evaluated list
                foreach (var neighbor in currentNode.Neighbors)         // check each adjacent node
                {
                    if (!closedList.Contains(neighbor.Key))             // skip node if it has already been evaluated
                    {
                        float newCostFromStart = currentNode.CostFromStart + neighbor.Value.Weight;    // find new g(x)
                        if (!openList.Contains(neighbor.Key))           // if neighbor is not in open list then add it
                            openList.Add(neighbor.Key);
                        if(newCostFromStart < neighbor.Key.CostFromStart)   // if new g(x) < old g(x)
                        {
                            // get new h(x)
                            float estimatedCostToEnd = Edge.GetMinimumDistance(neighbor.Key.Coordinates, endNode.Coordinates);
                            neighbor.Key.CostFromStart = newCostFromStart;          // update g(x)
                            neighbor.Key.EstimatedCostToEnd = estimatedCostToEnd;   // update h(x)
                            neighbor.Key.Previous = currentNode;                    // set previous to current node
                        }
                    }
                }
            }
            return null;    // No path between the start and end nodes
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
                float fScore = vertex.CostFromStart + vertex.EstimatedCostToEnd;
                if (fScore < minDistance) // compare to minimum distance found
                {
                    minDistance = fScore; // update minimum distance
                    closest = vertex;                   // update closest vertex
                }
            }
            return closest;                             // return closest vertex in list
        }

        public override string GetAbbreivatedName()
        {
            return "ASL";
        }
    }
}
