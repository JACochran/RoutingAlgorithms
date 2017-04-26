using OsmSharp.Streams;
using RoutingAlgorithmProject.Graph;
using RoutingAlgorithmProject.Routing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace RoutingAlgorithmProject.Utility
{
    public static class OsmUtility
    {

        public static Graph.RoutingGraph ReadOsmData(String path) 
        {
            RoutingGraph graph = new RoutingGraph();
            {
                using (var fileStreamSource = File.OpenRead(path))
                {
                    var source = new PBFOsmStreamSource(fileStreamSource);
                    var nodesAndWays = from osmGeo in source
                                       where osmGeo.Type == OsmSharp.OsmGeoType.Node || (osmGeo.Type == OsmSharp.OsmGeoType.Way && osmGeo.Tags != null && osmGeo.Tags.ContainsKey("highway"))
                                       select osmGeo;
                    var completed = nodesAndWays.ToComplete();
                    
                    foreach (var obj in completed)
                    {
                        if (obj.Type == OsmSharp.OsmGeoType.Way)
                        {
                            OsmSharp.Complete.CompleteWay way = (OsmSharp.Complete.CompleteWay)obj;

                            var vList = new List<Vertex>();
                            Vertex fromVertex = null;
                            foreach (OsmSharp.Node node in way.Nodes)
                            {
                                var location = new Coordinates(node.Latitude, node.Longitude);
                                var toVertex = graph.AddVertex(location);
                                if (fromVertex != null)
                                {
                                    var edge = new Edge(fromVertex, toVertex);
                                    graph.AddEdge(edge);
                                }
                                fromVertex = toVertex;
                            }                            
                        }
                    }
                }
            }
            graph.CleanGraph();
            return graph;
        }

        public static void TestGraph()
        {
            Graph.RoutingGraph g = new Graph.RoutingGraph();
            Graph.Vertex a = g.AddVertex(new Coordinates(0, 0));
            Graph.Vertex b = g.AddVertex(new Coordinates(0, 10));
            Graph.Vertex c = g.AddVertex(new Coordinates(5, 5));
            Graph.Vertex d = g.AddVertex(new Coordinates(10, 10));

            g.AddEdge(new Graph.Edge(a, c));
            g.AddEdge(new Graph.Edge(c, d));
            g.AddEdge(new Graph.Edge(a, b));
            g.AddEdge(new Graph.Edge(b, d));

            var dpf = new AStarMinHeapPathFinder(g);
            var path = dpf.FindShortestPath(new Coordinates(1, 1), new Coordinates(9, 9));
        }
    }

}
