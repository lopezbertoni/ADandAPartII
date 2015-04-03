using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingQuestion1._2
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

        private static double Schedule(Dictionary<int, Tuple<double, double, double>> graph)
        {
            var ordered = graph.OrderByDescending(x => x.Value.Item3).ThenByDescending(x => x.Value.Item1).ToList();
            double sum = 0;
            double completionTime = 0;
            foreach (var x in ordered)
            {
                completionTime += x.Value.Item2;
                sum += x.Value.Item1 * completionTime;
            }
            return sum;
        }

        private static Dictionary<int, Tuple<double, double, double>> ReadData(string filename)
        {
            var txtData = File.ReadLines(filename).ToArray();
            var inputData = new Dictionary<int, Tuple<double, double, double>>();

            var i = 1;
            foreach (var s in txtData.Skip(1))
            {
                var x = s.Split(' ');
                var w = Convert.ToDouble(x[0]);
                var l = Convert.ToDouble(x[1]);
                inputData.Add(i, new Tuple<double, double, double>(w, l, w/l));

                i++;
            }
            return inputData;
        }
    }
}
