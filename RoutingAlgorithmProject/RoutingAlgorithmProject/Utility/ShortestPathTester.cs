using RoutingAlgorithmProject.Graph;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoutingAlgorithmProject.Routing;
using System.IO;

namespace RoutingAlgorithmProject.Utility
{
    class ShortestPathTester
    {
        /// <summary>
        /// runs path shortest path algorithm on each implementation
        /// </summary>
        /// <param name="graph"></param>
        internal static void TestPathFinders(RoutingGraph graph, string testPointsPath)
        {
            //graph.Verticies.ForEach(vertex => vertex.UseId = true);
            GraphTestResults results = new GraphTestResults(graph.Name);
            //// get all types that inherit PathFinder
            var pathFinders = typeof(PathFinder).Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(PathFinder)));
            foreach (Type t in pathFinders)
            {
                if(t != typeof(DijkstraPathFinder) && t != typeof(AStarPathFinder))
                {
                    // run TestPathFinder on each type
                    PathFinder pf = (PathFinder)Activator.CreateInstance(t, graph);
                    results.AddAlgorithmTestResult(TestPathFinder(pf, testPointsPath));
                }
            }
            ParseResults(results, graph);
        }

        private static void ParseResults(GraphTestResults results, RoutingGraph graph)
        {
            string outputPath = System.IO.Path.Combine("output", DateTime.Now.ToString("yyyyMMdd_hhmmss") + "TotalResults.txt");
            if (!System.IO.Directory.Exists("output"))
                System.IO.Directory.CreateDirectory("output");
            using (System.IO.TextWriter tw = new System.IO.StreamWriter(outputPath, false))
            {
                WriteRoadNetworkCharacteristics(tw, graph);
                WriteRuntimes(tw, results);
                WriteIntervalRuntimes(tw, results);
                
            }
        }

        private static void WriteIntervalRuntimes(TextWriter tw, GraphTestResults results)
        {
            writeLineBreak(tw);
            string formatString = "{0,-20}\t{1,-20} ";
            tw.WriteLine(String.Format(formatString, "Interval", "Runtime"));
            writeLineBreak(tw);
            foreach (var algorithmTestResult in results.GetAlgorithmTestResults())
            {
                tw.WriteLine(algorithmTestResult.AlgorithmName);
                for (int i = 0; i < AlgorithmTestResults.NUMBEROFINTERVALS; i++)
                {
                    int start = (int)algorithmTestResult.GetIntervalLength() * i;
                    int end = (int)algorithmTestResult.GetIntervalLength() * i + 1;
                    string intervalString = "[" + i + "] (" + start + " - " + end + ")";
                    tw.WriteLine(String.Format(formatString, intervalString, algorithmTestResult.AverageIntervalRuntimes[i].ToString("N4")));
                }
                writeLineBreak(tw);
            }
        }

        private static void WriteRuntimes(TextWriter tw, GraphTestResults results)
        {
            Dictionary<string, double> avgRuntimes = new Dictionary<string, double>();
            writeLineBreak(tw);
            string formatString = "{0,-20} {1,-20} {2,-20}";
            tw.WriteLine(String.Format(formatString, "Algorithm", "DC", "VA"));
            foreach (var algorithmTestResult in results.GetAlgorithmTestResults())
            {
                double avgRunTime = algorithmTestResult.GetAverageRuntime();
                tw.WriteLine(String.Format(formatString, algorithmTestResult.AlgorithmName, avgRunTime.ToString("N5"), 0.0));
                avgRuntimes[algorithmTestResult.AlgorithmName] = avgRunTime;
            }
            writeLineBreak(tw);
            tw.WriteLine("Ratio of execution times");
            writeLineBreak(tw);
           // double aslDikl = (avgRuntimes["ASL"] / avgRuntimes["DIKL"]) * 100;
            double ashDikh = (avgRuntimes["ASH"] / avgRuntimes["DIKH"]);
            double asbaDikba = (avgRuntimes["ASBA"] / avgRuntimes["DIKBA"]);
          //  tw.WriteLine(String.Format(formatString, "ASL/DIKL", aslDikl, 0.0));
            tw.WriteLine(String.Format(formatString, "ASH/DIKH", ashDikh.ToString("p"), 0.0));
            tw.WriteLine(String.Format(formatString, "ASBA/DIKBA", asbaDikba.ToString("p"), 0.0));
            writeLineBreak(tw);
        }

        private static void writeLineBreak(TextWriter tw)
        {
            tw.WriteLine("--------------------------------------------------------------------------------");
        }

        private static void WriteRoadNetworkCharacteristics(TextWriter tw, RoutingGraph graph)
        {
            tw.WriteLine("Road network characteristics.");
            writeLineBreak(tw);
            string formatString = "{0,-8} {1,-12} {2,-12} {3, -20}";
            tw.WriteLine(String.Format(formatString, "Name", "# of nodes", "# of arcs", "Arc/node ratio"));
            double edgeRatio = (double)graph.EdgeCount / graph.Verticies.Count;
            tw.WriteLine(String.Format(formatString, graph.Name, graph.Verticies.Count, graph.EdgeCount, edgeRatio.ToString("N5")));
        }

        /// <summary>
        /// reads from a file then finds a path between every point in the file
        /// k points in file = k(k-1)/2 paths
        /// Creates output file for each implementation with the average time for each path
        /// </summary>
        /// <param name="pf"></param>
        private static AlgorithmTestResults TestPathFinder(PathFinder pf, string testPointsPath)
        {
            AlgorithmTestResults resultsList = new AlgorithmTestResults(pf.GetAbbreivatedName());
            List<StartDestinationPair> testPoints = ReadPointFile(testPointsPath);
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
                        resultsList.AddTestResult(new TestResults(elapsedTime, path.Count));
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

        
    }
}
