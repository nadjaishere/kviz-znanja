using System;
using System.IO;

class Program
{
 struct Pitanja
        {
            public string[] pitanje;
            public string[] odgovor1;
            public string[] odgovor2;
            public string[] odgovor3;
            public string[] odgovor4;
            public int[] indeks_tacnog_odg;
            public int brojpitanja;
        }
        static int kursorX = 0, kursorY = 0;
        //koliko polja mogu da idem gore dole
        //x=0 i y=4
        //

        static void PomerajKursora(int x, int y, int pomerajX, int pomerajY)
        {
            kursorX = 0;
            kursorY = 0;
            int pocetniY = Console.CursorTop;
            int pocetniX = Console.CursorLeft;
            ConsoleKeyInfo strelica;
            do
            {
                strelica = Console.ReadKey(true);
                if (strelica.Key == ConsoleKey.DownArrow)
                {
                    if (kursorY >= 0 && kursorY < y * pomerajY) kursorY += pomerajY;
                }
                else if (strelica.Key == ConsoleKey.UpArrow)
                {
                    if (kursorY > 0 && kursorY <= y * pomerajY) kursorY -= pomerajY;
                }
                else if (strelica.Key == ConsoleKey.LeftArrow)
                {
                    if (kursorX > 0 && kursorX <= x * pomerajX) kursorX -= pomerajX;
                }
                else if (strelica.Key == ConsoleKey.RightArrow)
                {
                    if (kursorX >= 0 && kursorX < x * pomerajX) kursorX += pomerajX;
                }
                else if (strelica.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else continue;
                Console.SetCursorPosition(pocetniX + kursorX, pocetniY + kursorY);
            }
            while (true);
        }
        //
        //
        static void UpisPitanja(string[] kategorije)
        {
            Console.WriteLine("Koliko pitanja zelite da unesete?");
            int brojpitanja;
            while (!int.TryParse(Console.ReadLine(), out brojpitanja) || brojpitanja < 0)
            {
                Console.WriteLine("Unesite ponovo broj pitanja");
            }
            for (int i = 0; i < brojpitanja; i++)
            {
                Console.WriteLine("Unesite ime kategorije kojoj zelite da dodelite pitanje koje unostite:");
                string ime_datoteke_kategorije = Console.ReadLine().ToLower();
                Console.WriteLine("Unesite pitanje");
                string pitanje = Console.ReadLine();
                Console.WriteLine("Unesite 4 ponudjena odgovora razdvojeni jednim razmakom");
                string[] ponudjeni_odgovori = Console.ReadLine().Split();
                while (ponudjeni_odgovori.Length != 4)
                {
                    Console.WriteLine("Unesite ponovo ponudjene odgovore ponovo");
                    ponudjeni_odgovori = Console.ReadLine().Split();
                }
                Console.WriteLine("Unesite indeks tacnog odgovora (broj od 1 do 4):");
                int indeks;
                while (!int.TryParse(Console.ReadLine(), out indeks) || indeks < 1 || indeks > 4)
                {
                    Console.WriteLine("Unesite ponovo indeks tacnog odgovora ponovo");
                }
                StreamWriter upis = new StreamWriter(ime_datoteke_kategorije, true);
                upis.WriteLine(pitanje + "|" + ponudjeni_odgovori[0] + "|" + ponudjeni_odgovori[1] + "|" + ponudjeni_odgovori[2] + "|" + ponudjeni_odgovori[3] + "|" + indeks);
                upis.Close();
            }

        }
        //
        //
        static void UnosPitanjaIzDatoteke(string ime_kategorije, int broj_kategorije,Pitanja[] kat)
        {
            int brojac_pitanja = 0;
            StreamReader ulaz = new StreamReader(ime_kategorije+".txt");
                while (!ulaz.EndOfStream)
                {
                kat[broj_kategorije].pitanje = new string[1000];
                kat[broj_kategorije].odgovor1 = new string[1000];
                kat[broj_kategorije].odgovor2 = new string[1000];
                kat[broj_kategorije].odgovor3 = new string[1000];
                kat[broj_kategorije].odgovor4 = new string[1000];
                kat[broj_kategorije].indeks_tacnog_odg = new int[1000];
                string s = ulaz.ReadLine();
                string[] red = s.Split('|');
                kat[broj_kategorije].pitanje[brojac_pitanja] = red[0];
                kat[broj_kategorije].odgovor1[brojac_pitanja] = red[1];
                kat[broj_kategorije].odgovor2[brojac_pitanja] = red[2];
                kat[broj_kategorije].odgovor3[brojac_pitanja] = red[3];
                kat[broj_kategorije].odgovor4[brojac_pitanja] = red[4];
                kat[broj_kategorije].indeks_tacnog_odg[brojac_pitanja] = Convert.ToInt32(red[5]);
                brojac_pitanja++;
                }
                kat[broj_kategorije].brojpitanja = brojac_pitanja;
        }
        //
        static bool PrekidKvizaKorisnik(ConsoleKeyInfo x, string[] kategorije,Pitanja[] kat)
        {
            if (x.Key == ConsoleKey.Escape)
            {
                return true;
            }
            else if (x.Key == ConsoleKey.Home)
            {
                UpisPitanja(kategorije);
                for (int i = 0; i < 20; i++)
                {
                    UnosPitanjaIzDatoteke(kategorije[i], i,kat);
                }
            }
            return false;
        }
        //
        static int IndeksKategorije(string kategorija,string[] kategorije)
        {
            int indeks_kategorije = 0;
            for (int i = 0; i < 20; i++)
            {
                if (kategorija==kategorije[i])
                {
                    indeks_kategorije = i + 1;
                }
            }
            return indeks_kategorije;
        }
        static bool ProveraPonavljanjaPitanja(int broj_pitanja,int[] brojevi_pitanja)
        {
            for (int i = 0; i < brojevi_pitanja.Length; i++)
            {
                if (broj_pitanja==brojevi_pitanja[i])
                {
                    return true;
                }
            }
            return false;
        }
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World");
            //ConsoleKeyInfo-tip podataka
            //ConsoleKeyInfo=Console.ReadKey
            //ConsoleKey.Escape
            //ConsoleKey.D1
            string[] kategorije = {
            "Sport",
            "Filmovi i serije",
            "Moda",
            "Geografija",
            "Istorija",
            "Poreklo reci",
            "Strana muzika",
            "Domaca muzika",
            "Zivotinje",
            "Igrice",
            "Glavni gradovi",
            "Opste znaje",
            "Informatika",
            "Hrana",
            "Pisci i njihova dela",
            "Umetnost",
            "Mitologija",
            "Nauka",
            "Poznate licnosti",
            "Razno"
            };
            Console.WriteLine("KVIZ ZNANJA");
            Console.WriteLine("Izaberite kategoriju:");
            string katego = Console.ReadLine();
            int broj_kategorije = IndeksKategorije(katego, kategorije);
            Pitanja[] kat = new Pitanja[1000];
            UnosPitanjaIzDatoteke(katego, broj_kategorije,kat);
            Console.WriteLine(kat[broj_kategorije].brojpitanja);
            //izbor random pitanja iz kategorije
            int[] random_pitanja_indeksi = new int[10];
            Random rnd = new Random();
            for (int i = 0; i < 10; i++)
            {
                random_pitanja_indeksi[i] = rnd.Next(1, kat[broj_kategorije].brojpitanja);
                while (ProveraPonavljanjaPitanja(random_pitanja_indeksi[i],random_pitanja_indeksi))
                {
                    random_pitanja_indeksi[i] = rnd.Next(1, kat[broj_kategorije].brojpitanja);
                }
            }


        }
}