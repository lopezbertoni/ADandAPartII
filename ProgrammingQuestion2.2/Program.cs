using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedUtilities;

namespace ProgrammingQuestion2._2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            KruskalMst("TestCase1.txt", 4);
            //KruskalMst("clustering_big.txt", 24);
        }

        private static void KruskalMst(string filename, int bitNumber)
        {
            //Read data in, first element contains number of nodes.
            var g = BuildGraph(filename, bitNumber);
            var size = g[0].Cost;
            //Sort data
            var graph = g.Skip(1).OrderBy(x => x.Cost).ToList();
            var a = new UnionFind(size);
            //var maxV1 = graph.Max(x => x.V1);
            //var maxV2 = graph.Max(x => x.V2);
            foreach (var edge in graph)
            {
                //Passed by index. Need to subtract 1
                if (a.Find(edge.V1 - 1) != a.Find(edge.V2 - 1))
                {
                    a.Union(edge.V1 - 1, edge.V2 - 1);
                }
            }
            Console.WriteLine(String.Join("{0}", a.ClusterCount));
        }

        private static List<EdgeCosts> BuildGraph(string filename, int bitnumber)
        {
            var a = ReadData(filename);
            var c = GetBinaryNumbersWithAtMostThreeOnes(bitnumber);

            var inputData = new List<EdgeCosts>();
            inputData.Add(new EdgeCosts{Cost = a.Count});
            foreach (var edge in a)
            {
                foreach (var i in c)
                {
                    var b = edge.Key ^ i.Key;

                    if (a.ContainsKey(b))
                    {
                        //It's part of the graph
                        inputData.Add(new EdgeCosts { V1 = edge.Value, V2 = a[b], Cost = i.Value });
                    }
                }
            }
            return inputData;
        }

        private static Dictionary<int, int> ReadData(string filename)
        {
            var txtData = File.ReadLines(filename).ToArray();
            var inputData = new Dictionary<int, int>();

            var i = 1;
            foreach (var s in txtData.Skip(1))
            {
                var x = Convert.ToInt32(s.Replace(" ", String.Empty), 2);
                if (!inputData.ContainsKey(x))
                {
                    inputData.Add(x, i);
                    i++;
                }
            }
            return inputData;
        }

        private static Dictionary<int, int> GetBinaryNumbersWithAtMostThreeOnes(int bitnumber)
        {
            var results = new Dictionary<int, int>();
            for (var i = 0; i < Math.Pow(2, bitnumber); i++)
            {
                var s = Convert.ToString(i, 2);
                var count = s.Count(x => x == '1');
                if (count < 3 && count != 0)
                {
                    results.Add(i, count);
                }
            }
            return results;
        }
    }
}
