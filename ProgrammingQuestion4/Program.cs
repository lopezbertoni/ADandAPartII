using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedUtilities;

namespace ProgrammingQuestion4
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestCase1 answer is 4 (s to t, 5 to 6)
            BellmanFord("TestCase1.txt");
        }

        public static void BellmanFord(string filename)
        {
            Console.WriteLine("Processing file {0}", filename);
            //Step 1, read data in
            var graph = ReadData(filename);

            //Step 2, initialize data
            var distances = new int[graph.Count];
            var predecessor = new int[graph.Count];
            for (var i = 0; i < distances.Count(); i++)
            {
                distances[i] = int.MaxValue;
            }
            distances[graph.First().Key - 1] = 0; //Setting source to 0

            //Step 3, calculate distances
            for (var i = 1; i <= graph.Count; i++)
            {
                //Check if we 
                if (graph.ContainsKey(i))
                {
                    //Loop through edges of V as head
                    foreach (var edge in graph[i])
                    {
                        if (edge.Tail - 1 < graph.Count)
                        {
                            if (distances[i - 1]  + edge.Cost < distances[edge.Tail - 1])
                            {
                                distances[edge.Tail - 1] = distances[i - 1] + edge.Cost;
                                predecessor[edge.Tail - 1] = i - 1;
                            }
                        }
                    }
                }
            }

            //Step 4, check for negative edge weights.
        }

        public static Dictionary<int, List<Edge>> ReadData(string filename)
        {
            var inputData = new Dictionary<int, List<Edge>>();
            var txtData = File.ReadLines(filename).ToArray();

            //Sanity Check
            var numberOfEdges = Convert.ToInt32(txtData.First().Split(' ')[1]);

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

        public static Dictionary<Edge, int> ReadDataHeadTailKey(string filename)
        {
            var inputData = new Dictionary<Edge, int>();
            var txtData = File.ReadLines(filename).ToArray();

            //Sanity Check
            var numberOfEdges = Convert.ToInt32(txtData.First().Split(' ')[1]);
            var edgeCount = 0;
            
            foreach (var s in txtData.Skip(1))
            {
                var x = s.Split(' ');
                
                inputData.Add(new Edge{Head = Convert.ToInt32(x[0]), Tail = Convert.ToInt32(x[1])}, Convert.ToInt32(x[2]));
                
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
