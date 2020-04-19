# PRVI PREIZKUS
## LedDisplay

Prejel si naročilo iz podjetja MojaTrgovina. Naročnik želi, da se kupcu, ki skenira črtno kodo izdelka, na led zaslonu izpiše ime, cena in opis skeniranega izdelka. Vsi artikli, ki jih uporabnik skenira, morajo biti izpisani na zaslonu. V različnih trgovinah po Sloveniji imajo zaslone različnih velikosti, vendar so vsi pravokotni. Širina in višina zaslona sta navedena spodaj, skupaj z besedilom, ki naj bo izpisano na zaslonu. Ker želimo izkoristiti cel led zaslon je potrebno preračunati koliko pikslov je lahko velik posamezen znak besedila prikazanega na zaslonu, ne da bi pri tem razdelili katero koli besedo. Vsi znaki morajo imeti enako širino in višino (npr. 'l' in 'm' zavzemata isti vodoravni prostor, kot tudi presledek). Znaki pisave so enake širine in višine, zato ni dodatnega razmika med sosednjimi znaki ali sosednjimi vrsticami.

## Vhod

Vsaka vrstica vhodne datoteke vsebuje en testni primer v obliki 'S V B'. S je širina in V višina število pikslov razpoložljivega prostora; B je besedilo, ki bi se naj prikazalo na led zaslonu.

## Izhod

Izhod vsebuje po eno vrstico za vsak primer. Če teksta ne moremo prikazati potem je vrednost 0.

## Oddaja rešitve

V prilogi se nahaja datoteka vhodi.txt, ki vsebuje testne primere, katere morate rešiti z vašim programom. Kot rešitev naloge oddate ZIP arhiv, ki vsebuje programsko rešitev (programsko kodo). V opis pri oddaji ideje dodajte izhode, ki jih vrne vaš program za testne primere iz datoteke vhodi.txt. Rešitve brez ustreznega opisa se ne bodo upoštevale.

Prenesi datoteko: Datoteka vhodi.txt

Primer delovanja programa

Vhod:
20 6 led display
100 20 led display 2020
10 20 MUST BE ABLE TO DISPLAY
55 25 Can you hack
100 20 display product text

Izhod:2
9
1
8
8

Za oddajo rešitve kliknite na gumb "Submit new idea".