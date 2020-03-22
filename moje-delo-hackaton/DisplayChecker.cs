using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moje_delo_hackaton
{
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
