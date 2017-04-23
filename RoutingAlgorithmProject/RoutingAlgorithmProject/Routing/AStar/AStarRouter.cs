using RoutingAlgorithmProject.Routing.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RoutingAlgorithmProject.Routing.AStar
{
    public class AStarRouter
    {
    /**
     * Constructor
     *
     * @param routingExtension
     *            Handle to a GeoPackage's routing extension
     * @param routingNetwork
     *            Network on which to route between a start and end node
     * @param nodeAttributeDescriptions
     *            Attributes of each network node to query for. These
     *            attributes will be passed to the edge cost evaluator via
     *            {@link AttributedNode#getAttributes()} as an array of {@link Object}s
     *            in the <i>in the order in which the {@link
     *            AttributeDescription}s are specified</i>.
     * @param edgeAttributeDescriptions
     *            Attributes of each network edge to query for. These
     *            attributes will be passed to the edge cost evaluator via
     *            {@link AttributedEdge#getEdgeAttributes()} as an
     *            array of {@link Object}s in the <i>in the order in which the
     *            {@link AttributeDescription}s are specified</i>.
     * @param edgeCostEvaluator
     *            Cost function for each edge in the path
     * @param heuristic
     *            Cost heuristic function to be applied between a intermediate
     *            and end node to determine the search order of A*
     * @param restrictedNodeIdentifiers
     *            Collection of nodes to not consider in routing
     * @param restrictedEdgeIdentifiers
     *            Collection of edges to not consider in routing
     * @throws SQLException
     *            if there is a database error
     */
    public AStarRouter(Collection<AttributeDescription> nodeAttributeDescriptions,
                       Collection<AttributeDescription> edgeAttributeDescriptions,
                       Func<AttributedEdge, Double>                   edgeCostEvaluator,
                       Func<AttributedNode, AttributedNode, Double> heuristic,
                       Collection<int> restrictedNodeIdentifiers = null,
                       Collection<int> restrictedEdgeIdentifiers = null) : base(nodeAttributeDescriptions,
                                                                                edgeAttributeDescriptions,
                                                                                edgeCostEvaluator,
                                                                                restrictedNodeIdentifiers,
                                                                                restrictedEdgeIdentifiers)
    {

        if(heuristic == null)
        {
            throw new ArgumentException("Heuristic function may not be null");
        }

        this.cachedHeuristic = new Memoize2<AttributedNode, AttributedNode, double>(heuristic);

    }



    private Memoize2<AttributedNode, AttributedNode, Double> cachedHeuristic;
        private NodeExitGetter                                   edgeGetter;

        
        public Route Route(int startNodeIdentifier,
                           int endNodeIdentifier)
        {
           
            SortedList<AStarVertex, double> openList = new SortedList<AStarVertex, double>();
            Collection<int> closedList = new Collection<int>();//new HashSet<>((this.restrictedNodeIdentifiers == null) ? new Collection<int>() : this.restrictedNodeIdentifiers);
            Dictionary<int, AStarVertex> nodeMap = new Dictionary<int, AStarVertex>();

            AttributedNode startNode = this.networkExtension.getAttributedNode(startNodeIdentifier, this.nodeAttributeDescriptions);
            AttributedNode endNode = this.networkExtension.getAttributedNode(endNodeIdentifier, this.nodeAttributeDescriptions);

            // Starting Vertex
            AStarVertex startVertex = new AStarVertex(startNode,
                                            0.0,
                                            this.cachedHeuristic.Get(startNode, endNode));

            nodeMap.Add(startNodeIdentifier, startVertex);

            for (AStarVertex currentVertex = startVertex; currentVertex != null && openList.Keys.Count > 0; currentVertex = openList.Keys[0])
            {
                // If current vertex is the target then we are done
                if (currentVertex.Node.Identifier == endNodeIdentifier)
                {
                    return GetAStarPath(endNodeIdentifier, nodeMap);
                }

                closedList.Add(currentVertex.Node.Identifier); // Put it in "done" pile

                foreach(AttributedEdge exit in this.edgeGetter.getExits(currentVertex.Node.Identifier)) // For each node adjacent to the current node
                {
                    // Ignore restricted edges
                    if (!this.restrictedEdgeIdentifiers.Contains(exit.EdgeIdentifier))
                    {
                        AStarVertex reachableVertex = nodeMap[exit.ToNode.Identifier];

                        if (reachableVertex == null)
                        {
                            reachableVertex = new AStarVertex(exit.ToNode);
                            nodeMap.Add(exit.ToNode.Identifier, reachableVertex);
                        }

                        // If the closed list already searched this vertex, skip it
                        if (!closedList.Contains(exit.ToNode.Identifier))
                        {
                            //double edgeCost = this.edgeCostEvaluator.Apply(exit);
                            double edgeCost = 

                            if (edgeCost <= 0.0)    // Are positive values that are extremely close to 0 going to be a problem?
                            {
                                throw new ArgumentException("The A* algorithm is only valid for edge costs greater than 0");
                            }

                            double costFromStart = currentVertex.CostFromStart + edgeCost;

                            bool isShorterPath = costFromStart < reachableVertex.CostFromStart;

                            if (!openList.ContainsKey(reachableVertex) || isShorterPath)
                            {
                                double estimatedCostFromEnd = exit.ToNode.Identifier == endNode.Identifier ? 0.0
                                                                                                           : cachedHeuristic.Get(reachableVertex.Node, endNode);

                                reachableVertex.Update(costFromStart,
                                                       estimatedCostFromEnd,
                                                       currentVertex,
                                                       exit.EdgeIdentifier,
                                                       exit.getEdgeAttributes(),
                                                       edgeCost);

                                if (isShorterPath)
                                {
                                    openList.Remove(reachableVertex);   // Re-add to trigger the reprioritization of this vertex
                                }

                                openList.Add(reachableVertex, costFromStart + estimatedCostFromEnd);
                            }
                        }
                    }
                }
            }

            return null;    // No path between the start and end nodes
        }


        public static Route GetAStarPath(int endIdentifier, Dictionary<int, AStarVertex> nodeMap)
        {
            LinkedList<IReadOnlyList<Object>> nodesAttributes = new LinkedList<IReadOnlyList<Object>>();
            LinkedList<List<Object>> edgesAttributes = new LinkedList<List<Object>>();
            LinkedList<int> edgeIdentifiers = new LinkedList<int>();
            LinkedList<Double> edgeCosts = new LinkedList<Double>();

            for (AStarVertex vertex = nodeMap[endIdentifier]; vertex != null; vertex = vertex.Previous)
            {
                nodesAttributes.AddFirst(vertex.Node.GetAttributes());

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
