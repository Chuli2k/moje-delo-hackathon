using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

/*
 * To še preveri:
 * - maxD(stV) = Maksimalna velikost pisave glede na dolžino besedila, število besed in trenutno število vrstic:
 * -- Seštej dolžine vseh besed
 * -- Prištej število presledkov: <št. presledkov> = <št. vseh besed> - 1 - ( <št. vrstic> - 1 )
 * -- Površino ekrana (širina * št. vrstic) deli z prejšnim rezultatom.
 * 
 */

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
            foreach (var line in lines)
            {
                var besede = new List<string>(line.Split(' '));
                besede.RemoveAt(0);
                besede.RemoveAt(0);
                var besedeLength = besede.Select(b => b.Length).ToList();
                Console.WriteLine($"{line} ({besedeLength.Aggregate("", (prev, curr) => prev + " " + curr.ToString())})");

                var (width, height, wordLengths) = ParseLine(line);
                var f = GetMaxFontSize(width, height, wordLengths.ToList());
                Console.WriteLine($"Result:{f}");
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Vrne (width, height, wordLengths[])
        /// </summary>
        /// <param name="line"></param>
        /// <returns>(width, height, wordLengths[])</returns>
        private static (int, int, IEnumerable<int>) ParseLine(string line)
        {
            var vseBesede = line.Split(' ');
            var besede = new List<string>(vseBesede);
            besede.RemoveAt(0);
            besede.RemoveAt(0);
            var besedeLength = besede.Select(b => b.Length).ToList();

            return (int.Parse(vseBesede[0]), int.Parse(vseBesede[1]), besedeLength);
        }

        private static int GetMaxFontSize(int width, int height, IList<int> wordLengths)
        {
            //Najdi min stranico ekrana in nastavi to kot navečjo možno velikost font-a
            var maxFontSize = width < height ? width : height;

            //Najdi max velikost ekrana glede na najdaljšo besedo, če je manjša kot prej, jo nastavi
            int maxfontSizeByMaxWordLength = width / wordLengths.Max(); //Celi del deljenja
            if (maxfontSizeByMaxWordLength < maxFontSize) maxFontSize = maxfontSizeByMaxWordLength;

            Console.WriteLine($"init maxFontSize:{maxFontSize}");

            while(maxFontSize > 0)
            {
                if (CheckFontSize(width, height, wordLengths, maxFontSize))
                    return maxFontSize;

                maxFontSize--;
            }

            return maxFontSize;
        }

        /// <summary>
        /// Preveri, če se podana velikost pisave da prikazat za podane dimenzije ekrana in velikosti besed
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="wordLengths"></param>
        /// <param name="maxFontSize"></param>
        /// <returns></returns>
        private static bool CheckFontSize(int width, int height, IList<int> wordLengths, int maxFontSize)
        {
            //Spremenjljivki za trenutni pozicijo
            var x = 0; //pixel na horizontali
            var y = 0; //V kateri vrstici sem...
            var nrSpaces = -1; //-1 ker, če je samo ena beseda, pomeni, da ni presledkov...
            
            //Normalizirana širina in višina glede na velikost pisave.
            int normWidth = width / maxFontSize;
            int normHeight = height / maxFontSize;

            for (int i = 0; i < wordLengths.Count; i++)
            {
                x += wordLengths[i];
                nrSpaces++;

                //Preveri, če je prekoračena širina. Če ja idi v novo vrstico
                if ((x + nrSpaces) > normWidth)
                {
                    //Idi v novo vrstico
                    y++;

                    if (y == normHeight)
                    {
                        //prekoračil sem število vrstic...
                        return false;
                    }

                    //Resetiraj x pozicijo
                    x = 0;

                    //resetiraj trenutno besedo, da se uporabi v novi vrstici
                    //TODO: Preveri kaj naredit, če je i==0. In kdaj se lahko to zgodi? Praviloma se ne bi smelo...
                    i--; 
                    
                    //Resetiraj število presledkov
                    nrSpaces = -1;
                }
            }

            return true;
        }
    }
}
