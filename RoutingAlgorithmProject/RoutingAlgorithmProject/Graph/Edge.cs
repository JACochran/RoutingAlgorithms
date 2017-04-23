using OsmSharp.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutingAlgorithmProject.Graph
{
    public class Edge
    {
        public float Weight;
        public TagsCollectionBase tags;

        public Edge(float weight, TagsCollectionBase tags)
        {
            this.Weight = weight;
            this.tags = tags;
        }
    }
}
