using System;
using System.Collections.Generic;
using System.Linq;
using RoutingAlgorithmProject.Graph;
using System.Collections.ObjectModel;
using Esri.ArcGISRuntime.Geometry;

namespace RoutingAlgorithmProject.PathFinder
{
    class AStarPathFinder : PathFinder
    {
        public AStarPathFinder(Graph.Graph graph) : base(graph)
        {
        }

        private double GetMinDistance(MapPoint start, MapPoint end)
        {
            return GeometryEngine.GeodesicDistance(start, end, LinearUnits.Meters);
        }

        public override List<Edge> FindShortestPath(MapPoint start, MapPoint end)
        {
            SortedList<Vertex, double> openList = new SortedList<Vertex, double>();
            Collection<int> closedList = new Collection<int>();//new HashSet<>((this.restrictedNodeIdentifiers == null) ? new Collection<int>() : this.restrictedNodeIdentifiers);
            Dictionary<int, Vertex> nodeMap = new Dictionary<int, Vertex>();

            var startNode = GetClosestVertex(start);
            var endNode = GetClosestVertex(end);
           

            // Starting Vertex
            Vertex startVertex = new Vertex(startNode,
                                            0.0,
                                            GetMinDistance(start, end));

            nodeMap.Add(startNodeIdentifier, startVertex);

            for (Vertex currentVertex = startVertex; currentVertex != null && openList.Keys.Count > 0; currentVertex = openList.Keys[0])
            {
                // If current vertex is the target then we are done
                if (currentVertex.Node.Identifier == endNodeIdentifier)
                {
                    return GetAStarPath(endNodeIdentifier, nodeMap);
                }

                closedList.Add(currentVertex.Node.Identifier); // Put it in "done" pile

                foreach (AttributedEdge exit in this.edgeGetter.getExits(currentVertex.Node.Identifier)) // For each node adjacent to the current node
                {
                    // Ignore restricted edges
                    if (!this.restrictedEdgeIdentifiers.Contains(exit.EdgeIdentifier))
                    {
                        Vertex reachableVertex = nodeMap[exit.ToNode.Identifier];

                        if (reachableVertex == null)
                        {
                            reachableVertex = new Vertex(exit.ToNode);
                            nodeMap.Add(exit.ToNode.Identifier, reachableVertex);
                        }

                        // If the closed list already searched this vertex, skip it
                        if (!closedList.Contains(exit.ToNode.Identifier))
                        {
                            double edgeCost = this.edgeCostEvaluator.Apply(exit);

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

        /// <summary>
        /// Finds the cloest vertex in the graph to a point
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        protected Vertex GetClosestVertex(MapPoint point)
        {
            Vertex closest = null;
            float minDistance = float.MaxValue;
            foreach (var vertex in graph.Verticies)
            {
                var distance = GeometryEngine.GeodesicDistance(point, vertex.Coordinates);
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
