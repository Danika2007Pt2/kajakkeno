namespace kajakkeno
{
    class NapiKeno
    {
        public int ev;
        public int het;
        public int nap;
        public string huzasDatum;
        public List<int> huzottSzamok = new List<int>();

        public NapiKeno(string sor)
        {
            string[] reszek = sor.Split(';');

            ev = int.Parse(reszek[0]);
            het = int.Parse(reszek[1]);
            nap = int.Parse(reszek[2]);
            huzasDatum = reszek[3];

            for(int i = 4; i < reszek.Length; i++)
            {
                huzottSzamok.Add(int.Parse(reszek[i]));
            }
        }
        public int TalalatSzam(List<int> tippek)
        {
            return tippek.Count(szam => huzottSzamok.Contains(szam));
        }
        public bool Helyes()
        {
            if(huzottSzamok.Count != 20)
                return false;
            return huzottSzamok.Distinct().Count() == 20;
        }

    }


    internal class Program
    {
        
        static void Main(string[] args)
        {
            //6
            List<NapiKeno> huzasok = new List<NapiKeno>();

            foreach(var sor in File.ReadAllLines("huzasok.csv"))
            {
                huzasok.Add(new NapiKeno(sor));
            }

            //7
            int hibas = huzasok.Count(h => !h.Helyes());
            Console.WriteLine($"Hibás napi adatok suáma: {hibas}");
            huzasok = huzasok.Where(h=> h.Helyes()).ToList();

            //9
            var utso = huzasok.Last();
            Console.WriteLine($"TIppjei, vesszővel elválasztva: ");
            List<int> tippek = Console.ReadLine().Split(",").Select(int.Parse).ToList();
            if (tippek.Count < 1 || tippek.Count > 10) ;
            {
                Console.WriteLine("Hiba!"); return;
            }

            Console.WriteLine("Tét(1-5): ");
            int tet = int.Parse(Console.ReadLine());
            if(tet < 1 || tet > 5)
            {
                Console.WriteLine("Hiba!"); return;
            }

            int talalat = utso.TalalatSzam(tippek);
            int szorzo = Szorzo(utso, tippek);
            int nyeremeny = szorzo * 200 * tet;

            Console.WriteLine($"Talalatok száma: {talalat}");
            Console.WriteLine($"NYeremény: {nyeremeny} Ft");

            List<int> fixTippek = new List<int> { 17, 28, 32, 44, 54, 63, 72, 75 };
            int osszeg = 0;
            foreach(var nap in huzasok.Where(h => h.ev == 2020))
            {
                int t = nap.TalalatSzam(fixTippek);
                int sz = Szorzo(nap,fixTippek);
                int napi = sz * 200 * 4;

                if (napi > 0) {
                    Console.WriteLine($"{nap.huzasDatum} - Nyeremémy: {napi} Ft");
                }
                Console.WriteLine($"2020-as nyeremény {osszeg} Ft");
            }
        }
        static int Szorzo(NapiKeno keno, List<int> tippek)
        {
            Dictionary<String, int> nyeroParok = new Dictionary<string, int>(){
                {"10-10",1000000}, {"10-9",8000}, {"10-8",350}, {"10-7",30}, {"10-6",3}, {"10-5",1}, {"10-0",2},
                {"9-9",100000}, {"9-8",1200}, {"9-7",100}, {"9-6",12}, {"9-5",3}, {"9-0",1},
                {"8-8",20000}, {"8-7",350}, {"8-6",25}, {"8-5",5}, {"8-0",1},
                {"7-7",5000}, {"7-6",60}, {"7-5",6}, {"7-4",1}, {"7-0",1},
                {"6-6",500}, {"6-5",20}, {"6-4",3}, {"6-0",1},
                {"5-5",200}, {"5-4",10}, {"5-3",2},
                {"4-4",100}, {"4-3",2},
                {"3-3",15}, {"3-2",1},
                {"2-2",6},
                {"1-1",2}
            };
            int jatekTipus = tippek.Count;
            int talalatokSzama = keno.TalalatSzam(tippek);
            string kulcs = jatekTipus + "-" + talalatokSzama;

            if (nyeroParok.Keys.Contains(kulcs))
                return nyeroParok[kulcs];
            else
                return 0;

        }


    }
}
