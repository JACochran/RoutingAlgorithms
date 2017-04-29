using RoutingAlgorithmProject.Graph;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoutingAlgorithmProject.Routing;

namespace RoutingAlgorithmProject.Utility
{
    class ShortestPathTester
    {
        /// <summary>
        /// runs path shortest path algorithm on each implementation
        /// </summary>
        /// <param name="graph"></param>
        internal static void TestPathFinders(RoutingGraph graph)
        {
            //OsmUtility.TestGraph();

            //(38.90147, -76.97354)->)
            //(38.90147,-76.97354) ->(38.91833,-76.99895)
            var a = new Coordinates(38.90147f, -76.97354f);
            var b = new Coordinates(38.91833f, -76.99895f);

            //PathFinder lkj =  new AStarMinHeapPathFinder(graph);
            //var a1 = lkj.FindShortestPath(a, b);
            //graph.ResetGraph();
            //PathFinder fff = new DijkstraApproximateBucketPathFinder(graph);
            //var a2 = fff.FindShortestPath(a, b);

            //List<List<TestResults>> results = new List<List<TestResults>>();
            ////// get all types that inherit PathFinder
            //var pathFinders = typeof(PathFinder).Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(PathFinder)));
            //foreach (Type t in pathFinders)
            //{
            //    // run TestPathFinder on each type
            //    PathFinder pf = (PathFinder)Activator.CreateInstance(t, graph);
            //    results.Add(TestPathFinder(pf));
            //}
            

            graph.Verticies.ForEach(vertex => vertex.UseId = true);

            //var pathfinder = new AStarApproximateBucketPathFinder(graph);
            //float pathlength = 0.0f;
            //var pfinder = new AStarMinHeapPathFinder(graph);
            //graph.ResetGraph();
            //var path = pathfinder.FindShortestPath(b, a, ref pathlength);
            //graph.ResetGraph();
            //var path2 = pfinder.FindShortestPath(b, a, ref pathlength);
            //ParseResults(results);
             TestPathFinder(new AStarApproximateBucketPathFinder(graph));
            ////graph.ResetGraph();
            // TestPathFinder(new DijkstraApproximateBucketPathFinder(graph));
            //    TestPathFinder(new AStarPathFinder(graph));
            //    TestPathFinder(new DijkstraMinHeapPathFinder(graph));
            //    TestPathFinder(new DijkstraPathFinder(graph));
        }



        private static void ParseResults(List<List<TestResults>> results)
        {
            string outputPath = System.IO.Path.Combine("output", DateTime.Now.ToString("yyyyMMdd_hhmmss") + "TotalResults.txt");
            if (!System.IO.Directory.Exists("output"))
                System.IO.Directory.CreateDirectory("output");
            using (System.IO.TextWriter tw = new System.IO.StreamWriter(outputPath, false))
            {
                tw.WriteLine(String.Format("{0,-40} {1,-20} {2,-20}", "PathFinder", "Avg Runtime", "Avg # of nodes"));
                foreach(var list in results)
                {
                    tw.WriteLine(GetResultString(list));  
                }
            }
        }

        private static string GetResultString(List<TestResults> list)
        {
            double totalTime = 0;
            float totalNodes = 0;
            foreach(var r in list)
            {
                totalNodes += r.NumberOfNodes;
                totalTime += r.Time;
            }
            double avgRuntime = totalTime / list.Count;
            double avgNodes = totalNodes / list.Count;
            return String.Format("{0,-40} {1,-20} {2,-20}", list.First().PathFinderType.Name, avgRuntime, avgNodes);
        }

        static void runTest(Object stateInfo)
        {
            PathFinder pf = (PathFinder)stateInfo;
            TestPathFinder(pf);
        }



