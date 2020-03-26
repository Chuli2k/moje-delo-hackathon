using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moje_delo_hackaton
{
    static class Helper
    {
        /// <summary>
        /// Vrne (width, height, wordLengths[])
        /// </summary>
        /// <param name="line"></param>
        /// <returns>(width, height, wordLengths[])</returns>
        public static (int, int, IList<int>) ParseLine(string line)
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
