using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoutingAlgorithmProject.Graph;

namespace RoutingAlgorithmProject.Utility
{
    public class TestResults
    {
        public readonly double Time;
        public readonly int NumberOfNodes;

        public TestResults(double time, int nodes)
        {
            this.Time = time;
            this.NumberOfNodes = nodes;
        }

    }

    public class AlgorithmTestResults
    {
        public readonly string AlgorithmName;

        private List<TestResults> TestResults;
        public List<TestResults> failures;


        private double totalRunTime = 0;
        public int MinPathLength = int.MaxValue;
        public int MaxPathLength = int.MinValue;

        public double[] AverageIntervalRuntimes;
        public static int NUMBEROFINTERVALS = 30;

        public AlgorithmTestResults(string name)
        {
            this.AlgorithmName = name;
            TestResults = new List<Utility.TestResults>();
            failures= new List<Utility.TestResults>();
            AverageIntervalRuntimes = new double[30];
        }

        public void AddTestResult(TestResults tr)
        {
            if (tr.NumberOfNodes > 0)
            {
                TestResults.Add(tr);
                MinPathLength = Math.Min(MinPathLength, tr.NumberOfNodes);
                MaxPathLength = Math.Max(MaxPathLength, tr.NumberOfNodes);
                totalRunTime += tr.Time;
            }
            else
            {
                failures.Add(tr);
            }
        }

        public List<TestResults> GetTestResults()
        {
            return TestResults;
        }

        public double GetAverageRuntime()
        {
            return totalRunTime / TestResults.Count;
        }

        public void CalculateIntervalRuntimes()
        {
            foreach (var r in TestResults)
            {
                AverageIntervalRuntimes[GetIntervalIndex(r.NumberOfNodes)] += r.Time;
            }
        }

        public double GetIntervalLength()
        {
            return (double)(MaxPathLength - MinPathLength) / NUMBEROFINTERVALS;
        }

        private int GetIntervalIndex(int length)
        {
            int shiftedLength = length - MinPathLength;
            if (length == MaxPathLength)
                return NUMBEROFINTERVALS-1;
            else
                return (int)(shiftedLength / GetIntervalLength());
        }
    }

    public class GraphTestResults
    {
        private List<AlgorithmTestResults> AlgorithmTestResults;
        public readonly string GraphName;
        private RoutingGraph _graph;


        public RoutingGraph Graph
        {
            get
            {
                return _graph;
            }
            set
            {
                _graph = value;
            }
        }

        public GraphTestResults(string name)
        {
            this.GraphName = name;
            AlgorithmTestResults = new List<Utility.AlgorithmTestResults>();
        }

        public void AddAlgorithmTestResult(AlgorithmTestResults atr)
        {
            atr.CalculateIntervalRuntimes();
            this.AlgorithmTestResults.Add(atr);
        }

        public List<AlgorithmTestResults> GetAlgorithmTestResults()
        {
            return AlgorithmTestResults;
        }

        
    }
}
