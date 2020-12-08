using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CA201208D
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            // Zvolenszki Benjámin, 2020. 12. 08. p.gy. dolgozat
            Console.WriteLine("Zvolenszki Benjámin, 2020. 12. 08. p.gy. dolgozat\n");

            var focistak = new List<Focista>();

            // Fájl beolvasása
            using (var sr = new StreamReader("juve.txt", Encoding.UTF8))
            {
                while (!sr.EndOfStream)
                {
                    var adatsor = sr.ReadLine()?.Split(';');
                    if (adatsor != null)
                        focistak.Add(new Focista(
                            mezSzam: Convert.ToByte(adatsor[0]),
                            nev: adatsor[1],
                            nemzetiseg: adatsor[2],
                            poszt: adatsor[3],
                            szulEv: Convert.ToUInt16(adatsor[4])
                        ));
                }
            }

            Console.WriteLine("A fájl beolvasása megtörtént.\n");

            // A) feladatsor
            Console.WriteLine("A) feladatsor");
            
            // 1. feladat: Jelenleg hány igazolt játékosa van a csapatnak?
            Console.WriteLine("1. feladat: Jelenleg hány igazolt játékosa van a csapatnak?");
            Console.WriteLine($"A csapatnak a lista szerint {focistak.Count} db igazolt játékosa van.\n");
            
            // 2. feladat: Van-e magyar nemzetiségű játékosuk?
            Console.WriteLine("2. feladat: Van-e magyar nemzetiségű játékosuk?");
            var magyar = focistak.Any(f => f.Nemzetiseg == "magyar") ? "van" : "nincs";
            Console.WriteLine($"A csapatnak {magyar} magyar játékosa.\n");
            
            // 3. feladat: Hány olasz játékos van a csapatban?
            Console.WriteLine("3. feladat: Hány olasz játékos van a csapatban?");
            var olasz = focistak.Count(f => f.Nemzetiseg == "olasz");
            Console.WriteLine($"A csapatnak {olasz} db olasz játékosa van.\n");
            
            // 4. feladat: Hogy hívják a legfiatalabb játékost?
            Console.WriteLine("4. feladat: Hogy hívják a legfiatalabb játékost?");
            var legfiatalabbSzulEv = focistak.Max(f => f.SzulEv);
            var legfiatalabbNev = focistak.Single(f => f.SzulEv == legfiatalabbSzulEv).Nev;
            Console.WriteLine($"A legfiatalabb focista neve: {legfiatalabbNev}\n");
            
            // 5. feladat: Mennyi az átlagéletkor a csapatban?
            Console.WriteLine("5. feladat: Mennyi az átlagéletkor a csapatban?");
            var atlEletkor = Math.Round(DateTime.Now.Year - focistak.Average(f => f.SzulEv));
            Console.WriteLine($"Az átlagéletkor {atlEletkor} év.\n");
            
            // 6. feladat: Egyes posztokon hány db játékos játszik?
            Console.WriteLine("6. feladat: Egyes posztokon hány db játékos játszik?");
            var posztok = focistak.Select(f => f.Poszt).ToHashSet();
            var jatekosokPosztokon = new Dictionary<string, int>();
            
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var poszt in posztok)
                jatekosokPosztokon.Add(
                    poszt, 
                    focistak.Count(f => f.Poszt == poszt));
            
            foreach (var poszt in jatekosokPosztokon)
                Console.WriteLine($"A {poszt.Key} poszton {poszt.Value} játékos játszik.");
            
            Console.WriteLine();
            
            // 7. feladat: Ki a legidősebb csatár?
            Console.WriteLine("7. feladat: Ki a legidősebb csatár?");
            var csatarok = focistak.Where(f => f.Poszt == "csatár").ToList();
            var legidosebbSzulEv = csatarok.Min(f => f.SzulEv);
            var legidosebbNev = csatarok.Single(f => f.SzulEv == legidosebbSzulEv).Nev;
            Console.WriteLine($"A csapat legidősebb csatára: {legidosebbNev}\n");
            
            // 8. feladat: Mely évek azok, amelyikben pontosan három játékos született?
            Console.WriteLine("8. feladat: Mely évek azok, amelyikben pontosan három játékos született?");
            var evek = focistak.Select(f => f.SzulEv).ToHashSet();
            var evekEsJatekosok = new Dictionary<ushort, int>();
            
            // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
            foreach (var ev in evek)
                evekEsJatekosok.Add(
                    ev,
                    focistak.Count(f => f.SzulEv == ev));

            var harmasEvek = (from ev in evekEsJatekosok where ev.Value == 3 select ev.Key).ToArray();
            Console.WriteLine($"Ezek az évek az alábbiak: {string.Join(", ", harmasEvek)}\n");
            
            // B) feladat
            Console.WriteLine("B) feladat: Kérj be a felhasználótól egy mezszámot, \nés írd ki annak a játékosnak a nevét, aki ezt a mezt viseli!");
            Console.WriteLine("A csapatban lévő mezszámok: " + 
                              string.Join(", ", focistak.Select(f => f.MezSzam).OrderBy(msz => msz)));
            
            Console.Write("\nKérlek válassz egy mezszámot a fentebbiekből: ");
            var valasztottMezSzam = Convert.ToByte(Console.ReadLine());
            var valasztottJatekos = focistak.Single(f => f.MezSzam == valasztottMezSzam).Nev;
            Console.WriteLine($"A választott mezszámhoz tartozó játékos: {valasztottJatekos}\n");
            
            // C) feladat
            Console.WriteLine("C) feladat: Írd ki hatvedek.txt-file-ba az összes hátvéd családnevét" + "\n" + @" (mindenkinek a neve második fele) és életkorát
            úgy," + "\n" + " hogy az életkorok egy oszlopban legyenek!" + "\n");

            using (var sw = new StreamWriter("hatvedek.txt"))
            {
                var hatvedek = focistak.Where(f => f.Poszt == "hátvéd");
                foreach (var jatekos in hatvedek) 
                    sw.WriteLine($"{jatekos.Nev.Split(' ')[1]}\t{DateTime.Now.Year - jatekos.SzulEv}");
            }
            Console.WriteLine("A fájlba írás megtörtént.");

            Console.ReadKey(true);
            // PROGRAM VÉGE
        }
    }
}