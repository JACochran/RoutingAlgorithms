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
        internal static void TestPathFinders(RoutingGraph[] graphs)
        {
            GraphTestResults[] results = new GraphTestResults[2];
            //// get all types that inherit PathFinder
            var pathFinders = typeof(PathFinder).Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(PathFinder)));
            for(int i=0; i<graphs.Length;i++)
            {
                results[i] = new GraphTestResults(graphs[i].Name);
                results[i].Graph = graphs[i];
                // run TestPathFinder on each type
                PathFinder ASBA = new AStarApproximateBucketPathFinder(graphs[i]);
                PathFinder ASH = new AStarMinHeapPathFinder(graphs[i]);
                PathFinder DIKBA = new DijkstraApproximateBucketPathFinder(graphs[i]);
                PathFinder DIKH = new DijkstraMinHeapPathFinder(graphs[i]);
                results[i].AddAlgorithmTestResult(TestPathFinder(ASBA));
                results[i].AddAlgorithmTestResult(TestPathFinder(ASH));
                results[i].AddAlgorithmTestResult(TestPathFinder(DIKBA));
                results[i].AddAlgorithmTestResult(TestPathFinder(DIKH));
            }
            ParseResults(results);
        }

        private static void ParseResults(GraphTestResults[] results)
        {
            string outputPath = System.IO.Path.Combine("output", DateTime.Now.ToString("yyyyMMdd_hhmmss") + "TotalResults.txt");
            if (!System.IO.Directory.Exists("output"))
                System.IO.Directory.CreateDirectory("output");
            using (System.IO.TextWriter tw = new System.IO.StreamWriter(outputPath, false))
            {
                WriteRoadNetworkCharacteristics(tw, results);
                WriteRuntimes(tw, results);
                foreach(var result in results)
                    WriteIntervalRuntimes(tw, result);
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

        private static void WriteRuntimes(TextWriter tw, GraphTestResults[] results)
        {
            Dictionary<string, double> avgRuntimes0 = new Dictionary<string, double>();
            Dictionary<string, double> avgRuntimes1 = new Dictionary<string, double>();
            writeLineBreak(tw);
            string formatString = "{0,-20} {1,-20} {2,-20}";
            tw.WriteLine(String.Format(formatString, "Algorithm", results[0].GraphName, results[1].GraphName));
            for (int i=0;i<results[0].GetAlgorithmTestResults().Count;i++)
            {
                var algorithmTestResult0 = results[0].GetAlgorithmTestResults()[i];
                var algorithmTestResult1 = results[1].GetAlgorithmTestResults()[i];
                double avgRunTime0 = algorithmTestResult0.GetAverageRuntime();
                double avgRunTime1 = algorithmTestResult1.GetAverageRuntime();
                tw.WriteLine(String.Format(formatString, algorithmTestResult0.AlgorithmName, avgRunTime0.ToString("N5"), avgRunTime1.ToString("N5")));
                avgRuntimes0[algorithmTestResult0.AlgorithmName] = avgRunTime0;
                avgRuntimes1[algorithmTestResult1.AlgorithmName] = avgRunTime1;
            }
            writeLineBreak(tw);
            tw.WriteLine("Ratio of execution times");
            writeLineBreak(tw);
           // double aslDikl = (avgRuntimes["ASL"] / avgRuntimes["DIKL"]) * 100;
            double ashDikh0 = (avgRuntimes0["ASH"] / avgRuntimes0["DIKH"]);
            double asbaDikba0 = (avgRuntimes0["ASBA"] / avgRuntimes0["DIKBA"]);
            double ashDikh1 = (avgRuntimes1["ASH"] / avgRuntimes1["DIKH"]);
            double asbaDikba1 = (avgRuntimes1["ASBA"] / avgRuntimes1["DIKBA"]);
            //  tw.WriteLine(String.Format(formatString, "ASL/DIKL", aslDikl, 0.0));
            tw.WriteLine(String.Format(formatString, "ASH/DIKH", ashDikh0.ToString("p"), ashDikh1.ToString("p")));
            tw.WriteLine(String.Format(formatString, "ASBA/DIKBA", asbaDikba0.ToString("p"), asbaDikba1.ToString("p")));
            writeLineBreak(tw);
        }

        private static void writeLineBreak(TextWriter tw)
        {
            tw.WriteLine("--------------------------------------------------------------------------------");
        }

        private static void WriteRoadNetworkCharacteristics(TextWriter tw, GraphTestResults[] results)
        {
            tw.WriteLine("Road network characteristics.");
            writeLineBreak(tw);
            string formatString = "{0,-8} {1,-12} {2,-12} {3, -20}";
            tw.WriteLine(String.Format(formatString, "Name", "# of nodes", "# of arcs", "Arc/node ratio"));
            foreach(var result in results)
            {
                var graph = result.Graph;
                double edgeRatio = (double)graph.EdgeCount / graph.Verticies.Count;
                tw.WriteLine(String.Format(formatString, graph.Name, graph.Verticies.Count, graph.EdgeCount, edgeRatio.ToString("N5")));
            }
        }

        /// <summary>
        /// reads from a file then finds a path between every point in the file
        /// k points in file = k(k-1)/2 paths
        /// Creates output file for each implementation with the average time for each path
        /// </summary>
        /// <param name="pf"></param>
        private static AlgorithmTestResults TestPathFinder(PathFinder pf)
        {
            AlgorithmTestResults resultsList = new AlgorithmTestResults(pf.GetAbbreivatedName());
            List<StartDestinationPair> testPoints = ReadPointFile(pf.Graph.TestPointsPath);
            string outputPath = System.IO.Path.Combine("output", DateTime.Now.ToString(pf.Graph.Name + "yyyyMMdd_hhmmss") + pf.GetType().Name + ".txt");
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

                    Vertex startVertex = pf.FindClosestVertex(startDestinationPair.Start);
                    Vertex destinationVertex = pf.FindClosestVertex(startDestinationPair.Destination);

                    sw.Start();
                    var path = pf.FindShortestPath(startVertex, destinationVertex, ref pathLength);
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
