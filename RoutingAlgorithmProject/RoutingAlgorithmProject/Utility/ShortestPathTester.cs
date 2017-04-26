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
            TestPathFinder(new AStarPathFinder(graph));
            TestPathFinder(new AStarMinHeapPathFinder(graph));
            TestPathFinder(new DijkstraMinHeapPathFinder(graph));
            TestPathFinder(new DijkstraPathFinder(graph));
        }

        /// <summary>
        /// reads from a file then finds a path between every point in the file
        /// k points in file = k(k-1)/2 paths
        /// Creates output file for each implementation with the average time for each path
        /// </summary>
        /// <param name="pf"></param>
        private static void TestPathFinder(PathFinder pf)
        {
            List<StartDestinationPair> testPoints = ReadPointFile(@"..\..\Resources\dcTestPoints.csv");
            string outputPath = System.IO.Path.Combine("output",DateTime.Now.ToString("yyyyMMdd_hhmmss") + pf.GetType().Name + ".txt");
            if (!System.IO.Directory.Exists("output"))
                System.IO.Directory.CreateDirectory("output");
            using (System.IO.TextWriter tw = new System.IO.StreamWriter(outputPath, false))
            {
                double totalRunTime = 0;
                int successfullPathsFound = 0;
                foreach (var startDestinationPair in testPoints)
                {
                    var sw = new Stopwatch();
                    sw.Start();
                    var path = pf.FindShortestPath(startDestinationPair.Start, startDestinationPair.Destination);
                    sw.Stop();
                    if (path != null && path.Count > 0)
                        startDestinationPair.ElapsedTimeSec = sw.Elapsed.TotalSeconds;
                    tw.WriteLine(pf.GetType().Name + " " + startDestinationPair.OutputString() + " with path length = " + path.Count);
                    if (startDestinationPair.ElapsedTimeSec != null)
                    {
                        totalRunTime += (double)startDestinationPair.ElapsedTimeSec;
                        successfullPathsFound++;
                    }
                }
                tw.WriteLine("Average Runtime(sec) = " + totalRunTime / successfullPathsFound);
            }
        }

        private static char[] coordsSplitChar = { ',' };

        private static List<StartDestinationPair> ReadPointFile(string v)
        {
            List<Coordinates> testPoints = new List<Coordinates>();
            using (System.IO.TextReader tr = new System.IO.StreamReader(v))
            {
                string line;
                while ((line = tr.ReadLine()) != null){
                    testPoints.Add(parseCoordinate(line));
                }
            }
            return CreateStartDestinationPair(testPoints);
        }

        private static List<StartDestinationPair> CreateStartDestinationPair(List<Coordinates> testPoints)
        {
            List<StartDestinationPair> pairs = new List<StartDestinationPair>();
            for (int i = 0; i < testPoints.Count-1; i++)
            {
                for (int j = i+1; j < testPoints.Count; j++)
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
            public double? ElapsedTimeSec;

            public StartDestinationPair(Coordinates start, Coordinates destination)
            {
                this.Start = start;
                this.Destination = destination;
            }

            public override string ToString()
            {
                return Start.ToString() + "->" + Destination.ToString();
            }

            public  string OutputString()
            {
                if (ElapsedTimeSec == null)
                    return this.ToString() + "Failed to find path in " + ElapsedTimeSec + " seconds.";
                else
                    return this.ToString() + "Completed in " + ElapsedTimeSec + " seconds.";
            }
        }
    }
}
