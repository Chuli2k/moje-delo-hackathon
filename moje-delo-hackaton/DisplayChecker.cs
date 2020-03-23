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
        public int CheckFontRunCount { get; set; }

        //Naredim spremenljivko, da ne računam vsakič sumo...
        private int sumWordListSizes = 0;

        private void Init(int displayWidth, int displayHeight, IList<int> wordListSizes)
        {
            DisplayWidth = displayWidth;
            DisplayHeight = displayHeight;
            WordListSizes = wordListSizes;
            sumWordListSizes = WordListSizes.Sum();
            CheckFontRunCount = 0;
        }

        /// <summary>
        /// Pogojna optimizacija:
        /// Iščem font kjer je površina ekrana(normalizirana) enaka površini besedila glede na vrstice(normalizirana).
        /// Predpostavka je, da lahko besede 'optimalno' razporedim v vrstice.Torej ni na koncu vrstic nič več praznega prostora.
        /// 
        /// S tem lahko preskočim nekaj iteracij preverjanja velikosti fonta.
        /// To se zgodi v primerih, kjer je rešitev čim bljižje 'optimalnem' izkoristku na zaslonu.Primer je recimo 'You want the truth?...', kjer dobim 30 namesto 70.
        /// Če pa je rešitev takšna, da so prazne vrstice ali zelo kratka beseda v eni vrstici, tako da ni celotna vrstica izpolnjena, potem je ta račun lahko odveč.
        /// 
        /// S = širina
        /// V = višina
        /// F = velikost pisave
        /// Sn = S / F = širina(normalizirana)
        /// Vn = V / F = višina(normalizirana) oz.število vrstic
        /// Ab = < št.besed > +sum(< dolžina besede >) = površina besed
        /// 
        /// A = Sn * Vn = (S * V) / F = Površina ekrana
        /// Ab(F) = Ab - Vn = Ab - (V / F) = Površina besed glede na število vrstic
        /// 
        /// Kdaj je A == Ab(F)?
        /// Ko => Ab* F^2 - V* F - S* V = 0
        ///    => F = (V - sqrt(V) * sqrt(4 * Ab * S + V)) / (2 * Ab)
        ///    => F = (V + sqrt(V) * sqrt(4 * Ab * S + V)) / (2 * Ab)
        /// </summary>
        /// <param name="maxFontSize"></param>
        /// <returns></returns>
        private int GetMaxFontFromFormula(int maxFontSize)
        {
            //F = (V + sqrt(V) * sqrt(4*Ab*S + V)) / (2 * Ab)
            var a = sumWordListSizes + WordListSizes.Count;
            var f = (DisplayHeight + Math.Sqrt(DisplayHeight) * Math.Sqrt(4 * a * DisplayWidth + DisplayHeight)) / (2 * a);
            //Console.WriteLine($"init:{maxFontSize}");
            if (f < maxFontSize) maxFontSize = (int)f;
            //Console.WriteLine($"init enačba:{f}; maxF:{maxFontSize}");

            return maxFontSize;
        }

        private int GetMaxFontForDisplayAndWordMax()
        {
            //Najdi min stranico ekrana in nastavi to kot navečjo možno velikost font-a
            var maxFontSize = DisplayWidth < DisplayHeight ? DisplayWidth : DisplayHeight;

            //Najdi max velikost ekrana glede na najdaljšo besedo, če je manjša kot prej, jo nastavi
            int maxfontSizeByMaxWordLength = DisplayWidth / WordListSizes.Max(); //Celi del deljenja
            if (maxfontSizeByMaxWordLength < maxFontSize) maxFontSize = maxfontSizeByMaxWordLength;

            return maxFontSize;
        }

        /// <summary>
        /// Iz vidika miniziranja klica funkcije CheckFontSize() je ta najboljša. Vsaj za podane primere.
        /// Formula za maxFont vrne kar dober približek maksimuma in lahko font samo zmanjšujem, dokler ne pridem do rešitve.
        /// Iz tega vidika iteriranje max fonta z pivot ne pridobimo nič. Vsaj ne na način, da samo razpolavljam. Iskanje boljšega pivota pa je isti problem kot računanje formule.
        /// Edini problem je tu zahtevnost formule. Noter je koren...
        /// </summary>
        /// <param name="displayWidth"></param>
        /// <param name="displayHeight"></param>
        /// <param name="wordListSizes"></param>
        /// <returns></returns>
        public int GetMaxFontSize(int displayWidth, int displayHeight, IList<int> wordListSizes)
        {
            Init(displayWidth, displayHeight, wordListSizes);

            var maxFontSize = GetMaxFontForDisplayAndWordMax();

            maxFontSize = GetMaxFontFromFormula(maxFontSize);
            
            //Console.Write($"init:{maxFontSize}; ");

            while (maxFontSize > 0)
            {
                if (CheckFontSize(maxFontSize))
                    return maxFontSize;

                maxFontSize--;
            }

            return maxFontSize;
        }

        /// <summary>
        /// Če se hočemo izognit računanju po formuli je ta funkcija boljša.
        /// Je enaka kot ta druga, samo da ni računanje max po formuli in iteriranje je narejeno z pivot-om.
        /// </summary>
        /// <param name="displayWidth"></param>
        /// <param name="displayHeight"></param>
        /// <param name="wordListSizes"></param>
        /// <returns></returns>
        public int GetMaxFontSizeAlt(int displayWidth, int displayHeight, IList<int> wordListSizes)
        {
            Init(displayWidth, displayHeight, wordListSizes);

            var maxFontSize = GetMaxFontForDisplayAndWordMax();

            if (maxFontSize == 0) return 0;

            //Console.Write($"init:{maxFontSize}; ");

            //Če je max možno prikazat, potem je to rezultat.
            if (CheckFontSize(maxFontSize)) return maxFontSize;

            //Če najmanjši možni font ni možno prikazat vrni 0
            var minFontSize = 1;
            if (!CheckFontSize(minFontSize)) return 0;

            //Izhodni pogoj je recimo pri minF=2 in maxF=3, pri tem je vedno min vrednost pravilna
            while (minFontSize + 1 != maxFontSize)
            {
                var pivot = (minFontSize + maxFontSize) / 2;
                var chkPivot = CheckFontSize(pivot);
                if (chkPivot)
                    minFontSize = pivot;
                else
                    maxFontSize = pivot;
            }

            return minFontSize;
        }

        /// <summary>
        /// Preveri, če se podana velikost pisave da prikazat za dimenzije ekrana in velikosti besed
        /// </summary>
        /// <returns></returns>
        private bool CheckFontSize(int maxFontSize)
        {
            CheckFontRunCount++;

            //Spremenjljivki za trenutni pozicijo
            var x = 0; //pixel na horizontali
            var y = 1; //V kateri vrstici sem...
            var nrSpaces = -1; //-1 ker, če je samo ena beseda, pomeni, da ni presledkov...

            //Normalizirana širina in višina glede na velikost pisave.
            int normWidth = DisplayWidth / maxFontSize;
            int normHeight = DisplayHeight / maxFontSize;

            //Preverjanje, da je na voljo dovolj prostora za vse znake
            //Note: Praviloma bi morala 'Pogojna optimizacija' preprečit, da bi se ta pogoj sploh prožil.
            //      Ampak, ker tukaj delam z celimi števili, ki so bolj realni, dobim dejansko površino ekrana manjšo.
            //      Zaradi tega se vseeno splača imet pogoj.
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
