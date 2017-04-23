using OsmSharp.Streams;
using RoutingAlgorithmProject.Graph;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RoutingAlgorithmProject.Utility
{
    public static class OsmUtility
    {

        public static Graph.RoutingGraph ReadOsmData(Coordinates start, Coordinates end) 
        {
            RoutingGraph graph = new RoutingGraph();
            string path = @"..\..\Resources\district-of-columbia-latest.osm.pbf"; // path to dc data
            if (File.Exists(path))
            {
                using (var fileStreamSource = File.OpenRead(path))
                {
                    var source = new PBFOsmStreamSource(fileStreamSource);

                    float buffer = 0.2f;
                    var maxLat = Math.Max(start.Latitude, end.Latitude) + buffer;
                    var maxLon = Math.Max(start.Longitude, end.Longitude) + buffer;
                    var minLat = Math.Min(start.Latitude, end.Latitude) - buffer;
                    var minLon = Math.Min(start.Longitude, end.Longitude) - buffer;

                    
                    var filtered = source.FilterBox(minLon , maxLat, maxLon, minLat); // left, top, right, bottom
                    //var progress = source.ShowProgress();
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

            var dpf = new PathFinder.DijkstraPathFinder(g);
            var path = dpf.FindShortestPath(new Coordinates(1, 1), new Coordinates(9, 9));

            var astarGraph = new Graph.RoutingGraph();
            var vertex1 = astarGraph.AddVertex(new Coordinates(0, 0));
            var vertex2 = astarGraph.AddVertex(new Coordinates(0, 10));
            var vertex3 = astarGraph.AddVertex(new Coordinates(5, 5));
            var vertex4 = astarGraph.AddVertex(new Coordinates(10, 10));


            astarGraph.AddEdge(new Graph.Edge(vertex1, vertex3));
            astarGraph.AddEdge(new Graph.Edge(vertex3, vertex4));
            astarGraph.AddEdge(new Graph.Edge(vertex1, vertex2));
            astarGraph.AddEdge(new Graph.Edge(vertex2, vertex4));
            var path2 = new PathFinder.AStarPathFinder(astarGraph).FindShortestPath(new Coordinates(1, 1), new Coordinates(9, 9));
        }
    }

}
