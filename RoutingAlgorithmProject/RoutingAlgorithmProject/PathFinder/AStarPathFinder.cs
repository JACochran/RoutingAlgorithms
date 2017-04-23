using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoutingAlgorithmProject.Graph;

namespace RoutingAlgorithmProject.PathFinder
{
    class AStarPathFinder : PathFinder
    {
        public AStarPathFinder(Graph.Graph graph) : base(graph)
        {
        }

        public override List<Edge> FindShortestPath(Coordinates start, Coordinates end)
        {
            throw new NotImplementedException();
        }
    }
}
