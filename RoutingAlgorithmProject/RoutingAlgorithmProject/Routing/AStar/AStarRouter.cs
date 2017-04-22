using RoutingAlgorithmProject.Routing.Models;
using System;
using System.Collections.Generic;

namespace RoutingAlgorithmProject.Routing.AStar
{
    class AStarRouter
    {

        public static Route GetAStartPath(int endIdentifier, Dictionary<int, Vertex> nodeMap)
        {
            LinkedList<List< Object >> nodesAttributes = new LinkedList<List<Object>>();
            LinkedList<List< Object >> edgesAttributes = new LinkedList<List<Object>>();
            LinkedList<int>      edgeIdentifiers = new LinkedList<int>();
            LinkedList<Double>       edgeCosts       = new LinkedList<Double>();

            for (Vertex vertex = nodeMap[endIdentifier]; vertex != null; vertex = vertex.Previous)
            {
                nodesAttributes.AddFirst(vertex.Node.Attributes);

                if (vertex.Previous != null)
                {
                    edgesAttributes.AddFirst(vertex.EdgeAttributes);
                    edgeIdentifiers.AddFirst(vertex.EdgeIdentifier);
                    edgeCosts.AddFirst(vertex.EdgeCost);
                }
            }

            return new Route(nodesAttributes,
                             edgesAttributes,
                             edgeIdentifiers,
                             edgeCosts);

        }
    }
}
