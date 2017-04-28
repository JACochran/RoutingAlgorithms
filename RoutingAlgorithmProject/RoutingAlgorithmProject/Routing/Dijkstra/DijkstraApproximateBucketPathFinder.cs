using System;
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
            public override List<Vertex> FindShortestPath(Coordinates start, Coordinates end)
        {
            Vertex startVertex = FindClosestVertex(start);
            Vertex destinationVertex = FindClosestVertex(end);
            List<Vertex> vertexList = this.graph.Verticies;
            int size = vertexList.Count();
            //Dictionary<Vertex, float> dist = new Dictionary<Vertex, float>();
            Models.PriorityQueues.ApproximateBucketQueue<Vertex> q = new Models.PriorityQueues.ApproximateBucketQueue<Vertex>();
            for (int i = 0; i < size; i++)
            {
                Vertex node = vertexList[i];
                float distance = 0;
                if (!vertexList[i].Equals(startVertex))
                    distance = Models.PriorityQueues.ApproximateBucketQueue<Vertex>.MaxDistance;
                node.CostFromStart = distance;
                vertexList[i].Previous = null;
                q.Enqueue(node, distance);
            }

            while (q.Count > 0)
            {
                Vertex node = q.Dequeue();
                if (node.Equals(destinationVertex))
                    return GetPathResult(destinationVertex);
                foreach (var neighbor in node.Neighbors)
                {
                    float newDistance = neighbor.Value.Weight + node.CostFromStart;
                    if (q.Contains(neighbor.Key) && newDistance < neighbor.Key.CostFromStart)
                    {
                        neighbor.Key.CostFromStart = newDistance;
                        neighbor.Key.Previous = node;
                        q.UpdatePriority(neighbor.Key, newDistance);
                    }
                }
            }
            return null;
        }
              
    }
}
