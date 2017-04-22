
using OsmSharp.Streams;
using System.IO;

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
                    var complete = filtered.ToComplete();
                }
            }
        }
    }
}
