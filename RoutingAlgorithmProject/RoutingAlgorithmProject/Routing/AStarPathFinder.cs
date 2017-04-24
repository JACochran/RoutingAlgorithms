﻿using System;
using System.Collections.Generic;
using System.Linq;
using RoutingAlgorithmProject.Graph;
using System.Collections.ObjectModel;

namespace RoutingAlgorithmProject.PathFinder
{
    class AStarPathFinder : PathFinder
    {
        public AStarPathFinder(RoutingGraph graph) : base(graph)
        {
        }

        public override List<Vertex> FindShortestPath(Coordinates start, Coordinates end)
        {
            try
            {
                List<Vertex> openList = new List<Vertex>();
                Collection<Vertex> closedList = new Collection<Vertex>();
                var nodeMap = new HashSet<Vertex>();

                var startNode = FindClosestVertex(start);
                var endNode = FindClosestVertex(end);

                startNode.Update(0.0f, Graph.Edge.GetMinimumDistance(startNode.Coordinates, endNode.Coordinates), null, float.MaxValue);
                nodeMap.Add(startNode);
                var currentVertex = startNode;
                while (currentVertex != null)
                {
                    // If current vertex is the target then we are done
                    if (currentVertex.Coordinates.Equals(endNode.Coordinates))
                    {
                        return GetAStarPath(endNode.Coordinates, nodeMap).ToList();
                    }

                    closedList.Add(currentVertex); // Put it in "done" pile

                    foreach (var exit in currentVertex.Neighbors) // For each node adjacent to the current node
                    {
                        Vertex reachableVertex = null;
                        reachableVertex = exit.Key;
                        if (!nodeMap.Contains(reachableVertex))
                        {
                            nodeMap.Add(reachableVertex);
                        }

                        // If the closed list already searched this vertex, skip it
                        if (!closedList.Contains(reachableVertex))
                        {
                            //double edgeCost = this.edgeCostEvaluator.Apply(exit);
                            float edgeCost = exit.Value.Weight;

                            if (edgeCost <= 0.0)    // Are positive values that are extremely close to 0 going to be a problem?
                            {
                                throw new ArgumentException("The A* algorithm is only valid for edge costs greater than 0");
                            }

                            float costFromStart = currentVertex.CostFromStart + edgeCost;

                            bool isShorterPath = costFromStart < reachableVertex.CostFromStart;

                            if (!openList.Contains(reachableVertex) || isShorterPath)
                            {
                                float estimatedCostFromEnd = exit.Key.Coordinates.Equals(endNode.Coordinates) ? 0.0f
                                                                                                               : Graph.Edge.GetMinimumDistance(reachableVertex.Coordinates, endNode.Coordinates);

                                reachableVertex.Update(costFromStart,
                                                       estimatedCostFromEnd,
                                                       currentVertex,
                                                       edgeCost);

                                if(!openList.Contains(reachableVertex))
                                    openList.Add(reachableVertex);
                            }
                        }

                    }

                    if (openList.Count > 0)
                    {
                        openList.Sort();
                        currentVertex = openList[0];
                        openList.RemoveAt(0);
                    }
                    else
                    {
                        currentVertex = null;
                    }
                }

            }catch(Exception ex)
            {

            }
            return null;    // No path between the start and end nodes
        }

        public static LinkedList<Vertex> GetAStarPath(Coordinates endLocation, HashSet<Vertex> nodeMap)
        {
            LinkedList<Vertex> path = new LinkedList<Vertex>();
            LinkedList<float> edgeCosts = new LinkedList<float>();
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
