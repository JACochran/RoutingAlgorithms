
using OsmSharp.Streams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RoutingAlgorithmProject.Utility
{
    public static class OsmUtility
    {

        public static Graph.Graph ReadOsmData(Graph.Coordinates start, Graph.Coordinates end)
        {
            Graph.Graph g = new Graph.Graph();
            string path = "..\\..\\..\\Resources\\district-of-columbia-latest.osm.pbf"; // path to dc data
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

                            List<Graph.Vertex> vList = new List<Graph.Vertex>();
                            Graph.Vertex fromVertex = null;
                            foreach (OsmSharp.Node node in way.Nodes)
                            {
                                Graph.Coordinates data = new Graph.Coordinates(node.Latitude, node.Longitude);
                                Graph.Vertex toVertex = g.AddVertex(data);
                                if (fromVertex != null)
                                {
                                    Graph.Edge edge = new Graph.Edge(fromVertex, toVertex);
                                    g.AddEdge(edge);
                                }
                                fromVertex = toVertex;
                            }                            
                        }
                    }
                }
            }
            return g;
        }

        public static void TestGraph()
        {
            Graph.Graph g = new Graph.Graph();
            Graph.Vertex a = g.AddVertex(new Graph.Coordinates(0, 0));
            Graph.Vertex b = g.AddVertex(new Graph.Coordinates(0, 10));
            Graph.Vertex c = g.AddVertex(new Graph.Coordinates(5, 5));
            Graph.Vertex d = g.AddVertex(new Graph.Coordinates(10, 10));

            g.AddEdge(new Graph.Edge(a, c));
            g.AddEdge(new Graph.Edge(c, d));
            g.AddEdge(new Graph.Edge(a, b));
            g.AddEdge(new Graph.Edge(b, d));

            PathFinder.PathFinder dpf = new PathFinder.DijkstraPathFinder(g);
            var path = dpf.FindShortestPath(new Graph.Coordinates(1, 1), new Graph.Coordinates(9, 9));
        }
    }

}
