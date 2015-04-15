using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedUtilities;

namespace ProgrammingQuestion3._1
{
    class Program
    {
        static void Main(string[] args)
        {
            Knapsack("TestCase1.txt");

        }

        private static void Knapsack(string filename)
        {
            var d = ReadData(filename);

            //First line has knapsack size and number of items. 
            //Get data
            var knapsackCapacity = d[0].Value;
            var n = d[0].Weight;

            //Initialize array
            var a = new int[n,knapsackCapacity];
            //for (var j = 0; j < knapsackCapacity; j++)
            //{
            //    a[0,j] = 0;
            //}

            for (var i = 1; i < n; i++)
            {
                for (var x = 0; x < knapsackCapacity; x++)
                {
                    a[i, x] = Math.Max(a[i - 1, x], a[i - 1, x - d[i].Weight] + d[i].Value);
                }
            }
            Console.WriteLine("The max value is  {0}", a[n, knapsackCapacity]);
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
