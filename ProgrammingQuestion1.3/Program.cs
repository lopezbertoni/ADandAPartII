using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingQuestion1._3
{
    class Program
    {
        private static readonly HashSet<int> Visited = new HashSet<int>();
        private static readonly Stack<int> VisitOrder = new Stack<int>(); 
        private static List<Tuple<int,int>> NodesToVisit = new List<Tuple<int,int>>();  

        static void Main(string[] args)
        {
            var t1Result = PrimsMst(ReadData("TestCase1.txt"));

            var t2Result = PrimsMst(ReadData("TestCase3.txt"));

            var realData = PrimsMst(ReadData("edges.txt"));

        }

        private static int PrimsMst(Dictionary<int, List<Tuple<int, int>>> graph)
        {
            Visited.Clear();
            VisitOrder.Clear();
            NodesToVisit.Clear();
            var sum = 0;
            var valueToAdd = 0;
            var v = graph.First().Key; //Get initial node
            VisitOrder.Push(v);

            while (VisitOrder.Count > 0)
            {
                //Get node
                var vertex = VisitOrder.Pop();
                Visited.Add(vertex);

                //Mark as visited in NodesToVIsit
                //var tuple = NodesToVisit.FirstOrDefault(x => x.Item1 == v && !x.Item3);
                //if (tuple != null)
                //{
                //    NodesToVisit.Remove(tuple);
                //}

                //Ordered by cost
                var currentNodeNeighbors = graph[vertex].ToList();//.OrderBy(x => x.Item2).ToList();
                //Loop through node edges
                //var nextVertex = 0;
                //valueToAdd = 0;
                var candidates = currentNodeNeighbors.Where(currentNodeNeighbor => !Visited.Contains(currentNodeNeighbor.Item1)).ToList();
                candidates.AddRange(NodesToVisit.Where(currentNodeNeighbor => !Visited.Contains(currentNodeNeighbor.Item1)));
                //var candidates = currentNodeNeighbors;
                //candidates.AddRange(NodesToVisit);
                //candidates.OrderBy(x => x.Item2);
                var i = 0;
                foreach (var currentNodeEdge in candidates.OrderBy(x => x.Item2))
                {
                    //Check if node has been visited
                    if (!Visited.Contains(currentNodeEdge.Item1))
                    {
                        if (i == 0)
                        {
                            VisitOrder.Push(currentNodeEdge.Item1);
                            sum += currentNodeEdge.Item2;                            
                        }
                        else
                        {
                            NodesToVisit.Add(new Tuple<int, int>(currentNodeEdge.Item1, currentNodeEdge.Item2));
                        }
                        i++;
                        //Node has not been visited. 
                        //Add to next to visit list
                        //NodesToVisit.Add(new Tuple<int, int>(currentNodeEdge.Item1, currentNodeEdge.Item2));
                        //VisitOrder.Push(currentNodeEdge.Item1);
                        //sum += currentNodeEdge.Item2;
                        //break;
                        //valueToAdd = currentNodeEdge.Item2;
                        //nextVertex = currentNodeEdge.Item1;
                    }
                }
                NodesToVisit = NodesToVisit.Distinct().ToList();
                //Pick next node to examine
                //if (NodesToVisit.Any())
                //{
                //    //We still have nodes to visit
                //    var nextNode = NodesToVisit.OrderBy(x => x.Item2).First();
                //    VisitOrder.Push(nextNode.Item1);
                //    sum += nextNode.Item2;
                //    NodesToVisit.Remove(nextNode);
                //}
                //if (nextVertex > 0)
                //{
                //    VisitOrder.Push(nextVertex);
                //    sum += valueToAdd;
                //}
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
                    e.Add(new Tuple<int, int>(node2, edgeCost));
                    inputData[node1] = e;
                }
                else
                {
                    //Node already present
                    inputData.TryGetValue(node1, out e);
                    //Sort by key
                    e.Add(new Tuple<int, int>(node2, edgeCost));
                    inputData[node1] = e;
                }
                //Add both ways
                var e1 = new List<Tuple<int, int>>();
                if (!inputData.ContainsKey(node2))
                {
                    //Add it to graph
                    e1.Add(new Tuple<int, int>(node1, edgeCost));
                    inputData[node2] = e1;
                }
                else
                {
                    //Node already present
                    inputData.TryGetValue(node2, out e1);
                    //Sort by key
                    e1.Add(new Tuple<int, int>(node1, edgeCost));
                    inputData[node2] = e1;
                }
            }
            return inputData;
        }

    }
}
