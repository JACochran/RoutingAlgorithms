using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OsmSharp.Tags;

namespace RoutingAlgorithmProject.Graph
{
    public class VertexData
    {
        public long? id;
        public float? latitude;
        public float? longitude;
        public TagsCollectionBase tags;

        public VertexData(long? id, float? latitude, float? longitude, TagsCollectionBase tags)
        {
            this.id = id;
            this.latitude = latitude;
            this.longitude = longitude;
            this.tags = tags;
        }
    }
}
