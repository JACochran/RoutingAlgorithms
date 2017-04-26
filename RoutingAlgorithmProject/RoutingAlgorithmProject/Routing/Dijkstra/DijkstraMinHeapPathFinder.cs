using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoutingAlgorithmProject.Graph;

namespace RoutingAlgorithmProject.Routing
{
    class DijkstraMinHeapPathFinder : DijkstraPathFinder
    {
        public DijkstraMinHeapPathFinder(RoutingGraph graph) : base(graph)
        {
        }

        public override List<Vertex> FindShortestPath(Coordinates start, Coordinates end)
        {
            Vertex startVertex = FindClosestVertex(start);
            Vertex destinationVertex = FindClosestVertex(end);
            List<Vertex> vertexList = this.graph.Verticies;
            int size = vertexList.Count();
            //Dictionary<Vertex, float> dist = new Dictionary<Vertex, float>();
            Dictionary<Vertex, VertexNode> heapNodeIndex = new Dictionary<Vertex, VertexNode>(); // need to keep track of node index
            Models.PriorityQueues.MinKHeap<VertexNode> q = new Models.PriorityQueues.MinKHeap<VertexNode>(this.graph.Verticies.Count);
            for (int i = 0; i < size; i++)
            {
                VertexNode node = new VertexNode(vertexList[i]);
                float distance = 0;
                if (!vertexList[i].Equals(startVertex))
                   distance = Int32.MaxValue;
                node.Vertex.CostFromStart = distance;
                heapNodeIndex[node.Vertex] = node;
                vertexList[i].Previous = null;
                q.Enqueue(node, distance);
            }

            while (q.Count > 0)
            {
                VertexNode node = q.Dequeue(); //MinDist(q, dist);
                if (node.Vertex.Equals(destinationVertex))
                    return GetPathResult(destinationVertex);
                foreach (var neighbor in node.Vertex.Neighbors)
                {
                    float newDistance = neighbor.Value.Weight + node.Vertex.CostFromStart;
                    if (q.Contains(heapNodeIndex[neighbor.Key]) && newDistance < neighbor.Key.CostFromStart)
                    {
                        neighbor.Key.CostFromStart = newDistance;
                        neighbor.Key.Previous = node.Vertex;
                        q.UpdatePriority(heapNodeIndex[neighbor.Key], newDistance);
                    }
                }
            }
            return null;
        }

        private class VertexNode : Models.PriorityQueues.MinKHeapNode
        {
            public Vertex Vertex;

            public VertexNode(Vertex v)
            {
                this.Vertex = v;
            }
        }
        
    }


}
