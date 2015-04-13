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
            //KruskalMst("TestCase1.txt");
            SingleLinkClustering("TestCase1.txt", 3);
            SingleLinkClustering("TestCase1.txt", 4);

            SingleLinkClustering("TestCase2.txt", 3);
            SingleLinkClustering("TestCase2.txt", 4);

            SingleLinkClustering("TestCase3.txt", 2);
            SingleLinkClustering("TestCase3.txt", 3);
            SingleLinkClustering("TestCase3.txt", 4);

            SingleLinkClustering("TestCase4.txt", 2);
            SingleLinkClustering("TestCase4.txt", 3);
            SingleLinkClustering("TestCase4.txt", 4);

            SingleLinkClustering("clustering1.txt", 4);
        }

        private static void SingleLinkClustering(string filename, int k)
        {
            //Read data in, first element contains number of nodes.
            var g = ReadData(filename);
            var size = g[0].Cost;
            if (size > k)
            {
                //Sort data
                var graph = g.Skip(1).OrderBy(x => x.Cost).ToList();
                var a = new UnionFind(size);
                var i = 0;
                var d = 0;
                while (a.ClusterCount >= k)
                {
                    //Console.WriteLine("Current cluster size is: {0}", a.ClusterCount);
                    var edge = graph[i];
                    //Passed by index. Need to subtract 1
                    if (a.Find(edge.V1 - 1) != a.Find(edge.V2 - 1))
                    {
                        //Console.WriteLine(String.Format("Merging {0},{1}", edge.V1, edge.V2));
                        a.Union(edge.V1 - 1, edge.V2 - 1);
                        d = edge.Cost;
                    }
                    i++;
                }
                //Console.WriteLine(a.GetInfo());
                Console.WriteLine(String.Format("The maximum spacing for {0} - clustering in {1} is: {2}", k, filename, d));
            }
        }

        private static void KruskalMst(string filename)
        {
            //Read data in, first element contains number of nodes.
            var g = ReadData(filename);
            var size = g[0].Cost;
            //Sort data
            var graph = g.Skip(1).OrderBy(x => x.Cost).ToList();
            var a = new UnionFind(size);
            var mst = new List<string>();
            foreach (var edge in graph)
            {
                //Passed by index. Need to subtract 1
                if (a.Find(edge.V1-1) != a.Find(edge.V2-1))
                {
                    mst.Add(String.Format("{0},{1}", edge.V1, edge.V2));
                    a.Union(edge.V1-1, edge.V2-1);
                }
            }
            Console.WriteLine(String.Join(" | ", mst));
        }

        private static List<EdgeCosts> ReadData(string filename)
        {
            var txtData = File.ReadLines(filename).ToArray();
            var inputData = new List<EdgeCosts>();

            inputData.Add(new EdgeCosts{Cost = Convert.ToInt32(txtData[0])});
            foreach (var s in txtData.Skip(1))
            {
                var x = s.Split(' ');
                inputData.Add(new EdgeCosts { V1 = Convert.ToInt32(x[0]), V2 = Convert.ToInt32(x[1]), Cost = Convert.ToInt32(x[2]) });
            }
            return inputData;
        }

    }
}
