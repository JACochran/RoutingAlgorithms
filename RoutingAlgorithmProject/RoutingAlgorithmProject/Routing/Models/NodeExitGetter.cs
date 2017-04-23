using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace RoutingAlgorithmProject.Routing.Models
{
    public class NodeExitGetter
    {
        /**
         * Constructor
         *
         * @param nodeAttributeDescriptions
         *         Collection of attributes that will be retrieved for the
         *         edge's two endpoints
         * @param edgeAttributeDescriptions
         *         Collection of attributes that will be retrieved for the
         *         edge
         * @
         *         if there is a database error
         *         
         *         
         *         Connection databaseConnection,
                                 Network network,
                                 Collection<AttributeDescription> nodeAttributeDescriptions,
                                 Collection<AttributeDescription> edgeAttributeDescriptions
         *         
         */
        protected NodeExitGetter(Collection<AttributeDescription> nodeAttributeDescriptions,
                                 Collection<AttributeDescription> edgeAttributeDescriptions)
        {
            //super(databaseConnection,
            //      ()-> {
            //    StringBuilder edgeAttributes = new StringBuilder();

            //    if (edgeAttributeDescriptions != null)
            //    {
            //        for (AttributeDescription attribute : edgeAttributeDescriptions)
            //        {
            //            edgeAttributes.append(String.format(", edge.%s", attribute.getName()));
            //        }
            //    }

            //    StringBuilder nodeAttributes = new StringBuilder();

            //    if (nodeAttributeDescriptions != null)
            //    {
            //        for (AttributeDescription attribute : nodeAttributeDescriptions)
            //        {
            //            nodeAttributes.append(String.format(", f.%s", attribute.getName()));  // from_node attributes, 'f'
            //        }

            //        for (AttributeDescription attribute : nodeAttributeDescriptions)
            //        {
            //            nodeAttributes.append(String.format(", t.%s", attribute.getName())); // to_node attributes, 't'
            //        }
            //    }

            //    return String.format("SELECT edge.id, edge.to_node%1$s%2$s\n" +
            //                                 "FROM %3$s as f,\n" +
            //                                 "     %3$s as t,\n" +
            //                                 "     (SELECT id, to_node%1$s\n" +
            //                                 "      FROM %4$s\n" +
            //                                 "      WHERE from_node = ?) as edge\n" +
            //                                 "WHERE f.node_id = ? AND t.node_id = to_node;",
            //                         edgeAttributes.toString(),                                       // %1$s additional requested edge attributes to query for
            //                         nodeAttributes.toString(),                                       // %2$s requested node attributes to query for
            //                         GeoPackageNetworkExtension.getNodeAttributesTableName(network),  // %3$s node attribute table name
            //                         network.getTableName());                                         // %4$s network table name;
            //});

            int edgeAttributeCount = edgeAttributeDescriptions == null ? 0 : edgeAttributeDescriptions.Count;
            int nodeAttributeCount = nodeAttributeDescriptions == null ? 0 : nodeAttributeDescriptions.Count;

            this.firstEdgeAttributeColumn = 3;  // Edge attributes always start after edge.id and edge.to_node, and column indices are 1-based
            this.lastEdgeAttributeColumn = this.firstEdgeAttributeColumn + edgeAttributeCount;

            this.firstFromNodeAttributeColumn = edgeAttributeCount == 0 ? this.firstEdgeAttributeColumn : this.lastEdgeAttributeColumn + 1;    // From node attributes follow edge attributes
            this.lastFromNodeAttributeColumn = this.firstFromNodeAttributeColumn + nodeAttributeCount - 1;

            this.firstToNodeAttributeColumn = nodeAttributeCount == 0 ? this.firstFromNodeAttributeColumn : this.lastFromNodeAttributeColumn + 1;  // To node attributes follow edge attributes
            this.lastToNodeAttributeColumn = this.firstToNodeAttributeColumn + nodeAttributeCount - 1;
        }

        /**
         * Returns a collection of edge that represent exits from the supplied node
         *
         * @param fromNodeIdentifier
         *             'From' node identifier
         * @return a collection of edge that represent exits from the supplied node
         * @
         *             if there is a database error
         */
        public List<AttributedEdge> getExits(int fromNodeIdentifier)
        {
            //this.fromNodeIdentifier = fromNodeIdentifier;

            //this.getPreparedStatement().setInt(1, fromNodeIdentifier);
            //this.getPreparedStatement().setInt(2, fromNodeIdentifier);

            //return this.execute();
        }
        
    protected List<AttributedEdge> processResult(ResultSet resultSet)
        {
            //return JdbcUtility.map(resultSet,
            //                       results-> new AttributedEdge(results.getInt(1),   // edge identifier
            //                                                     NodeExitGetter.this.getEdgeAttributes(results),
            //                                                     new AttributedNode(this.fromNodeIdentifier,
            //                                                                        NodeExitGetter.this.getFromNodeAttributes(results)),
            //                                                     new AttributedNode(resultSet.getInt(2),
            //                                                                        NodeExitGetter.this.getToNodeAttributes(results))));
        }

        protected List<Object> getEdgeAttributes(ResultSet resultSet)
        {
            //return this.firstEdgeAttributeColumn < this.lastEdgeAttributeColumn ? JdbcUtility.getObjects(resultSet, this.firstEdgeAttributeColumn, this.lastEdgeAttributeColumn)
            //                                                                    : null;
        }

        protected List<Object> getFromNodeAttributes(ResultSet resultSet)
        {
            //return this.firstFromNodeAttributeColumn < this.lastFromNodeAttributeColumn ? JdbcUtility.getObjects(resultSet, this.firstFromNodeAttributeColumn, this.lastFromNodeAttributeColumn)
            //                                                                            : null;
        }

        protected List<Object> getToNodeAttributes(ResultSet resultSet)
        {
            //return this.firstToNodeAttributeColumn < this.lastToNodeAttributeColumn ? JdbcUtility.getObjects(resultSet, this.firstToNodeAttributeColumn, this.lastToNodeAttributeColumn)
            //                                                                        : null;
        }

        private int firstEdgeAttributeColumn;
        private int lastEdgeAttributeColumn;
        private int firstFromNodeAttributeColumn;
        private int lastFromNodeAttributeColumn;
        private int firstToNodeAttributeColumn;
        private int lastToNodeAttributeColumn;

        private int fromNodeIdentifier;
    }

}
