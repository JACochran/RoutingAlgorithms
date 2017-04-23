
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RoutingAlgorithmProject.Graph
{
    public class Vertex
    {
        // Private member-variables
        private Coordinates data;
        private Dictionary<Vertex, Edge> neighbors = null; 

        public Vertex(Coordinates data) {
            this.data = data;
        }
        
        public Coordinates Coordinates
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }

        public Dictionary<Vertex, Edge> Neighbors
        {
            get
            {
                if (neighbors == null)
                    neighbors = new Dictionary<Vertex, Edge>();
                return neighbors;
            }
            set
            {
                neighbors = value;
            }
        }

        public void AddEdge(Vertex to, Edge e)
        {
            this.Neighbors[to] = e;
        }

        public override bool Equals(object obj)
        {
            var item = obj as Vertex;
            if (item == null)
                return false;
            return data.Equals(item.data);
        }

        public override int GetHashCode()
        {
            return data.GetHashCode();
        }

        public override string ToString()
        {
            return "(" + Coordinates.Latitude.ToString() + "," +Coordinates.Longitude.ToString() + ")";
        }
    }
       
}


