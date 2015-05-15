using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace ProgrammingQuestion6
{
    internal class Program
    {
        private static Random randNum;
        private static void Main(string[] args)
        {
            randNum = new Random();

            //Papadimitriou("TestCase1.txt");
            //Papadimitriou("TestCase2.txt");
            //ReadData("TestCase2.txt");
            Papadimitriou("2sat1.txt");
            Papadimitriou("2sat2.txt");
            Papadimitriou("2sat3.txt");
            Papadimitriou("2sat4.txt");
            Papadimitriou("2sat5.txt");
            Papadimitriou("2sat6.txt");
        }

        private static void Papadimitriou(string filename)
        {
            var inputData = ReadData(filename);
            var n = inputData.Count;

            var outerLoop = Math.Round(Math.Log(n, 2));
            var innerloop = 2*n ^ 2;
            var status = "Failed";

            for (var i = 0; i < outerLoop; i++)
            {
                if (status != "Failed")
                {
                    //Console.WriteLine("{0} Passed", filename);
                    break;
                }
                var assignment = GetRandomAssignment(inputData);
                for (var j = 0; j < innerloop; j++)
                {
                    var numberPassed = 0;
                    var pass = true;
                    foreach (var clause in inputData)
                    {
                        var ac1 = assignment[Math.Abs(clause.C1)];
                        var ac2 = assignment[Math.Abs(clause.C2)];

                        if (clause.C1 < 0)
                        {
                            ac1 = !ac1;
                        }
                        if (clause.C2 < 0)
                        {
                            ac2 = !ac2;
                        }

                        pass = ac1 || ac2;
                        if (pass)
                        {
                            numberPassed++;
                        }
                        else
                        {
                            assignment[Math.Abs(clause.C1)] = !assignment[Math.Abs(clause.C1)];
                            break;
                        }
                    }
                    if (numberPassed != n)
                    {
                        //Console.WriteLine("Failed");
                    }
                    else
                    {
                        status = "Passed";
                        //Console.WriteLine("Passed");
                        break;
                    }
                }
            }
            Console.WriteLine("{0} {1}",filename, status);
        }

        private static List<Clause> ReadData(string filename)
        {
            var inputData = new List<Clause>();
            var txtData = File.ReadLines(filename).ToArray();

            foreach (var s in txtData.Skip(1))
            {
                var x = s.Split(' ');
                var c1 = Convert.ToInt32(x[0]);
                var c2 = Convert.ToInt32(x[1]);
                inputData.Add(new Clause {C1 = c1, C2 = c2, Filter = false});
            }
            //All data in place, now add filter
            foreach (var clause in inputData)
            {
                var c1 = inputData.Where(x => x.C1 == -clause.C1 || x.C2 == -clause.C1).ToList();
                var c2 = inputData.Where(x => x.C1 == -clause.C2 || x.C2 == -clause.C2).ToList();

                if (!c1.Any())
                {
                    var f = inputData.Where(x => x.C1 == clause.C1 || x.C2 == clause.C1).ToList();
                    foreach (var clause1 in f)
                    {
                        //var g = inputData.Where(x => x.C1 == clause1.C1 && x.C2 == clause1.C2).First();
                        clause1.Filter = true;
                    }
                }
                if (!c2.Any())
                {
                    var f = inputData.Where(x => x.C1 == clause.C2 || x.C2 == clause.C2).ToList();
                    foreach (var clause1 in f)
                    {
                        //var g = inputData.Where(x => x.C1 == clause1.C1 && x.C2 == clause1.C2).First();
                        clause1.Filter = true;
                    }
                }
            }

            var t = inputData.Where(x => !x.Filter);
            return inputData;
        }
        
        private static Dictionary<int, bool> GetRandomAssignment(IEnumerable<Clause> inputData)
        {
            var assignment = new Dictionary<int, bool>();

            var min = 0;
            var max = 100;
            
            foreach (var clause in inputData)
            {
                var key1 = Math.Abs(clause.C1);
                var key2 = Math.Abs(clause.C2);

                //Console.WriteLine(t);
                var boolValue = randNum.Next(min, max) % 2 == 0;
                if (!assignment.ContainsKey(key1))
                {
                    assignment.Add(key1, boolValue);
                }
                if (!assignment.ContainsKey(key2))
                {
                    assignment.Add(key2, boolValue);
                }
            }

            return assignment;
        }


        //private static void ReadData(string filename)
        //{
        //    var txtData = File.ReadLines(filename).ToArray();
        //    Console.WriteLine(String.Format("{0} with {1} conditions", filename, txtData.First()));

        //    var inputData = new Dictionary<int, int>();
        //    var inputDataRev = new Dictionary<int, int>();
        //    //var inputData = new List<Clause>();
        //    foreach (var s in txtData.Skip(1))
        //    {
        //        var x = s.Split(' ');
        //        var c1 = Convert.ToInt32(x[0]);
        //        var c2 = Convert.ToInt32(x[1]);
        //        //inputData.Add(new Clause {C1 = c1, C2 = c2});
        //        //inputDataDict.Add(c1, c2);
        //        if (inputData.ContainsKey(c1))
        //        {
        //            if (!inputData.ContainsKey(c2))
        //            {
        //                inputData.Add(c2, c1);
        //            }
        //        }
        //        else
        //        {
        //            inputData.Add(c1, c2);
        //        }
        //        if (inputDataRev.ContainsKey(c2))
        //        {
        //            if (!inputDataRev.ContainsKey(c1))
        //            {
        //                inputDataRev.Add(c1, c2);
        //            }
        //        }
        //        else
        //        {
        //            inputDataRev.Add(c2, c1);
        //        }
        //    }
        //    var filteredList = DeleteIfPossible(inputData, inputDataRev);

        //    Console.WriteLine(String.Format("{0} with {1} conditions after pre-processing", filename, filteredList.Count));
        //}

        private static Dictionary<int, int> DeleteIfPossible(Dictionary<int, int> inputData, Dictionary<int, int> inputDataRev)
        {
            var filteredList = new Dictionary<int, int>();


            foreach (var kvp in inputData)
            {
                if ((inputData.ContainsKey(-kvp.Key) || inputDataRev.ContainsKey(-kvp.Key)) && ((inputData.ContainsKey(-kvp.Value) || inputDataRev.ContainsKey(-kvp.Value))))
                {
                    filteredList.Add(kvp.Key, kvp.Value);
                }
            }

            //foreach (var kvp in inputData)
            //{
            //    if (inputData.ContainsKey(-kvp.Key))
            //    {
            //        filteredList.Add(kvp.Key, kvp.Value);
            //    }
            //}

            //var secondFilter = new Dictionary<int, int>();
            //foreach (var kvp in filteredList)
            //{
            //    if (inputDataRev.ContainsKey(-kvp.Key))
            //    {
            //        secondFilter.Add(kvp.Key, kvp.Value);
            //    }
            //}
            return filteredList;
        }
    }
}
