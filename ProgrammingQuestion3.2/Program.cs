using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedUtilities;

namespace ProgrammingQuestion3._2
{
    class Program
    {
        static void Main(string[] args)
        {
        }

        private static int Knapsack(string filename)
        {
            var d = ReadData(filename);

            //First line has knapsack size and number of items. 
            //Get data
            var knapsackCapacity = d[0].Value + 1;
            var n = d[0].Weight + 1;
            var values = d.Skip(1).Select(x => x.Value).ToArray();
            var weights = d.Skip(1).Select(x => x.Weight).ToArray();

            return KnapsackRecursive(weights, values, knapsackCapacity, n);
        }

        private static int KnapsackRecursive(int[] weights, int[] values, int w, int n)
        {
            if (w == 0 || n ==0)
            {
                return 0;
            }

            if (weights[n-1] > w)
            {
                return KnapsackRecursive(weights, values, w, n - 1);
            }

            var alpha = values[n-1] + KnapsackRecursive(weights, values, w - weights[n-1], n-1);
            var beta = KnapsackRecursive(weights, values, w, n - 1);
            return Math.Max(alpha, beta);
        }

        private static List<KnapsackItems> ReadData(string filename)
        {
            var txtData = File.ReadLines(filename).ToArray();
            var inputData = new List<KnapsackItems>();

            foreach (var s in txtData)
            {
                var x = s.Split(' ');
                inputData.Add(new KnapsackItems { Value = Convert.ToInt32(x[0]), Weight = Convert.ToInt32(x[1]) });
            }
            return inputData;
        }
    }
}
