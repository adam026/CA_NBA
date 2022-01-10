using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TANBA
{
    class JatekosForOrder
    {
        public string CsapatNev { get; set; }
        public Jatekos Jatekos { get; set; }
    }
    class Jatekos
    {
        public string Nev { get; set; }
        public int EvesFizetes { get; set; }
        public int Szerzodes { get; set; }
        public Jatekos(string nev, string evesFizetes, string szerzodes)
        {
            Nev = nev;
            EvesFizetes = Convert.ToInt32(evesFizetes);
            Szerzodes = Convert.ToInt32(szerzodes);
        }

    }
    class Program
    {
        static Dictionary<string, List<Jatekos>> csapatok = new Dictionary<string, List<Jatekos>>();
        static int jatekosokSzama = 0;
        static void Main(string[] args)
        {
            Beolvasas();
            //OsszesCsapatNeve();
            //JovedelemEvekreSzamitva();
            //LegjobbKereset();
            //CsapatonkentElkoltottPenz();
            //LegnagyobbKulonbseg();
            //LegalacsonyabbAtlagfizetes();
            //LegdragabbJatekosNeve();
            //AtlagfizetesAzNBAban();
            //CsapatonkentAtlagFelett();
            EvekSzerintCsokkenobe();
            Console.ReadLine();
        }

        private static void EvekSzerintCsokkenobe()
        {
            var rendezett = new List<JatekosForOrder>();
            foreach (var csapat in csapatok)
            {
                foreach (var jatekos in csapat.Value)
                {
                    rendezett.Add(new JatekosForOrder { CsapatNev = csapat.Key, Jatekos = jatekos });
                }
            }

            var rendezett2 = rendezett.OrderByDescending(x => x.Jatekos.EvesFizetes).ToList();

            foreach (var jatekos in rendezett2)
            {
                Console.WriteLine($"{jatekos.CsapatNev} - {jatekos.Jatekos.Nev} - {jatekos.Jatekos.EvesFizetes}");
            }
        }

        private static void CsapatonkentAtlagFelett()
        {
            foreach (var csapat in csapatok)
            {
                var csapatAtlag = csapat.Value.Average(x => x.EvesFizetes);
                var atlagFelettDb = csapat.Value.Count(x => x.EvesFizetes > csapatAtlag);

                Console.WriteLine($"{csapat.Key} : {atlagFelettDb} db");
            }
        }

        private static void AtlagfizetesAzNBAban()
        {
            var atlag = csapatok.Values.Average(x => x.Average(y => y.EvesFizetes));
            long sum = 0;

            foreach (var csapat in csapatok.Values)
            {
                foreach (var jatekos in csapat)
                {
                    sum += jatekos.EvesFizetes;
                }
            }

            Console.WriteLine($"Átlag: {atlag}");
            Console.WriteLine($"Átlag másképp: {sum/jatekosokSzama}");
        }

        private static void LegdragabbJatekosNeve()
        {
            foreach (var csapat in csapatok)
            {
                Console.WriteLine($"{csapat.Key}: {csapat.Value.Where(x => x.EvesFizetes == csapat.Value.Max(y => y.EvesFizetes)).First().Nev}");
            }
        }

        private static void LegalacsonyabbAtlagfizetes()
        {
            (string Nev, double avg) minAvg = ("", int.MaxValue);

            foreach (var csapat in csapatok)
            {
                double avg = csapat.Value.Average(x => x.EvesFizetes);
                if (avg < minAvg.avg)
                {
                    minAvg = (csapat.Key, avg);
                }
            }

            Console.WriteLine($"A legalacsonyabb átlagfizetésű csapat: {minAvg.Nev} - A legalacsonyabb átlagfizetés: {minAvg.avg}");
        }

        private static void LegnagyobbKulonbseg()
        {
            (string Nev, int dif) legnagyobbkolonbseg = ("", 0);

            foreach (var csapat in csapatok)
            {
                int dif = csapat.Value.Max(x => x.EvesFizetes) - csapat.Value.Min(x => x.EvesFizetes);
                if (dif > legnagyobbkolonbseg.dif)
                {
                    legnagyobbkolonbseg = (csapat.Key, dif);
                }
            }

            Console.WriteLine($"A legnagyobb különbség a {legnagyobbkolonbseg.Nev} csapatban van: {legnagyobbkolonbseg.dif} millió dollár");
        }

        private static void CsapatonkentElkoltottPenz()
        {
            foreach (var csapat in csapatok)
            {
                Console.WriteLine($"{csapat.Key} : ${csapat.Value.Sum(x => x.EvesFizetes)}");
            }
        }

        private static void LegjobbKereset()
        {
            var max = new Jatekos("", "0", "0");

            foreach (var csapat in csapatok.Values)
            {
                foreach (var jatekos in csapat)
                {
                    if (jatekos.EvesFizetes > max.EvesFizetes)
                    {
                        max = jatekos;
                    }
                }
            }

            Console.WriteLine("Legnagyobb jövedelem a szezonban: " + $"{max.Nev} - {max.EvesFizetes} millió dollár");
        }

        private static void JovedelemEvekreSzamitva()
        {
            foreach (var csapat in csapatok)
            {
                Console.WriteLine($"{csapat.Key}");
                foreach (var jatekos in csapat.Value)
                {

                    Console.WriteLine($"\t{jatekos.Nev} - {jatekos.EvesFizetes * jatekos.Szerzodes}");
                }
            }
        }

        private static void OsszesCsapatNeve()
        {
            Console.WriteLine("A csapatok:");
            foreach (var csapatNev in csapatok.Keys)
            {
                Console.WriteLine($"\t{csapatNev}");
            }
        }

        private static void Beolvasas()
        {
            using (var sr = new StreamReader(@"..\..\..\Resources\NBA2003.csv"))
            {
                jatekosokSzama = int.Parse(sr.ReadLine());

                while (!sr.EndOfStream)
                {
                    var sor = sr.ReadLine().Split(';');

                    if (csapatok.ContainsKey(sor[0].Trim('\"')))
                    {
                        csapatok[sor[0].Trim('\"')].Add(new Jatekos(sor[1].Trim('\"'), sor[2], sor[3]));
                    }
                    else
                    {
                        csapatok.Add(
                            key: sor[0].Trim('\"'),
                            value: new List<Jatekos>()
                            {
                                new Jatekos(sor[1].Trim('\"'), sor[2], sor[3])
                            });
                    }
                }


            }
        }
    }
}
