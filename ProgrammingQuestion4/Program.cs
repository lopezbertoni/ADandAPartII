using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph;
using QuickGraph.Algorithms;
using QuickGraph.Algorithms.Observers;
using QuickGraph.Algorithms.ShortestPath;
using SharedUtilities;

namespace ProgrammingQuestion4
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestCase1
            BellmanFord2("TestCase1.txt");
            BellmanFord2("TestCase2.txt");
            BellmanFord2("TestCase3.txt");

            BellmanFord2("TestCase4.txt");
            BellmanFord2("TestCase5.txt");
            BellmanFord2("TestCase6.txt");
            BellmanFord2("TestCase7.txt");
            BellmanFord2("TestCase8.txt");

            BellmanFord2("g1.txt");
            BellmanFord2("g2.txt");
            BellmanFord2("g3.txt");
            //BellmanFord2("large.txt");
        }

        private static AdjacencyGraph<double, Edge<double>> _graph;
        private static Dictionary<Edge<double>, double> _edgeCost; 
        public static void BellmanFord2(string filename)
        {
            //EdgeCost.Clear();
            ReadDataQuickGraph(filename);

            //Get algorithm
            var edgeCost = AlgorithmExtensions.GetIndexer(_edgeCost);
            //var tryGetPath = _graph.ShortestPathsBellmanFord(edgeCost, 1);

            //IEnumerable<Edge<int>> path;
            //if (tryGetPath(1000, out path))
            //    foreach (var edge in path)
            //        Console.WriteLine(edge);


            var bellManFord = new BellmanFordShortestPathAlgorithm<double, Edge<double>>(_graph, edgeCost);
            var distObserver = new VertexDistanceRecorderObserver<double, Edge<double>>(edgeCost);
            distObserver.Attach(bellManFord);
            bellManFord.Compute(1);

            var minDistance = distObserver.Distances.OrderBy(x => x.Value).Select(x => x.Value).Take(2).Min();
            Console.WriteLine("The minimum distance for {0} is {1}", filename, minDistance);
            Console.WriteLine(bellManFord.FoundNegativeCycle ? "There are negative cycles. " : "There are NO negative cycles. ");

        }

        public static void ReadDataQuickGraph(string filename)
        {
            _graph = new AdjacencyGraph<double, Edge<double>>();
            var txtData = File.ReadLines(filename).ToArray();

            _edgeCost = new Dictionary<Edge<double>, double>(Convert.ToInt32(txtData.First().Split(' ')[1]));
            foreach (var s in txtData.Skip(1))
            {
                var x = s.Split(' ');
                var source = Convert.ToDouble(x[0]);
                var target = Convert.ToDouble(x[1]);
                var edge = new Edge<double>(source, target);
                var cost = Convert.ToDouble(x[2]);
                _graph.AddVerticesAndEdge(edge);
                _edgeCost.Add(edge, cost);
            }

        }

        public static void BellmanFord(string filename)
        {
            Console.WriteLine("Processing file {0}", filename);
            //Step 1, read data in
            var graph = ReadData(filename);

            //Step 2, initialize data
            var numberOfEdges = graph.First().Value.First().Cost+1;
            var numberOfVertices = graph.First().Value.First().Tail+1;
            var distances = new int[numberOfVertices];
            //var predecessor = new int[graph.Count];
            //Clean metadata
            graph.Remove(0);
            var a = new int[numberOfVertices, numberOfEdges];
            for (var i = 0; i < numberOfVertices; i++)
            {
                distances[i] = int.MaxValue;
                a[i, 0] = int.MaxValue;
            }
            distances[0] = 0; //Setting source to 0
            a[0, 0] = 0;

            var listOfSource = graph.First();
            var listOfFakeEdges = new List<Edge>();
            //Add fake vertex. 
            foreach (var g in graph)
            {
                listOfFakeEdges.Add(new Edge { Tail = g.Key, Cost = 0 });
            }
            graph[listOfSource.Key] = listOfFakeEdges;
            graph.Add(numberOfVertices, listOfSource.Value);

            var orderedGraph = graph.OrderBy(x => x.Key).ToList();

            //Step 3, calculate distances
            for (var i = 1; i < numberOfEdges; i++)
            {
                foreach (var edges in orderedGraph)
                {
                    //Grab edges from v
                    foreach (var edge in edges.Value)
                    {
                        var distanceSource = distances[edges.Key - 1];
                        //Check if source is not infinity.
                        if (distanceSource != int.MaxValue)
                        {
                            //Try to update Tail 
                            var cost = edge.Cost;
                            var distanceDestination = distances[edge.Tail - 1];
                            var tentativeDistance = (distanceSource + cost);
                            if (tentativeDistance < distanceDestination)
                            {
                                distances[edge.Tail - 1] = tentativeDistance;
                                a[edge.Tail - 1, i] = tentativeDistance;
                            }
                        }
                    }
                }
            }

            var negativeCycles = false;
            //Step 4, check for negative cost cycle.
            foreach (var edges in graph)
            {
                //Grab edges from v
                foreach (var edge in edges.Value)
                {
                    var distanceSource = distances[edges.Key - 1];
                    var distanceTarget = distances[edge.Tail - 1];
                    if (distanceTarget > distanceSource + edge.Cost)
                    {
                        negativeCycles = true;
                        break;
                    }

                }
            }

            Console.WriteLine(negativeCycles ? "There are negative cycles. " : "There are NO negative cycles. ");

            var minDistance = distances.Min();
            Console.WriteLine("The minimum distance is {0}", minDistance);

        }

        public static Dictionary<int, List<Edge>> ReadData(string filename)
        {
            var inputData = new Dictionary<int, List<Edge>>();
            var txtData = File.ReadLines(filename).ToArray();

            //Metadata
            var metaData = txtData.First().Split(' ');
            var numberVertices = Convert.ToInt32(metaData[0]);
            var numberOfEdges = Convert.ToInt32(metaData[1]);
            inputData.Add(0, new List<Edge>{new Edge{Tail = numberVertices, Cost = numberOfEdges}});

            var edgeCount = 0;
            foreach (var s in txtData.Skip(1))
            {
                var x = s.Split(' ');
                var key = Convert.ToInt32(x[0]);
                if (inputData.ContainsKey(key))
                {
                    inputData[key].Add(new Edge{Tail = Convert.ToInt32(x[1]), Cost = Convert.ToInt32(x[2])});
                }
                else
                {
                    inputData.Add(key, new List<Edge> { new Edge { Tail = Convert.ToInt32(x[1]), Cost = Convert.ToInt32(x[2]) } });
                }
                edgeCount++;
            }
            //First line contains file edge count data. 
            if (numberOfEdges != txtData.Count() - 1)
            {
                throw new Exception();
            }
            return inputData;
        }
    }
}
