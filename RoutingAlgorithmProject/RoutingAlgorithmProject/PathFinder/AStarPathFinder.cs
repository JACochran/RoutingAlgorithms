using System;
using System.Collections.Generic;
using System.Linq;
using RoutingAlgorithmProject.Graph;
using System.Collections.ObjectModel;
using Esri.ArcGISRuntime.Geometry;
using RoutingAlgorithmProject.Routing.Models;

namespace RoutingAlgorithmProject.PathFinder
{
    class AStarPathFinder : PathFinder
    {
        public AStarPathFinder(Graph.Graph graph) : base(graph)
        {
        }

        private double GetMinDistance(MapPoint start, MapPoint end)
        {
            return GeometryEngine.GeodesicDistance(start, end, LinearUnits.Meters);
        }

        public override List<Edge> FindShortestPath(MapPoint start, MapPoint end)
        {
            SortedList<AStarVertex, double> openList = new SortedList<AStarVertex, double>();
            Collection<AStarVertex> closedList = new Collection<AStarVertex>();//new HashSet<>((this.restrictedNodeIdentifiers == null) ? new Collection<int>() : this.restrictedNodeIdentifiers);
            var nodeMap = new HashSet<AStarVertex>();
            
            var startNode = FindClosestVertex(start);
            var endNode   = FindClosestVertex(end);
           

            // Starting Vertex
            var startVertex = new AStarVertex(startNode.Coordinates,
                                              0.0,
                                              GetMinDistance(start, end));

            nodeMap.Add(startVertex);

            for (AStarVertex currentVertex = startVertex; currentVertex != null && openList.Keys.Count > 0; currentVertex = openList.Keys[0])
            {
                // If current vertex is the target then we are done
                if (currentVertex.Coordinates.IsEqual(end))
                {
                    return GetAStarPath(end, nodeMap).ToList();
                }

                closedList.Add(currentVertex); // Put it in "done" pile

                foreach (var exit in  currentVertex.AStarNeighbors) // For each node adjacent to the current node
                {
                    // Ignore restricted edges
                    //if (!this.restrictedEdgeIdentifiers.Contains(exit.EdgeIdentifier))
                    AStarVertex reachableVertex = null;
                   var hasBeenVisited = nodeMap.Contains(exit.Key);

                   if (hasBeenVisited == false)
                   {
                       reachableVertex = new AStarVertex(exit.Key.Coordinates, double.MaxValue, double.MaxValue);
                       nodeMap.Add(reachableVertex);
                   }

                   // If the closed list already searched this vertex, skip it
                   if (!closedList.Contains(exit.Key))
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
                           double estimatedCostFromEnd = exit.Key.Coordinates.IsEqual(endNode.Coordinates) ? 0.0
                                                                                                           : GetMinimumDistance(reachableVertex.Coordinates, endNode.Coordinates);

                           reachableVertex.Update(costFromStart,
                                                  estimatedCostFromEnd,
                                                  currentVertex,
                                                  edgeCost);

                           if (isShorterPath)
                           {
                               openList.Remove(reachableVertex);   // Re-add to trigger the reprioritization of this vertex
                           }

                           openList.Add(reachableVertex, costFromStart + estimatedCostFromEnd);
                       }
                   }
                 
                }
            }

            return null;    // No path between the start and end nodes
        }

        public static LinkedList<Edge> GetAStarPath(MapPoint endLocation, HashSet<AStarVertex> nodeMap)
        {
            LinkedList<Edge> path = new LinkedList<Edge>();
            LinkedList<Double> edgeCosts = new LinkedList<Double>();

            for (var vertex = nodeMap.First(aStarVertex => aStarVertex.Coordinates.IsEqual(endLocation)); vertex != null; vertex = vertex.Previous)
            {
                //nodesAttributes.AddFirst(vertex.Node.GetAttributes());

                if (vertex.Previous != null)
                {
                    path.AddFirst(FindEdge(vertex, vertex.Previous));
                    edgeCosts.AddFirst(vertex.EdgeCost);
                }
            }

            return path;

        }
    }
}
