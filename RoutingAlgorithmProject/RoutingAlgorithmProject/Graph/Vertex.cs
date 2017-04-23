
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RoutingAlgorithmProject.Graph
{
    public class Vertex
    {
        // Private member-variables
        private Coordinates coords;
        private Dictionary<Vertex, Edge> neighbors = null; 

        public Vertex(Coordinates coords) {
            this.coords = coords;
            this.neighbors = new Dictionary<Vertex, Edge>();
        }
        
        /// <summary>
        /// location of the vertex on the globe
        /// </summary>
        public Coordinates Coordinates
        {
            get
            {
                return coords;
            }
            set
            {
                coords = value;
            }
        }

        /// <summary>
        /// Adjacent vertices are stored as a map of vertex to edge
        /// </summary>
        public Dictionary<Vertex, Edge> Neighbors
        {
            get
            {
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

        /// <summary>
        /// A vertex is considered equal if it has the same coordinates
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var item = obj as Vertex;
            if (item == null)
                return false;
            return coords.Equals(item.coords);
        }

        public override int GetHashCode()
        {
            return coords.GetHashCode();
        }

        public override string ToString()
        {
            return "(" + Coordinates.Latitude.ToString() + "," +Coordinates.Longitude.ToString() + ")";
        }
    }
       
}


