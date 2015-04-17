using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SharedUtilities;

namespace ProgrammingQuestion3._1
{
    class Program
    {
        public static int Iterations = 0;

        static void Main(string[] args)
        {
            //Knapsack("TestCase1.txt");
            //KnapsackR("TestCase1.txt");
            //KnapsackRMemo("TestCase1.txt");

            //Knapsack("TestCase2.txt");
            //KnapsackR("TestCase2.txt");
            //KnapsackRMemo("TestCase2.txt");

            Knapsack("knapsack1.txt");
            //KnapsackR("knapsack1.txt");
            KnapsackRMemo("knapsack1.txt");


            KnapsackRMemo("knapsack_big.txt");
        }

        private static void KnapsackR(string filename)
        {
            var d = ReadData(filename);

            //First line has knapsack size and number of items. 
            //Get data
            var knapsackCapacity = d[0].Value;
            var n = d[0].Weight;
            var ordered = d.Skip(1).OrderBy(x => x.Weight).ToList();
            var values = ordered.Select(x => x.Value).ToArray();
            var weights = ordered.Select(x => x.Weight).ToArray();

            Iterations = 0;
            var r = KnapsackRecursive(weights, values, knapsackCapacity, n);
            Console.WriteLine("Recursive solution => The max value for {0} is  {1}", filename, r);
            Console.WriteLine("This took {0} iterations", Iterations);
        }

        private static void KnapsackRMemo(string filename)
        {
            var d = ReadData(filename);

            //First line has knapsack size and number of items. 
            //Get data
            var knapsackCapacity = d[0].Value;
            var n = d[0].Weight;
            var ordered = d.Skip(1).OrderBy(x => x.Weight).ToList();
            var values = ordered.Select(x => x.Value).ToArray();
            var weights = ordered.Select(x => x.Weight).ToArray();

            //Memoization version
            Iterations = 0;
            var cache = new Dictionary<string, int>();

            var r1 = KnapsackRecursiveMemoization(weights, values, knapsackCapacity, n, cache);
            Console.WriteLine("Recursive memoized => The max value for {0} is  {1}", filename, r1);
            Console.WriteLine("This took {0} iterations", Iterations);
        }

        private static int KnapsackRecursive(int[] weights, int[] values, int w, int n)
        {
            if (w == 0 || n == 0)
            {
                return 0;
            }

            Iterations++;
            if (weights[n - 1] > w)
            {
                return KnapsackRecursive(weights, values, w, n - 1);
            }
            var alpha = values[n - 1] + KnapsackRecursive(weights, values, w - weights[n - 1], n - 1);
            var beta = KnapsackRecursive(weights, values, w, n - 1);
            //Console.WriteLine("w = {0}, n = {1}, value = {2}, alpha = {3}, beta={4}", w, n, Math.Max(alpha, beta), alpha, beta);
            return Math.Max(alpha, beta);
        }

        private static int KnapsackRecursiveMemoization(int[] weights, int[] values, int w, int n, Dictionary<string, int> cache)
        {
            if (w == 0 || n == 0)
            {
                return 0;
            }
            if (!cache.ContainsKey(String.Format("{0}{1}", n,w)))
            {
                if (weights[n - 1] > w)
                {
                    return KnapsackRecursiveMemoization(weights, values, w, n - 1, cache);
                }
                var alpha = values[n - 1] + KnapsackRecursiveMemoization(weights, values, w - weights[n - 1], n - 1, cache);
                var beta = KnapsackRecursiveMemoization(weights, values, w, n - 1, cache);
                //Console.WriteLine("w = {0}, n = {1}, value = {2}, alpha = {3}, beta={4}", w, n, Math.Max(alpha, beta), alpha, beta);
                //cache[n, w] = Math.Max(alpha, beta);
                var result = Math.Max(alpha, beta);
                cache.Add(String.Format("{0}{1}", n, w), result);
                Iterations++;
                return result;
            }
            return cache[String.Format("{0}{1}", n, w)];
        }

        private static void Knapsack(string filename)
        {
            var d = ReadData(filename);

            //First line has knapsack size and number of items. 
            //Get data
            var knapsackCapacity = d[0].Value+1;
            var n = d[0].Weight+1;

            //Initialize array, by default all elements are zeroes. 
            var a = new int[n,knapsackCapacity];

            for (var i = 1; i < n; i++)
            {
                for (var x = 0; x < knapsackCapacity; x++)
                {
                    if (d[i].Weight <= x)
                    {
                        a[i, x] = Math.Max(a[i - 1, x], a[i - 1, x - d[i].Weight] + d[i].Value);
                    }
                }
            }
            Console.WriteLine("Dynamic Programming => The max value for {0} is  {1}", filename, a[n-1, knapsackCapacity-1]);
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
