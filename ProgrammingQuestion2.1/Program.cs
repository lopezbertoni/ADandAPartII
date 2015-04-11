using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedUtilities;

namespace ProgrammingQuestion2._1
{
    class Program
    {
        static void Main(string[] args)
        {
            KruskalMst("TestCase1.txt");
        }

        private static void KruskalMst(string filename)
        {
            //Read data in - data is sorted
            var graph = ReadData(filename).OrderBy(x => x.Cost).ToList();

            var a = new UnionFind(graph.Count);
            var mst = new List<string>();
            foreach (var edge in graph)
            {
                //Passed by index. Need to subtract 1
                if (a.Find(edge.V1) != a.Find(edge.V2))
                {
                    mst.Add(String.Format("{0},{1}", edge.V1, edge.V2));
                    a.Union(edge.V1, edge.V2);
                }
            }
            var result = mst;
        }

        private static List<EdgeCosts> ReadData(string filename)
        {
            var txtData = File.ReadLines(filename).ToArray();
            var inputData = new List<EdgeCosts>();

            foreach (var s in txtData.Skip(1))
            {
                var x = s.Split(' ');
                inputData.Add(new EdgeCosts { V1 = Convert.ToInt32(x[0]), V2 = Convert.ToInt32(x[1]), Cost = Convert.ToInt32(x[2]) });
            }
            return inputData;
        }

    }
}
