using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingQuestion1._3
{
    class Program
    {
        private static readonly HashSet<int> Visited = new HashSet<int>();
        private static readonly Stack<int> VisitOrder = new Stack<int>(); 

        static void Main(string[] args)
        {
            var t1Result = ReadData("TestCase1.txt");

            var t2Result = PrimsMst(ReadData("TestCase3.txt"));

            var realData = ReadData("edges.txt");

        }

        private static int PrimsMst(Dictionary<int, List<Tuple<int, int>>> graph)
        {
            var sum = 0;

            var v = graph.First().Key; //Get initial node
            VisitOrder.Push(v);

            while (VisitOrder.Count > 0)
            {
                //Get node
                var vertex = VisitOrder.Pop();
                Visited.Add(vertex);

                //Ordered by cost
                var currentNodeNeighbors = graph[vertex].OrderByDescending(x => x.Item1).ToList();
                //Loop through node edges
                foreach (var currentNodeEdge in currentNodeNeighbors)
                {
                    //Check if node has been visited
                    if (!Visited.Contains(currentNodeEdge.Item2))
                    {
                        //Node has not been visited. 
                        VisitOrder.Push(currentNodeEdge.Item2);
                    }
                }
                if (currentNodeNeighbors.Any())
                {
                    sum += currentNodeNeighbors.Last().Item1;
                }
            }

            return sum;
        }

        private static Dictionary<int, List<Tuple<int, int>>> ReadData(string filename)
        {
            var txtData = File.ReadLines(filename).ToArray();
            var inputData = new Dictionary<int, List<Tuple<int, int>>>();

            foreach (var s in txtData.Skip(1))
            {
                var x = s.Split(' ');
                var node1 = Convert.ToInt32(x[0]);
                var node2 = Convert.ToInt32(x[1]);
                var edgeCost = Convert.ToInt32(x[2]);

                var e = new List<Tuple<int, int>>();
                if (!inputData.ContainsKey(node1))
                {
                    //Add it to graph
                    e.Add(new Tuple<int, int>(edgeCost, node2));
                    inputData[node1] = e;
                }
                else
                {
                    //Node already present
                    inputData.TryGetValue(node1, out e);
                    //Sort by key
                    e.Add(new Tuple<int, int>(edgeCost, node2));
                    inputData[node1] = e;
                }
                //Add both ways
                var e1 = new List<Tuple<int, int>>();
                if (!inputData.ContainsKey(node2))
                {
                    //Add it to graph
                    e1.Add(new Tuple<int, int>(edgeCost, node1));
                    inputData[node2] = e1;
                }
                else
                {
                    //Node already present
                    inputData.TryGetValue(node2, out e1);
                    //Sort by key
                    e1.Add(new Tuple<int, int>(edgeCost, node1));
                    inputData[node2] = e1;
                }
            }
            return inputData;
        }

    }
}
