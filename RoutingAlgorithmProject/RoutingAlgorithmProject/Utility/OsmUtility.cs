using Itinero;
using Itinero.IO.Osm;
using Itinero.Osm.Vehicles;
using OsmSharp.Osm.PBF.Streams;
using OsmSharp.Osm.Xml.Streams;
using System.IO;
using System.Linq;

namespace RoutingAlgorithmProject.Utility
{
    public static class OsmUtility
    {

        public static void ReadOsmData()
        {
            ReadOsmDataAgain();
            //using (var fileStream = new FileInfo(@".../.../Resources/north-america-latest.osm.pbf").OpenRead())
            //{
            //    var source = new PBFOsmStreamSource(fileStream);

            //    //-83.6752, 36.5408, -75.2418, 39.4659

            //    var filtered = source.FilterBox(-83.6752f, 39.4659f, -75.2418f, 36.5408f); // left, top, right, bottom

            //    var target = new PBFOsmStreamTarget(new FileInfo(@".../.../Resources/Virginia.osm.pbf").Open(FileMode.Create, FileAccess.ReadWrite));
            //    target.RegisterSource(filtered);
            //    target.Pull();

            //}
        }

        private static void ReadOsmDataAgain()
        {
            using (var file = new FileInfo(@".../.../Resources/north-america-latest.osm.pbf").OpenRead())
            {

                var source = new PBFOsmStreamSource(file);

                //var filtered = source.FilterSpatial(-83.6752f, 39.4659f, -75.2418f, 36.5408f); // left, top, right, bottom
                //var filtered = from osmGeo in source
                //               where osmGeo.Type == OsmSharp.Osm.OsmGeoType.Node
                //               select osmGeo;

                //var complete = filtered.ToList();


            }
        }
    }
}
