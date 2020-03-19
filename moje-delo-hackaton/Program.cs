using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

/*
 * širina višina besedilo
 * 
 * Vhod:
 * 20 6 led display
 * 100 20 led display 2020
 * 10 20 MUST BE ABLE TO DISPLAY
 * 55 25 Can you hack
 * 100 20 display product text
 * 
 * Izhod:
 * 2
 * 9
 * 1
 * 8
 * 8
 * 
 * 20 100 display product text => 2
 * 
 * Hitro kaj lahko testiram:
 * - maxE = Maksimalna velikost pisave glede na ekran:
 * -- Vzame se min vrednost dimenzij ekrana -> min(širina, višina)
 * - maxB = Maksimalna velikost pisave glede na najdaljšo besedo
 * -- Najdem dolžino najdaljše besede, širino ekrana delim z to dolžino
 *
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
                var vseBesede = line.Split(' ');
                var besede = new List<string>(vseBesede);
                besede.RemoveAt(0);
                besede.RemoveAt(0);
                var besedeLength = besede.Select(b => b.Length).ToList();
                Console.WriteLine($"Širina:{vseBesede[0]}; Višina:{vseBesede[1]}; {besede.Aggregate<string>((prev, curr) => prev + " " + curr)} ({besedeLength.Aggregate("", (prev, curr) => prev + " " + curr.ToString())})");

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

                //Če ja => return(maxFontSize)
                //Drugače:
                maxFontSize--;
            }

            return maxFontSize;
        }

        private static bool CheckFontSize(int width, int height, IList<int> wordLengths, int maxFontSize)
        {
            //**** Preveri, če se trenutna velikost da prikazat. *****
            //Spremenjljivki za trenutni pozicijo
            var x = 0; //pixel na horizontali
            var y = 0; //V kateri vrstici sem...
            var nrSpaces = -1; //-1 ker, če je samo ena beseda, pomeni, da ni presledkov...
            int maxLines = height / maxFontSize;

            for (int i = 0; i < wordLengths.Count; i++)
            {
                x += wordLengths[i] * maxFontSize;
                nrSpaces++;

                //Preveri, če je prekoračena širina. Če ja idi v novo vrstico
                if ((x + nrSpaces * maxFontSize) > width)
                {
                    //Idi v novo vrstico
                    y++;

                    if (y == maxLines)
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
