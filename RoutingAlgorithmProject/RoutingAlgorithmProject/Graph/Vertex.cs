
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RoutingAlgorithmProject.Graph
{
    public class Vertex
    {
        // Private member-variables
        private VertexData data;
        private Dictionary<Vertex, Edge> neighbors = null; // vertex, edge weight to neighboring vertices

        public Vertex(VertexData data) {
            this.data = data;
        }
        
        public VertexData Data
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
    }
       
}


