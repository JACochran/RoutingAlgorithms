﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoutingAlgorithmProject.Graph;

namespace RoutingAlgorithmProject.Routing
{
    class DijkstraApproximateBucketPathFinder : DijkstraPathFinder
    {
        public DijkstraApproximateBucketPathFinder(RoutingGraph graph) : base(graph)
        {
        }
            public override List<Vertex> FindShortestPath(Vertex startNode, Vertex endNode, ref float pathLength)
        {
            List<Vertex> vertexList = this.graph.Verticies;
            int size = vertexList.Count();
            //Dictionary<Vertex, float> dist = new Dictionary<Vertex, float>();
            Models.PriorityQueues.ApproximateBucketQueue<Vertex> q = new Models.PriorityQueues.ApproximateBucketQueue<Vertex>();
            for (int i = 0; i < size; i++)
            {
                Vertex node = vertexList[i];
                if (vertexList[i].Equals(startNode))
                    node.CostFromStart = 0;
                vertexList[i].Previous = null;
                q.Enqueue(node, node.CostFromStart);
            }

            while (q.Count > 0)
            {
                Vertex node = q.Dequeue();
                if (node.Equals(endNode))
                    return GetPathResult(endNode, ref pathLength);
                foreach (var neighbor in node.Neighbors)
                {
                    float newDistance = neighbor.Value.Weight + node.CostFromStart;
                    if(newDistance < neighbor.Key.CostFromStart)
                    {
                        neighbor.Key.CostFromStart = newDistance;

                        if (q.Contains(neighbor.Key))
                        {
                            neighbor.Key.Previous = node;
                            q.UpdatePriority(neighbor.Key, newDistance);
                        }
                    }
                }
            }
            return null;
        }

        public override string GetAbbreivatedName()
        {
            return "DIKBA";
        }
    }
}
