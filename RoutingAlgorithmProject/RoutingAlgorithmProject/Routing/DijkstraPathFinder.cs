using System;
using System.Collections.Generic;
using System.Linq;
using RoutingAlgorithmProject.Graph;

namespace RoutingAlgorithmProject.PathFinder
{
    class DijkstraPathFinder : PathFinder<Vertex>
    {
        private RoutingGraph<Vertex> routingGraph;

        public DijkstraPathFinder(RoutingGraph<Vertex> graph) : base(graph)
        {
        }
        
        public override List<Edge> FindShortestPath(Coordinates start, Coordinates end)
        {
            Vertex startVertex = FindClosestVertex(start);
            Vertex destinationVertex = FindClosestVertex(end);
            List<Vertex> vertexList = this.graph.Verticies;
            int size = vertexList.Count();
            Dictionary<Vertex, float> dist = new Dictionary<Vertex, float>();
            Dictionary<Vertex, Graph.Vertex> prev = new Dictionary<Vertex, Vertex>();
            HashSet<Vertex> q = new HashSet<Vertex>();
            for (int i = 0; i < size; i++)
            {
                if (vertexList[i].Equals(startVertex))
                    dist[vertexList[i]] = 0;
                else
                    dist[vertexList[i]] = Int32.MaxValue;
                prev[vertexList[i]] = null;
                q.Add(vertexList[i]);
            }

            while (q.Count > 0)
            {
                Vertex u = MinDist(q, dist);
                if (u.Equals(destinationVertex))
                    break;
                q.Remove(u);
                foreach (var neighbor in u.Neighbors)
                {
                    var alt = dist[u] + neighbor.Value.Weight;
                    if (alt < dist[neighbor.Key])
                    {
                        dist[neighbor.Key] = alt;
                        prev[neighbor.Key] = u;
                    }
                }
            }
            return GetPathResult(prev, destinationVertex);
        }

        private List<Edge> GetPathResult(Dictionary<Vertex, Vertex> prev, Vertex target)
        {
            List<Edge> path = new List<Edge>();
            Vertex u = target;
            Edge e = null;
            while (prev[u] != null)
            {
                path.Insert(0, FindEdge(prev[u], u));
                u = prev[u];
            }
            return path;

        }

        private Vertex MinDist(HashSet<Vertex> q, Dictionary<Vertex, float> dist)
        {
            Graph.Vertex closest = null;
            float minDistance = float.MaxValue;
            foreach (var vertex in q)
            {
                if (dist[vertex] < minDistance)
                {
                    minDistance = dist[vertex];
                    closest = vertex;
                }
            }
            return closest;
        }
    }
}
