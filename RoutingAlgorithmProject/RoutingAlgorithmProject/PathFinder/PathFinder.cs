using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutingAlgorithmProject.PathFinder
{
    public abstract class PathFinder
    {
        protected Graph.Graph graph;

        public PathFinder(Graph.Graph graph)
        {
            this.graph = graph;
        }

        public abstract List<Graph.Edge> FindShortestPath(Graph.Coordinates start, Graph.Coordinates end);

        protected Graph.Vertex FindClosestVertex(Graph.Coordinates point)
        {
            Graph.Vertex closest = null;
            float minDistance = float.MaxValue;
            foreach(var vertex in graph.Verticies)
            {
                var distance = Graph.Edge.DistanceBetweenPoints(point, vertex.Coordinates);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = vertex;
                }
            }
            return closest;
        }
           
    }


}
