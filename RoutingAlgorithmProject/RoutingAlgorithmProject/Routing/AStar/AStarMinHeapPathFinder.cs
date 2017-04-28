﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoutingAlgorithmProject.Graph;
using System.Collections.ObjectModel;

namespace RoutingAlgorithmProject.Routing
{
    class AStarMinHeapPathFinder: AStarPathFinder
    {
        public AStarMinHeapPathFinder(RoutingGraph graph) : base(graph)
        {
        }

        public override List<Vertex> FindShortestPath(Coordinates start, Coordinates end)
        {
            try
            {
                Models.PriorityQueues.MinKHeap<Vertex> openList = new Models.PriorityQueues.MinKHeap<Vertex>(this.graph.Verticies.Count);
                HashSet<Vertex> closedList = new HashSet<Vertex>();

               // var nodeMap = new HashSet<Vertex>();

                var startNode = FindClosestVertex(start);
                var endNode = FindClosestVertex(end);

                startNode.Update(0.0f, Graph.Edge.GetMinimumDistance(startNode.Coordinates, endNode.Coordinates), null);
               // nodeMap.Add(startNode);
                var currentVertex = startNode;
                while (currentVertex != null)
                {
                    closedList.Add(currentVertex); // Put it in "done" pile
                    //// If current vertex is the target then we are done
                    if (currentVertex.Equals(endNode))
                    {
                        return GetPathResult(endNode);
                    }

                    

                    foreach (var exit in currentVertex.Neighbors) // For each node adjacent to the current node
                    {
                        Vertex reachableVertex = exit.Key;

                        // If the closed list already searched this vertex, skip it
                        if (!closedList.Contains(reachableVertex))
                        {
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

                                reachableVertex.Update(costFromStart,estimatedCostFromEnd,currentVertex);

                                if (!openList.Contains(reachableVertex))
                                    openList.Enqueue(reachableVertex, estimatedCostFromEnd + costFromStart);
                                else
                                    openList.UpdatePriority(reachableVertex, estimatedCostFromEnd + costFromStart);
                            }
                        }

                    }

                    if (openList.Count > 0)
                    {
                        currentVertex = openList.Dequeue();                        
                    }
                    else
                    {
                        currentVertex = null;
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return null;    // No path between the start and end nodes
        }

    }
}
