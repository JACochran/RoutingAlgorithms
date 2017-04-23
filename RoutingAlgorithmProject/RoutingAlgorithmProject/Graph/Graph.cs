using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutingAlgorithmProject.Graph
{
    public class Graph
    {
        private List<Vertex> vertices;

        public Graph() 
        {
          this.vertices = new List<Vertex>();
        }

        public void AddVertex(Vertex Vertex)
        {
            // adds a Vertex to the graph
            Vertices.Add(Vertex);
        }

        public void AddEdge(Vertex from, Vertex to, Edge e)
        {
            from.AddEdge(to, e);
            to.AddEdge(from, e);
        }

        public bool Contains(long? id)
        {
            foreach (Vertex v in Vertices)
            {
                if (v.Data.id == id)
                    return true;
            }
            return false;
        }

        public Vertex GetVertex(long? id)
        {
            return Vertices.FirstOrDefault(x => x.Data.id == id);
        }
        public List<Vertex> Vertices
        {
            get
            {
                return vertices;
            }
        }

        public static float FindWeight(VertexData v1, VertexData v2)
        {
            var R = 6371; // Radius of the earth in km
            var dLat = deg2rad(v2.latitude - v1.latitude);  // deg2rad below
            var dLon = deg2rad(v2.longitude - v1.longitude);
            var a =
              Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(deg2rad(v1.latitude)) * Math.Cos(deg2rad(v2.latitude)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c; // Distance in km
            return (float)d;
        }

        private static float deg2rad(float? deg)
        {
            return (float) (deg * (Math.PI / 180));
        }

    }
}
