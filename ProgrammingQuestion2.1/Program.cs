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
        }

        private static List<EdgeCosts> ReadData(string filename)
        {
            var txtData = File.ReadLines(filename).ToArray();
            var inputData = new List<EdgeCosts>();

            foreach (var s in txtData.Skip(1))
            {
                var x = s.Split(' ');
                inputData.Add(new EdgeCosts { V1 = Convert.ToInt32(x[0]), V2 = Convert.ToInt32(x[1]), Cost = Convert.ToInt32(x[1]) });
            }
            return inputData.OrderBy(x => x.Cost).ToList();
        }

    }
}
