using System;
using System.Collections.Generic;

namespace RoutingAlgorithmProject.Graph
{

    public class Vertex: IComparable
    {
        // Private member-variables
        private Coordinates coords;
        private Dictionary<Vertex, Edge> neighbors = null;

        // AStar variables
        public Vertex Previous { get; internal set; }
        public float EstimatedCostToEnd { get; set; }
        public float CostFromStart { get; set; }

        public Vertex(Coordinates coords)
        {
            this.coords = coords;
            this.neighbors = new Dictionary<Vertex, Edge>();
            this.CostFromStart = float.MaxValue;
            this.EstimatedCostToEnd = float.MaxValue;
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

        public virtual void AddEdge(Vertex to, Edge e)
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
            return coords.ToString() ;
        }

        public void Update(float costFromStart,
                           float estimatedCostToEnd,
                           Vertex previous)
        {
            if (costFromStart < 0.0)
            {
                throw new ArgumentException("Distance from start may not be less than 0");
            }

            if (estimatedCostToEnd < 0.0)
            {
                throw new ArgumentException("Distance from end may not be less than 0");
            }

            CostFromStart = costFromStart;
            EstimatedCostToEnd = estimatedCostToEnd;
            Previous = previous;
        }

        public int CompareTo(object obj)
        {
            var vertex2 = obj as Vertex;
            if (obj == null)
            {
                throw new ArgumentException("cannot compare these two types");
            }

            float cost = EstimatedCostToEnd + CostFromStart;
            return cost.CompareTo(vertex2.EstimatedCostToEnd + vertex2.CostFromStart);
        }
    }

}


