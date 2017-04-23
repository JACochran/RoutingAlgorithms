
using OsmSharp.Streams;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RoutingAlgorithmProject.Utility
{
    public static class OsmUtility
    {

        public static Graph.Graph ReadOsmData()
        {
            Graph.Graph g = new Graph.Graph();
            string path = "..\\..\\..\\Resources\\district-of-columbia-latest.osm.pbf"; // path to dc data
            if (File.Exists(path))
            {
                using (var fileStreamSource = File.OpenRead(path))
                {
                    var source = new PBFOsmStreamSource(fileStreamSource);
                    var filtered = source.FilterBox(-77f, 39f, -76f, 38f); // left, top, right, bottom
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
    }
}
