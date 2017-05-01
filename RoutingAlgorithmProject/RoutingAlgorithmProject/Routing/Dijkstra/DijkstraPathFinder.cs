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
        
        public override List<Vertex> FindShortestPath(Vertex startNode, Vertex endNode, ref float pathLength)
        {
            
            List<Vertex> vertexList = this.graph.Verticies;
            HashSet<Vertex> q = new HashSet<Vertex>();
            for (int i = 0; i < vertexList.Count(); i++)
            {
                if (vertexList[i].Equals(startNode))
                    vertexList[i].CostFromStart = 0;
                else
                    vertexList[i].CostFromStart = Int32.MaxValue;
                vertexList[i].Previous = null;
                q.Add(vertexList[i]);
            }

            while (q.Count > 0)
            {
                Vertex u = MinDist(q);
                if (u.Equals(endNode))
                    return GetPathResult(endNode, ref pathLength);
                q.Remove(u);
                foreach (var neighbor in u.Neighbors)
                {
                    var newDistance = u.CostFromStart + neighbor.Value.Weight;
                    if (newDistance < neighbor.Key.CostFromStart)
                    {
                        neighbor.Key.CostFromStart = newDistance;
                        neighbor.Key.Previous = u;
                    }
                }
            }
            return null;
        }

        public override string GetAbbreivatedName()
        {
            return "DIKL";
        }

        private Vertex MinDist(HashSet<Vertex> q)
        {
            Graph.Vertex closest = null;
            float minDistance = float.MaxValue;
            foreach (var vertex in q)
            {
                if (vertex.CostFromStart < minDistance)
                {
                    minDistance = vertex.CostFromStart;
                    closest = vertex;
                }
            }
            return closest;
        }
    }
}
