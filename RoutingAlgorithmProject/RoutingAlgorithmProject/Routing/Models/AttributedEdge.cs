using System;
using System.Collections.Generic;

namespace RoutingAlgorithmProject.Routing.Models
{
    public class AttributedEdge
    {
    /**
     * Constructor
     *
     * @param edgeIdentifier
     *             Unique edge identifier
     * @param edgeAttributes
     *             All or a subset of this edge's attributes
     * @param fromNode
     *             Unique 'from' node identifier, and attributes
     * @param toNode
     *             Unique 'to' node identifier, and attributes
     */ 
    public AttributedEdge( int edgeIdentifier,
                           List<Object> edgeAttributes,
                           AttributedNode fromNode,
                           AttributedNode toNode)
        {
            EdgeIdentifier = edgeIdentifier;
            _edgeAttributes = edgeAttributes ?? new List<object>();
            FromNode = fromNode;
            ToNode = toNode;
        }

        public IReadOnlyList<Object> getEdgeAttributes()
        {
            return _edgeAttributes.AsReadOnly();
        }


        public  int EdgeIdentifier { get; }
        private List<Object>   _edgeAttributes;
        public  AttributedNode FromNode { get; }
        public  AttributedNode ToNode { get; }
}

}