        /// <summary>
        /// reads from a file then finds a path between every point in the file
        /// k points in file = k(k-1)/2 paths
        /// Creates output file for each implementation with the average time for each path
        /// </summary>
        /// <param name="pf"></param>
        private static List<TestResults> TestPathFinder(PathFinder pf)
        {
            List<TestResults> resultsList = new List<TestResults>();
            List<StartDestinationPair> testPoints = ReadPointFile(@"..\..\Resources\dcTestPoints.csv");
            string outputPath = System.IO.Path.Combine("output", DateTime.Now.ToString("yyyyMMdd_hhmmss") + pf.GetType().Name + ".txt");
            if (!System.IO.Directory.Exists("output"))
                System.IO.Directory.CreateDirectory("output");
            using (System.IO.TextWriter tw = new System.IO.StreamWriter(outputPath, false))
            {
                double totalRunTime = 0;
                int successfullPathsFound = 0;
                foreach (var startDestinationPair in testPoints)
                {
                    float pathLength = 0;
                    var sw = new Stopwatch();
                    sw.Start();
                    var path = pf.FindShortestPath(startDestinationPair.Start, startDestinationPair.Destination, ref pathLength);
                    sw.Stop();
                    pf.ResetGraph();
                    string msg = null;
                    double elapsedTime = sw.Elapsed.TotalSeconds;
                    if (path != null && path.Count > 0)
                    {
                        totalRunTime += elapsedTime;
                        successfullPathsFound++;
                        msg = startDestinationPair.ToString() + "Completed in " + elapsedTime.ToString() + " seconds." + " with path length = " + pathLength + " vertex count = " + path.Count;
                    }
                    else
                    {
                        msg = startDestinationPair.ToString() + "Failed to find path in " + elapsedTime.ToString() + " seconds.";
                    }
                    tw.WriteLine(pf.GetType().Name + " " + msg);
                    if(path!=null)
                        resultsList.Add(new TestResults(elapsedTime, path.Count, pf.GetType()));
                }
                tw.WriteLine("Average Runtime(sec) = " + totalRunTime / successfullPathsFound + " Failed Paths = " + (testPoints.Count - successfullPathsFound));
            }
            return resultsList;
        }


        private static char[] coordsSplitChar = { ',' };

        private static List<StartDestinationPair> ReadPointFile(string v)
        {
            List<Coordinates> testPoints = new List<Coordinates>();
            using (System.IO.TextReader tr = new System.IO.StreamReader(v))
            {
                string line;
                while ((line = tr.ReadLine()) != null)
                {
                    testPoints.Add(parseCoordinate(line));
                }
            }
            return CreateStartDestinationPair(testPoints);
        }

        private static List<StartDestinationPair> CreateStartDestinationPair(List<Coordinates> testPoints)
        {
            List<StartDestinationPair> pairs = new List<StartDestinationPair>();
            for (int i = 0; i < testPoints.Count - 1; i++)
            {
                for (int j = i + 1; j < testPoints.Count; j++)
                {
                    pairs.Add(new StartDestinationPair(testPoints[i], testPoints[j]));
                }
            }
            return pairs;
        }

        private static Coordinates parseCoordinate(string pair)
        {
            var coordStrings = pair.Split(coordsSplitChar);
            var coordinate = new Coordinates(float.Parse(coordStrings[0]), float.Parse(coordStrings[1]));
            return coordinate;
        }

        public class StartDestinationPair
        {
            public readonly Coordinates Start;
            public readonly Coordinates Destination;

            public StartDestinationPair(Coordinates start, Coordinates destination)
            {
                this.Start = start;
                this.Destination = destination;
            }

            public override string ToString()
            {
                return Start.ToString() + "->" + Destination.ToString();
            }


        }

        public class TestResults
        {
            public readonly double Time;
            public readonly int NumberOfNodes;
            public readonly Type PathFinderType;

            public TestResults(double time, int nodes, Type pf)
            {
                this.Time = time;
                this.NumberOfNodes = nodes;
                this.PathFinderType = pf;
            }

        }
    }
}
