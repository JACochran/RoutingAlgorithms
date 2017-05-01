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

        public static Graph.RoutingGraph ReadOsmData(string path, string testPointsPath, string name) 
        {
            RoutingGraph graph = new RoutingGraph(name, testPointsPath);
            {
                using (var fileStreamSource = File.OpenRead(path))
                {
                    var source = new PBFOsmStreamSource(fileStreamSource);
                    var filtered = name.Equals("VA")? source.FilterBox(-77.8f, 39.4f, -77f, 38.67f) : source;
                    var nodesAndWays = from osmGeo in filtered
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

        //public static void TestGraph()
        //{
        //    Graph.RoutingGraph graph = new Graph.RoutingGraph("test");
        //    Graph.Vertex a = graph.AddVertex(new Coordinates(0, 0));
        //    Graph.Vertex b = graph.AddVertex(new Coordinates(0, 1));
        //    Graph.Vertex c = graph.AddVertex(new Coordinates(0.5f, 0.5f));
        //    Graph.Vertex d = graph.AddVertex(new Coordinates(1, 1));
        //    Graph.Vertex e = graph.AddVertex(new Coordinates(0.2f, 0.5f));
        //    Graph.Vertex f = graph.AddVertex(new Coordinates(0.3f, 0.7f));
        //    Graph.Vertex g = graph.AddVertex(new Coordinates(0.6f, 0.7f));

        //    graph.AddEdge(new Graph.Edge(a, b));
        //    graph.AddEdge(new Graph.Edge(a, c));
        //    graph.AddEdge(new Graph.Edge(a, e));
        //    graph.AddEdge(new Graph.Edge(b, d));
        //    graph.AddEdge(new Graph.Edge(b, e));
        //    graph.AddEdge(new Graph.Edge(b, g));
        //    graph.AddEdge(new Graph.Edge(c, d));
        //    graph.AddEdge(new Graph.Edge(c, e));
        //    graph.AddEdge(new Graph.Edge(c, g));
        //    graph.AddEdge(new Graph.Edge(d, g));
        //    graph.AddEdge(new Graph.Edge(e, f));
        //    graph.AddEdge(new Graph.Edge(f, g));



        //    float pathLength = 0;
        //    var dpf = new DijkstraApproximateBucketPathFinder(graph);
        //    //var path = dpf.FindShortestPath(new Coordinates(0.1f, 0.1f), new Coordinates(0.9f, 0.9f), ref pathLength);
        //}
    }

}
