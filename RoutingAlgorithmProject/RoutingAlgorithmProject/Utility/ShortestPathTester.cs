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

        internal static void TestPathFinders(RoutingGraph graph)
        {
            //OsmUtility.TestGraph();
            TestPathFinder(new DijkstraMinHeapPathFinder(graph));
            TestPathFinder(new DijkstraPathFinder(graph));
            TestPathFinder(new AStarPathFinder(graph));
            TestPathFinder(new AStarMinHeapPathFinder(graph));
        }

        private static void TestPathFinder(PathFinder pf)
        {
            List<StartDestinationPair> testPoints = ReadPointFile(@"..\..\Resources\dcTestPointsSmall.csv");
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
                    tw.WriteLine(pf.GetType().Name + " " + startDestinationPair.OutputString());
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

        //private static StartDestinationPair parseStartDestinationPair(string line)
        //{
        //    var coordPairStrings =line.Split(pairSplitChar);
        //    Coordinates start = parseCoordinate(coordPairStrings[0]);
        //    Coordinates destination = parseCoordinate(coordPairStrings[1]);
        //    return new StartDestinationPair(start, destination);
        //}
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
