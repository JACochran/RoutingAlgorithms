
using OsmSharp.Streams;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RoutingAlgorithmProject.Utility
{
    public static class OsmUtility
    {

        public static void ReadOsmData()
        {
            string path = "..\\..\\..\\Resources\\district-of-columbia-latest.osm.pbf"; // path to dc data
            if (File.Exists(path))
            {
                using (var fileStreamSource = File.OpenRead(path))
                {
                    var source = new PBFOsmStreamSource(fileStreamSource);
                    var filtered = source.FilterBox(-80f, 42f, -76f, 38f); // left, top, right, bottom
                    //var progress = source.ShowProgress();
                    var nodesAndWays = from osmGeo in filtered
                                       where osmGeo.Type == OsmSharp.OsmGeoType.Node || (osmGeo.Type == OsmSharp.OsmGeoType.Way && osmGeo.Tags != null && osmGeo.Tags.ContainsKey("highway"))
                                       select osmGeo;
                    var completed = nodesAndWays.ToComplete();

                    Graph.Graph g = new Graph.Graph();
                    foreach (var obj in completed)
                    {
                        if (obj.Type == OsmSharp.OsmGeoType.Way)
                        {
                            OsmSharp.Complete.CompleteWay way = (OsmSharp.Complete.CompleteWay)obj;

                            List<Graph.Vertex> vList = new List<Graph.Vertex>();

                            foreach (OsmSharp.Node node in way.Nodes)
                            {
                                Graph.Vertex vertex = g.GetVertex(node.Id);
                                if (vertex == null)
                                {
                                    Graph.VertexData data = new Graph.VertexData(node.Id, node.Latitude, node.Longitude, node.Tags);
                                    vertex = new Graph.Vertex(data);
                                    g.AddVertex(vertex);
                                }
                                else
                                {
                                    var x = 1;
                                }
                                vList.Add(vertex);
                            }
                            // find end points of edge?
                            if (vList.Count < 2)
                            {
                                Graph.Vertex start = vList[0];
                                Graph.Vertex end = vList[1];
                                if (start != null && end != null)
                                    g.AddEdge(start, end, new Graph.Edge(Graph.Graph.FindWeight(start.Data, end.Data), way.Tags));
                               
                            }
                        }
                    }
                }
            }
        }
    }
}
