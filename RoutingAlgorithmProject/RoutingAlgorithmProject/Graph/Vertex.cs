
using Esri.ArcGISRuntime.Geometry;
using System.Collections.Generic;

namespace RoutingAlgorithmProject.Graph
{
    public class Vertex
    {
        // Private member-variables
        private long? id;

        public Vertex(MapPoint coords)
        {
            Coordinates = coords;
            Neighbors = new Dictionary<Vertex, Edge>();
        }

        public long? Identifier
        {
            get; set;
        }

        /// <summary>
        /// location of the vertex on the globe
        /// </summary>
        public MapPoint Coordinates
        {
            get; set;
        }

        /// <summary>
        /// Adjacent vertices are stored as a map of vertex to edge
        /// </summary>
        public Dictionary<Vertex, Edge> Neighbors
        {
            get; set;
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
            return Coordinates.IsEqual(item.Coordinates);
        }

        public override int GetHashCode()
        {
            return Coordinates.GetHashCode();
        }

        public override string ToString()
        {
            return "(" + Coordinates.Y.ToString() + "," +Coordinates.X.ToString() + ")";
        }
    }
       
}


