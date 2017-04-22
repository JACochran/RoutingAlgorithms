using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutingAlgorithmProject.Routing.Models
{
    public abstract class Vertex
    {
        public Vertex Previous { get; internal set; }
        public Attributes Node { get; internal set; }
        public List<object> EdgeAttributes { get; internal set; }
        public int EdgeIdentifier { get; internal set; }
        public double EdgeCost { get; internal set; }
    }
}
