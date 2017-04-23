﻿//using RoutingAlgorithmProject.Graph;
//using System;
//using System.Collections.Generic;

//namespace RoutingAlgorithmProject.Routing.Models
//{
//    public class AStarVertex : Vertex, IComparable
//    {
//        public AStarVertex Previous { get; internal set; }
//        public double EdgeCost { get; internal set; }
//        public double EstimatedCostToEnd { get; set; }
//        public double CostFromStart { get; set; }

//        public int CompareTo(object obj)
//        {
//            var vertex2 = obj as AStarVertex;
//            if (obj == null)
//            {
//                throw new ArgumentException("cannot compare these two types");
//            }

//            Double cost = EstimatedCostToEnd + CostFromStart;
//            return cost.CompareTo(vertex2.EstimatedCostToEnd + vertex2.CostFromStart);
//        }

//        public AStarVertex(Coordinates  location): base(location)
//        {
//            CostFromStart = double.MaxValue;
//            EstimatedCostToEnd = double.MaxValue;
//        }

//        public override void AddEdge(Vertex to, Edge edge)
//        {
//            base.AddEdge(to, edge);
//        }

//        public void Update(double costFromStart,
//                           double estimatedCostToEnd,
//                           AStarVertex previous,
//                           double edgeCost)
//        {
//            if (costFromStart < 0.0)
//            {
//                throw new ArgumentException("Distance from start may not be less than 0");
//            }

//            if (estimatedCostToEnd < 0.0)
//            {
//                throw new ArgumentException("Distance from end may not be less than 0");
//            }

//            CostFromStart = costFromStart;
//            EstimatedCostToEnd = estimatedCostToEnd;
//            Previous = previous;
//            EdgeCost = edgeCost;
//        }
        
//        //private double costFromStart = Double.MaxValue;
//        //private double estimatedCostToEnd = Double.MaxValue;
//        //private AStarVertex previous;                              // Parent node
//        //private int edgeIdentifier;                        // Unique identifier of the edge taken to get to this node
//        //private List<Object> edgeAttributes;                        // Attributes of edge with the edge being parent to this one
//        //private double edgeCost;                              // Calculated cost of parent node to this one

//    }
//}
