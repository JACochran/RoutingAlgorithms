using System;
using System.Collections.Generic;

namespace RoutingAlgorithmProject.Routing.Models
{
    public class AStarVertex : IComparable
    {
        public AStarVertex Previous { get; internal set; }
        public List<object> EdgeAttributes { get; internal set; }
        public int EdgeIdentifier { get; internal set; }
        public double EdgeCost { get; internal set; }
        public double EstimatedCostToEnd { get; set; }
        public double CostFromStart { get; set; }
        public AttributedNode Node { get; internal set; }

        public int CompareTo(object obj)
        {
            var vertex2 = obj as AStarVertex;
            if (obj == null)
            {
                throw new ArgumentException("cannot compare these two types");
            }

            Double cost = EstimatedCostToEnd + CostFromStart;
            return cost.CompareTo(vertex2.EstimatedCostToEnd + vertex2.CostFromStart);
        }


        public AStarVertex(AttributedNode node)
        {
            this.node = node;
        }

        public AStarVertex(AttributedNode node,
                     double costFromStart,
                     double estimatedCostToEnd)
        {
            this.node = node;
            this.costFromStart = costFromStart;
            this.estimatedCostToEnd = estimatedCostToEnd;
        }

        public override bool Equals(Object obj)
        {
            return obj == this ||
                   !(obj == null || GetType() != obj.GetType()) && node == ((AStarVertex)obj).node;

        }

        public override int GetHashCode()
        {
            return node.Identifier;
        }

        /**
         * @return the node
         */
        AttributedNode getNode()
        {
            return node;
        }

        /**
         * @return the costFromStart
         */
        double getCostFromStart()
        {
            return costFromStart;
        }

        /**
         * @return the estimatedCostToEnd
         */
        double getEstimatedCostToEnd()
        {
            return estimatedCostToEnd;
        }

        /**
         * @return the previous
         */
        AStarVertex getPrevious()
        {
            return previous;
        }

        double getEdgeCost()
        {
            return edgeCost;
        }

        int getEdgeIdentifier()
        {
            return edgeIdentifier;
        }

        IReadOnlyCollection<Object> GetEdgeAttributes()
        {
            var edgeAttributes = new List<Object>() { this.edgeAttributes };
            return edgeAttributes.AsReadOnly();
        }

        public void Update(double costFromStart,
                     double estimatedCostToEnd,
                     AStarVertex previous,
                     int edgeIdentifier,
                     IReadOnlyList<Object> edgeAttributes,
                     double edgeCost)
        {
            if (costFromStart < 0.0)
            {
                throw new ArgumentException("Distance from start may not be less than 0");
            }

            if (estimatedCostToEnd < 0.0)
            {
                throw new ArgumentException("Distance from end may not be less than 0");
            }

            this.costFromStart = costFromStart;
            this.estimatedCostToEnd = estimatedCostToEnd;
            this.previous = previous;
            this.edgeIdentifier = edgeIdentifier;
            this.edgeAttributes = edgeAttributes;
            this.edgeCost = edgeCost;
        }

        private AttributedNode node;

        private double costFromStart = Double.MaxValue;
        private double estimatedCostToEnd = Double.MaxValue;
        private AStarVertex previous;                              // Parent node
        private int edgeIdentifier;                        // Unique identifier of the edge taken to get to this node
        private List<Object> edgeAttributes;                        // Attributes of edge with the edge being parent to this one
        private double edgeCost;                              // Calculated cost of parent node to this one

    }
}
