using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RoutingAlgorithmProject.Routing.Models
{
    public abstract class Router
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
         * @param restrictedNodeIdentifiers
         *            Collection of nodes to not consider in routing
         * @param restrictedEdgeIdentifiers
         *            Collection of edges to not consider in routing
         */
        protected Router( GeoPackageRoutingExtension       routingExtension,
                          RoutingNetworkDescription        routingNetwork,
                          Collection<AttributeDescription> nodeAttributeDescriptions,
                          Collection<AttributeDescription> edgeAttributeDescriptions,
                          Func<AttributedEdge, Double> edgeCostEvaluator,
                          Collection<int>              restrictedNodeIdentifiers = null,
                          Collection<int>              restrictedEdgeIdentifiers = null)
        {
            if (routingExtension == null)
            {
                throw new ArgumentException("Routing extension may not be null");
            }

            if (routingNetwork == null)
            {
                throw new ArgumentException("Network may not be null");
            }

            if (edgeCostEvaluator == null)
            {
                throw new ArgumentException("Edge cost function may not be null");
            }

            this.routingExtension = routingExtension;
            this.networkExtension = routingExtension.getNetworkExtension();
            this.routingNetwork = routingNetwork;
            this.edgeCostEvaluator = edgeCostEvaluator;

            this.nodeAttributeDescriptions = nodeAttributeDescriptions == null ? new List<AttributeDescription>() : new List<AttributeDescription>(nodeAttributeDescriptions);
            this.edgeAttributeDescriptions = edgeAttributeDescriptions == null ? new List<AttributeDescription>() : new List<AttributeDescription>(edgeAttributeDescriptions);
            this.restrictedNodeIdentifiers = restrictedNodeIdentifiers == null ? new HashSet<int>() : new HashSet<int>(restrictedNodeIdentifiers);
            this.restrictedEdgeIdentifiers = restrictedEdgeIdentifiers == null ? new HashSet<int>() : new HashSet<int>(restrictedEdgeIdentifiers);
        }

        /**
         * This algorithm will find the route from the start node to the end node
         *
         * @param startNodeIdentifier
         *            Starting node
         * @param endNodeIdentifier
         *            Ending node
         * @return Optimal path from the start node to the end node
         * @throws SQLException
         *             if there is a database error
         */
        public abstract Route Route(int startNodeIdentifier,
                                     int endNodeIdentifier);

        protected  GeoPackageRoutingExtension   routingExtension;
        protected  GeoPackageNetworkExtension   networkExtension;
        protected  RoutingNetworkDescription    routingNetwork;
        protected  List<AttributeDescription>   nodeAttributeDescriptions;
        protected  List<AttributeDescription>   edgeAttributeDescriptions;
        protected  Func<AttributedEdge, Double> edgeCostEvaluator;
        protected  HashSet<int>                 restrictedNodeIdentifiers;
        protected  HashSet<int>                 restrictedEdgeIdentifiers;
    }

}
