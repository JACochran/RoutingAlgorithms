﻿using System.Collections.Generic;
using System.Linq;


namespace RoutingAlgorithmProject.Graph
{
    public class RoutingGraph 
    {
        private Dictionary<Coordinates, Vertex> vertexMap;
        private string _name;
        private int _edges;
        private string _testPointsPath;

        public RoutingGraph(string name, string testPointsPath)
        {
            this.vertexMap = new Dictionary<Coordinates, Vertex>();
            this._name = name;
            this._testPointsPath = testPointsPath;
        }

        public string Name{
            get
            {
                return _name;
            }
        }

        public string TestPointsPath
        {
            get
            {
                return _testPointsPath;
            }
        }

        public int EdgeCount
        {
            get{
                return _edges;
            }
        }


        // <summary>
        /// if a vertex does not exist at location coords, add to the dictionary
        /// </summary>
        /// <param name="coords"></param>
        /// <returns>reference to vertex at location coords</returns>
        public Vertex AddVertex(Coordinates coords)
        {
            var vertex = this.GetVertex(coords);
            if (vertex == null)
            {
                vertex = new Graph.Vertex(coords);
                vertexMap.Add(coords, vertex);
            }
            return vertex;
        }

        /// <summary>
        /// Adds an edge to the graph
        /// creates an edge in the neighbor list of each vertex
        /// </summary>
        /// <param name="e"></param>
        public void AddEdge(Edge e)
        {
            if (!vertexMap.ContainsKey(e.To.Coordinates))
            {
                vertexMap.Add(e.To.Coordinates, e.To);
            }
            if (!vertexMap.ContainsKey(e.From.Coordinates))
            {
                vertexMap.Add(e.From.Coordinates, e.From);
            }
            e.From.AddEdge(e.To, e);
            e.To.AddEdge(e.From, e);
            _edges++;
        }

        /// <summary>
        /// Checks if the graph contains a vertex at location coords
        /// returns null if not found
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
        public Vertex GetVertex(Coordinates coords)
        {
            if (vertexMap.ContainsKey(coords))
                return vertexMap[coords];
            else
                return null;
        }

        /// <summary>
        /// Returns all Verticies in the graph
        /// </summary>
        public List<Graph.Vertex> Verticies
        {
            get
            {
                return vertexMap.Values.ToList();
            }
        }

        public void ResetGraph()
        {
            foreach(var v in this.vertexMap.Values)
            {
                v.CostFromStart = float.MaxValue;
                v.EstimatedCostToEnd = float.MaxValue;
                v.Priority = -1;
                v.QueueIndex = -1;
                v.FIFOnext = null;
                v.FIFOprev = null;
                v.InQueue = false;
            }
        }

        public void CleanGraph()
        {
            // use DFS to remove unneeded verticies
            var start = this.vertexMap.Values.First();

            var visited = new HashSet<Vertex>();

            var stack = new Stack<Vertex>();
            stack.Push(start);

            while (stack.Count > 0)
            {
                var vertex = stack.Pop();

                if (visited.Contains(vertex))
                    continue;

                CleanVertex(vertex);
                visited.Add(vertex);

                var neighborList = vertex.Neighbors.Keys.ToList();
                foreach (var neighbor in neighborList)
                    if (!visited.Contains(neighbor))
                        stack.Push(neighbor);
            }

            return;
        }

        private void CleanVertex(Vertex middleNode)
        {
            var neighborList = middleNode.Neighbors.ToArray();
            if (middleNode.Neighbors.Count == 2)
            {
                var startEdge = neighborList[0].Value;
                var endEdge = neighborList[1].Value;

                var startNode = neighborList[0].Key;
                var endNode = neighborList[1].Key;

                float totalDistance = startEdge.Weight + endEdge.Weight;

                // new edge from startEdge to endEdge
                Edge newEdge = new Edge(startNode, endNode, totalDistance);

                //remove edges to middle node
                startNode.Neighbors.Remove(middleNode);
                endNode.Neighbors.Remove(middleNode);
                // remove middle node from graph;
                this.vertexMap.Remove(middleNode.Coordinates);

                // add new edge 
                AddEdge(newEdge);
            }
        }
    }
}
