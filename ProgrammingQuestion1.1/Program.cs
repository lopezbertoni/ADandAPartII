using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingQuestion1._1
{
    class Program
    {
        static void Main(string[] args)
        {
            var t1Result = Schedule(ReadData("TestCase1.txt"));
            Console.WriteLine("The results is: {0}", t1Result);

            var t2Result = Schedule(ReadData("TestCase2.txt"));
            Console.WriteLine("The results is: {0}", t2Result);

            var t3Result = Schedule(ReadData("TestCase3.txt"));
            Console.WriteLine("The results is: {0}", t3Result);

            var realData = Schedule(ReadData("jobs.txt"));
            Console.WriteLine("The results is: {0}", realData);
        }

        private static int Schedule(Dictionary<int, Tuple<int, int, int>> graph)
        {
            var ordered = graph.OrderByDescending(x => x.Value.Item3).ThenByDescending(x => x.Value.Item1).ToList();
            var sum = 0;
            var completionTime = 0;
            foreach (var x in ordered)
            {
                completionTime += x.Value.Item2;
                sum += x.Value.Item1 * completionTime;
            }
            return sum;
        }

        private static Dictionary<int, Tuple<int, int, int>> ReadData(string filename)
        {
            var txtData = File.ReadLines(filename).ToArray();
            var inputData = new Dictionary<int, Tuple<int, int, int>>();

            var i = 1;
            foreach (var s in txtData.Skip(1))
            {
                var x = s.Split(' ');
                var w = Convert.ToInt32(x[0]);
                var l = Convert.ToInt32(x[1]);
                inputData.Add(i, new Tuple<int, int, int>(w, l, w - l));
                
                i++;
            }
            return inputData;
        }

    }
}
