using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using BenchmarkDotNet.Running;

namespace moje_delo_hackaton
{
    class Program
    {
        static void Main(string[] args)
        {
            DisplayHackathonOutput();

            //BenchmarkTest();

            Console.ReadKey();
        }

        private static void BenchmarkTest()
        {
            var summary = BenchmarkRunner.Run<DisplayCheckerBenchmark>();
        }

        private static void DisplayHackathonOutput()
        {
            var inputFilePath = "vhodi.txt";
            //var inputFilePath = "vhodi-test.txt";

            if (!File.Exists(inputFilePath))
            {
                Console.WriteLine($"Datoteka {inputFilePath} ne obstaja");
                return;
            }

            var lines = File.ReadAllLines(inputFilePath);
            var results = new List<int>();
            var checker = new DisplayChecker();
            Console.WriteLine("Vhod:");
            foreach (var line in lines)
            {
                var (width, height, wordLengths) = Helper.ParseLine(line);
                //Console.WriteLine($"{line} ({wordLengths.Aggregate("", (prev, curr) => prev + " " + curr.ToString())})");
                Console.WriteLine(line);

                results.Add(checker.GetMaxFontSize(width, height, wordLengths));
                //Console.Write($"old:{checker.CheckFontRunCount}; ");

                //Alternativna metoda. Večkrat se preverja velikost fonta, samo je brez računanja korena...
                //results.Add(checker.GetMaxFontSizeAlt(width, height, wordLengths));
                //Console.WriteLine($"new:{checker.CheckFontRunCount}");,
            }

            Console.WriteLine();
            Console.WriteLine("Izhod:");
            foreach (var result in results)
            {
                Console.WriteLine(result);
            }
        }
    }
}
