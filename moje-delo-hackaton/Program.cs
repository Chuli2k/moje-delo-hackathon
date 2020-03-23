using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace moje_delo_hackaton
{
    class Program
    {
        static void Main(string[] args)
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
                var (width, height, wordLengths) = ParseLine(line);
                //Console.WriteLine($"{line} ({wordLengths.Aggregate("", (prev, curr) => prev + " " + curr.ToString())})");
                Console.WriteLine(line);
                results.Add(checker.GetMaxFontSize(width, height, wordLengths));
                //Console.Write($"old:{checker.CheckFontRunCount}; ");
                results.Add(checker.GetMaxFontSizeAlt(width, height, wordLengths));
                //Console.WriteLine($"new:{checker.CheckFontRunCount}");
            }

            Console.WriteLine();
            Console.WriteLine("Izhod:");
            foreach (var result in results)
            {
                Console.WriteLine(result);
            }

            Console.ReadKey();
        }

        /// <summary>
        /// Vrne (width, height, wordLengths[])
        /// </summary>
        /// <param name="line"></param>
        /// <returns>(width, height, wordLengths[])</returns>
        private static (int, int, IList<int>) ParseLine(string line)
        {
            var vseBesede = line.Split(' ');
            var besede = new List<string>(vseBesede);
            besede.RemoveAt(0);
            besede.RemoveAt(0);
            var besedeLength = besede.Select(b => b.Length).ToList();

            return (int.Parse(vseBesede[0]), int.Parse(vseBesede[1]), besedeLength);
        }
    }
}
