using System;
using System.Collections.Generic;
using System.Linq;
using RoutingAlgorithmProject.Graph;
using System.Collections.ObjectModel;

namespace RoutingAlgorithmProject.PathFinder
{
    class AStarPathFinder : PathFinder<Vertex>
    {
        public AStarPathFinder(RoutingGraph<Vertex> graph) : base(graph)
        {
        }

        public override List<Vertex> FindShortestPath(Coordinates start, Coordinates end)
        {
            //start = new Graph.Coordinates(38.903671f, -77.000038f);
            //end = new Graph.Coordinates(38.902446f, -76.997449f);
            SortedList<Vertex, double> openList = new SortedList<Vertex, double>();
            Collection<Vertex> closedList = new Collection<Vertex>();
            var nodeMap = new HashSet<Vertex>();
            
            var startNode = FindClosestVertex(start);
            var endNode   = FindClosestVertex(end);

            startNode.Update(0.0, GetMinimumDistance(startNode.Coordinates, endNode.Coordinates), null, double.MaxValue);
            nodeMap.Add(startNode);
            var currentVertex = startNode;
            while(currentVertex != null)
            {
                // If current vertex is the target then we are done
                if (currentVertex.Coordinates.Equals(endNode.Coordinates))
                {
                    return GetAStarPath(endNode.Coordinates, nodeMap).ToList();
                }

                closedList.Add(currentVertex); // Put it in "done" pile

                foreach (var exit in  currentVertex.Neighbors) // For each node adjacent to the current node
                {
                    var reachableVertex = nodeMap.FirstOrDefault(vertex => vertex.Equals(exit.Value.To));
                    
                    if (reachableVertex == null)
                    {
                        reachableVertex = exit.Value.To;
                        nodeMap.Add(reachableVertex);
                    }

                    // If the closed list already searched this vertex, skip it
                    if (!closedList.Contains(reachableVertex))
                    {
                        //double edgeCost = this.edgeCostEvaluator.Apply(exit);
                        double edgeCost = GetMinimumDistance(exit.Value.To.Coordinates, exit.Value.From.Coordinates);

                        if (edgeCost <= 0.0)    // Are positive values that are extremely close to 0 going to be a problem?
                        {
                            throw new ArgumentException("The A* algorithm is only valid for edge costs greater than 0");
                        }

                        double costFromStart = currentVertex.CostFromStart + edgeCost;

                        bool isShorterPath = costFromStart < reachableVertex.CostFromStart;

                        if (!openList.ContainsKey(reachableVertex) || isShorterPath)
                        {
                            double estimatedCostFromEnd = exit.Key.Coordinates.Equals(endNode.Coordinates) ? 0.0
                                                                                                           : GetMinimumDistance(reachableVertex.Coordinates, endNode.Coordinates);

                            reachableVertex.Update(costFromStart,
                                                   estimatedCostFromEnd,
                                                   currentVertex,
                                                   edgeCost);

                            if (isShorterPath)
                            {
                                openList.Remove(reachableVertex);   // Re-add to trigger the reprioritization of this vertex
                            }

                            if(openList.ContainsKey(reachableVertex))
                            {
                                openList.Remove(reachableVertex); //SHould never hit but keeping for break point
                            }

                            openList.Add(reachableVertex, costFromStart + estimatedCostFromEnd);
                        }
                    }

                }

                if (openList.Count > 0)
                {
                    currentVertex = openList.Keys[0];
                    openList.RemoveAt(0);
                }
                else
                {
                    currentVertex = null;
                }
            }

            return null;    // No path between the start and end nodes
        }

        public static LinkedList<Vertex> GetAStarPath(Coordinates endLocation, HashSet<Vertex> nodeMap)
        {
            LinkedList<Vertex> path = new LinkedList<Vertex>();
            LinkedList<Double> edgeCosts = new LinkedList<Double>();
            for (var vertex = nodeMap.First(aStarVertex => aStarVertex.Coordinates.Equals(endLocation)); vertex != null; vertex = vertex.Previous)
            {
                if (vertex.Previous != null)
                {
                    path.AddFirst(vertex);
                    edgeCosts.AddFirst(vertex.EdgeCost);
                }
            }

            return path;
        }
    }
}
