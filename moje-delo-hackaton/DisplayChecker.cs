using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moje_delo_hackaton
{
    /*
     * Pogojna optimizacija:
     * Iščem font kjer je površina ekrana (normalizirana) enaka površini besedila glede na vrstice (normalizirana).
     * Predpostavka je, da lahko besede 'optimalno' razporedim v vrstice. Torej ni na koncu vrstic nič več praznega prostora.
     * S tem lahko preskočim nekaj iteracij preverjanja velikosti fonta. 
     * To se zgodi v primerih, kjer je rešitev čim bljižje 'optimalnem' izkoristku na zaslonu. Primer je recimo 'You want the truth?...', kjer dobim 30 namesto 70.
     * Če pa je rešitev takšna, da so prazne vrstice ali zelo kratka beseda v eni vrstici, tako da ni celotna vrstica izpolnjena, potem je ta račun lahko odveč.
     * 
     * S = širina
     * V = višina
     * F = velikost pisave
     * Sn = S / F = širina(normalizirana)
     * Vn = V / F = višina(normalizirana) oz. število vrstic
     * Ab = <št. besed> + sum(<dolžina besede>) = površina besed
     * 
     * A = Sn * Vn = (S * V) / F = Površina ekrana
     * Ab(F) = Ab - Vn = Ab - (V/F) = Površina besed glede na število vrstic
     * 
     * Kdaj je A == Ab(F) ?
     * Ko => Ab*F^2 - V*F - S*V = 0
     *    => F = (V - sqrt(V) * sqrt(4*Ab*S + V)) / (2 * Ab)
     *    => F = (V + sqrt(V) * sqrt(4*Ab*S + V)) / (2 * Ab)
     */
    public class DisplayChecker
    {
        public int DisplayWidth { get; private set; }
        public int DisplayHeight { get; private set; }
        public IList<int> WordListSizes { get; private set; }

        //Naredim spremenljivko, da ne računam vsakič sumo...
        private int sumWordListSizes = 0;

        public int GetMaxFontSize(int displayWidth, int displayHeight, IList<int> wordListSizes)
        {
            //Init
            DisplayWidth = displayWidth;
            DisplayHeight = displayHeight;
            WordListSizes = wordListSizes;
            sumWordListSizes = WordListSizes.Sum();

            //Najdi min stranico ekrana in nastavi to kot navečjo možno velikost font-a
            var maxFontSize = DisplayWidth < DisplayHeight ? DisplayWidth : DisplayHeight;

            //Najdi max velikost ekrana glede na najdaljšo besedo, če je manjša kot prej, jo nastavi
            int maxfontSizeByMaxWordLength = DisplayWidth / WordListSizes.Max(); //Celi del deljenja
            if (maxfontSizeByMaxWordLength < maxFontSize) maxFontSize = maxfontSizeByMaxWordLength;

            //Pogojna optimizacija (razlaga odzgoraj):
            //F = (V + sqrt(V) * sqrt(4*Ab*S + V)) / (2 * Ab)
            var a = sumWordListSizes + WordListSizes.Count;
            var f = (DisplayHeight + Math.Sqrt(DisplayHeight) * Math.Sqrt(4 * a * DisplayWidth + DisplayHeight)) / (2 * a);
            //Console.WriteLine($"init:{maxFontSize}");
            if (f < maxFontSize) maxFontSize = (int)f;
            //Console.WriteLine($"init enačba:{f}; maxF:{maxFontSize}");

            while (maxFontSize > 0)
            {
                if (CheckFontSize(maxFontSize))
                    return maxFontSize;

                maxFontSize--;
            }

            return maxFontSize;
        }

        /// <summary>
        /// Preveri, če se podana velikost pisave da prikazat za dimenzije ekrana in velikosti besed
        /// </summary>
        /// <returns></returns>
        private bool CheckFontSize(int maxFontSize)
        {
            //Spremenjljivki za trenutni pozicijo
            var x = 0; //pixel na horizontali
            var y = 1; //V kateri vrstici sem...
            var nrSpaces = -1; //-1 ker, če je samo ena beseda, pomeni, da ni presledkov...

            //Normalizirana širina in višina glede na velikost pisave.
            int normWidth = DisplayWidth / maxFontSize;
            int normHeight = DisplayHeight / maxFontSize;

            //Preverjanje, da je na voljo dovolj prostora za vse znake
            int area = normWidth * normHeight;
            int numChars = sumWordListSizes + WordListSizes.Count - normHeight;
            if (area < numChars) return false;

            for (int i = 0; i < WordListSizes.Count; i++)
            {
                x += WordListSizes[i];
                nrSpaces++;

                //Preveri, če je prekoračena širina. Če ja idi v novo vrstico
                if ((x + nrSpaces) > normWidth)
                {
                    //Idi v novo vrstico
                    y++;

                    if (y > normHeight)
                    {
                        //prekoračil sem število vrstic...
                        return false;
                    }

                    //Resetiraj x pozicijo
                    x = 0;

                    //resetiraj trenutno besedo, da se uporabi v novi vrstici
                    i--;

                    //Resetiraj število presledkov
                    nrSpaces = -1;
                }
            }

            return true;
        }
    }
}
