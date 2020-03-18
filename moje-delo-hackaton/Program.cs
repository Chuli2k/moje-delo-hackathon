using System;
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
            var inputPath = "vhodi.txt";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Datoteka {inputPath} ne obstaja");
                return;
            }

            var lines = File.ReadAllLines(inputPath);
            foreach (var line in lines)
            {
                var vseBesede = line.Split(' ');
                var besede = new string[vseBesede.Length - 2];
                var j = 0;
                for (int i = 2; i < vseBesede.Length; i++)
                {
                    besede[j] = vseBesede[i];
                }
                Console.WriteLine($"Širina:{besede[0]}; Višina:{besede[1]}; {line}");
            }
        }
    }
}
