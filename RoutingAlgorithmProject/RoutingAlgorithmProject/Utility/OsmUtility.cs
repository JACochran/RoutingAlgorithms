using Esri.ArcGISRuntime.Geometry;
using OsmSharp.Streams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RoutingAlgorithmProject.Utility
{
    public static class OsmUtility
    {

        public static Graph.Graph ReadOsmData(MapPoint start, MapPoint end)
        {
            Graph.Graph g = new Graph.Graph();
            string path = @"..\..\Resources\district-of-columbia-latest.osm.pbf"; // path to dc data
            if (File.Exists(path))
            {
                using (var fileStreamSource = File.OpenRead(path))
                {
                    var source = new PBFOsmStreamSource(fileStreamSource);

                    float buffer = 0.2f;
                    var maxLat = Math.Max(start.Y, end.Y) + buffer;
                    var maxLon = Math.Max(start.X, end.X) + buffer;
                    var minLat = Math.Min(start.Y, end.Y) - buffer;
                    var minLon = Math.Min(start.X, end.X) - buffer;

                    
                    var filtered = source.FilterBox((float)minLon , (float)maxLat, (float)maxLon, (float)minLat); // left, top, right, bottom
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
                                MapPoint location = new MapPoint((double)node.Longitude,(double)node.Latitude, SpatialReferences.Wgs84);
                                Graph.Vertex toVertex = g.AddVertex(location);
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
            Graph.Vertex a = g.AddVertex(new MapPoint(0, 0, SpatialReferences.Wgs84));
            Graph.Vertex b = g.AddVertex(new MapPoint(0, 10, SpatialReferences.Wgs84));
            Graph.Vertex c = g.AddVertex(new MapPoint(5, 5, SpatialReferences.Wgs84));
            Graph.Vertex d = g.AddVertex(new MapPoint(10, 10, SpatialReferences.Wgs84));

            g.AddEdge(new Graph.Edge(a, c));
            g.AddEdge(new Graph.Edge(c, d));
            g.AddEdge(new Graph.Edge(a, b));
            g.AddEdge(new Graph.Edge(b, d));

            PathFinder.PathFinder dpf = new PathFinder.DijkstraPathFinder(g);
            var path = dpf.FindShortestPath(new MapPoint(1, 1, SpatialReferences.Wgs84), new MapPoint(9, 9, SpatialReferences.Wgs84));
            var x = 1;
        }
    }

}
