using System;
using System.Collections.Generic;

namespace RoutingAlgorithmProject.Routing.Models
{
    public class AttributedNode
    {
        /**
         * Constructor
         *
         * @param identifier
         *             Unique node identifier
         * @param attributeValues
         *             Some or all of the nodes attributes
         */
        public AttributedNode( int identifier,
                               List<Object> attributeValues)
        {
            Identifier = identifier;
            _attributeValues = attributeValues == null ? new List<Object>() : attributeValues;
        }
        

        public IReadOnlyList<Object> GetAttributes()
        {
            return _attributeValues.AsReadOnly();
        }

        /**
         * Gets the node's attribute as described by the supplied index
         *
         * @param attributeIndex
         *             Index of the attribute to retrieve. These values correspond
         *             to order in which the attributes were requested.
         * @return value at the specified index
         */
        public Object GetAttribute( int attributeIndex)
        {
            return _attributeValues[attributeIndex];
        }

        public int Identifier { get; }
        private  List<Object> _attributeValues; // TODO This should be Map<AttributeDescription, Object>, but I'm concerned about performance.  Once we're happy with performance numbers in routing, we should make this change and see what, if any, performance impact it has.
    }

}
