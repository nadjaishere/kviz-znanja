using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projekat2godina1
{
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
        struct RangLista
        {
            public string ime;
            public int brpoena;
        }
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
        private static void UnosPitanjaIzDatoteke1(string ime_kategorije, int broj_kategorije, Pitanja[] kat)
        {
            Array.Resize(ref kat[broj_kategorije].indeks_tacnog_odg, kat[broj_kategorije].brojpitanja);
            UnosPitanjaIzDatoteke(ime_kategorije, broj_kategorije, kat);
        }
        //
        static void UnosPitanjaIzDatoteke(string ime_kategorije, int broj_kategorije, Pitanja[] kat)
        {
            BrojPitanjaPoKattegorijama("pitanja", kat);
            int brojac_pitanja = 0;
            StreamReader ulaz = new StreamReader(ime_kategorije + ".txt");
            kat[broj_kategorije].pitanje = new string[kat[broj_kategorije].brojpitanja];
            kat[broj_kategorije].odgovor1 = new string[kat[broj_kategorije].brojpitanja];
            kat[broj_kategorije].odgovor2 = new string[kat[broj_kategorije].brojpitanja];
            kat[broj_kategorije].odgovor3 = new string[kat[broj_kategorije].brojpitanja];
            kat[broj_kategorije].odgovor4 = new string[kat[broj_kategorije].brojpitanja];
            kat[broj_kategorije].indeks_tacnog_odg = new int[kat[broj_kategorije].brojpitanja];
            while (!ulaz.EndOfStream)
            {
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
            Console.WriteLine(ime_kategorije + "|" + brojac_pitanja);
            ulaz.Close();
        }
        //
        //
        static bool PrekidKvizaKorisnik(ConsoleKeyInfo x, string[] kategorije, Pitanja[] kat)
        {
            if (x.Key == ConsoleKey.Escape)
            {
                return true;
            }
            else if (x.Key == ConsoleKey.Home)
            {
                UpisPitanja(kategorije, kat);
                for (int i = 0; i < 20; i++)
                    UnosPitanjaIzDatoteke(kategorije[i], i, kat);
            }
            return false;
        }
        static void UpisPitanja(string[] kategorije, Pitanja[] kat)
        {
            Console.WriteLine("Koliko pitanja zelite da unesete?");
            int brojpitanja;
            while (!int.TryParse(Console.ReadLine(), out brojpitanja) || brojpitanja < 0)
                Console.WriteLine("Unesite ponovo broj pitanja");
            for (int i = 0; i < brojpitanja; i++)
            {
                Console.WriteLine("Unesite ime kategorije kojoj zelite da dodelite pitanje koje unostite:");
                string ime_datoteke_kategorije = Console.ReadLine().ToLower();
                Console.WriteLine("Unesite pitanje");
                string pitanje = Console.ReadLine();
                if (pitanje.IndexOf("?") != pitanje.Length - 1)
                    pitanje += "?";
                Console.WriteLine("Unesite 4 ponudjena odgovora razdvojeni jednim razmakom:");
                string[] ponudjeni_odgovori = Console.ReadLine().Split();
                while (ponudjeni_odgovori.Length != 4)
                {
                    Console.WriteLine("Unesite ponovo ponudjene odgovore ponovo!");
                    ponudjeni_odgovori = Console.ReadLine().Split();
                }
                Console.WriteLine("Unesite indeks tacnog odgovora (broj od 1 do 4):");
                int indeks_kategorije = IndeksKategorije(ime_datoteke_kategorije, kategorije);
                int indeks;
                while (!int.TryParse(Console.ReadLine(), out indeks) || indeks < 1 || indeks > 4)
                    Console.WriteLine("Unesite ponovo indeks tacnog odgovora ponovo!");
                StreamWriter upis = new StreamWriter(ime_datoteke_kategorije+".txt", true);
                upis.WriteLine(pitanje + "|" + ponudjeni_odgovori[0] + "|" + ponudjeni_odgovori[1] + "|" + ponudjeni_odgovori[2] + "|" + ponudjeni_odgovori[3] + "|" + indeks);
                upis.Close();
                kat[indeks_kategorije-1].brojpitanja++;
                Console.WriteLine(kat[indeks_kategorije-1].brojpitanja+"*");
                PromenaBrojaPitanja("pitanja", indeks_kategorije, kat);
            }
        }
        //
        static void BrojPitanjaPoKattegorijama(string ime_datoteke, Pitanja[] kat)
        {
            StreamReader broj_pitanja = new StreamReader(ime_datoteke + ".txt");
            int br = 0;
            while (!broj_pitanja.EndOfStream)
            {
                string s = broj_pitanja.ReadLine();
                string[] red = s.Split('|');
                for (int i = 0; i < red.Length; i++)
                {
                    kat[i].brojpitanja = Convert.ToInt32(red[i]);
                }
                br++;
            }
            broj_pitanja.Close();
        }
        static void PromenaBrojaPitanja(string ime_datoteke, int indeks_kategorije, Pitanja[] kat)
        {
            StreamReader br = new StreamReader(ime_datoteke + ".txt");
            string[] red;
            while (!br.EndOfStream)
            {
                string s = br.ReadLine();
                red = s.Split('|');
                red[indeks_kategorije - 1] = Convert.ToString(kat[indeks_kategorije - 1].brojpitanja);

            }
            br.Close();
            StreamWriter broj_pitanja = new StreamWriter(ime_datoteke + ".txt");
            for (int i = 0; i < 20; i++)
            {
                if (i != 19)
                    broj_pitanja.Write(kat[i].brojpitanja + "|");

                else
                    broj_pitanja.Write(kat[i].brojpitanja);
            }
            broj_pitanja.Close();

        }
        //
        static int IndeksKategorije(string kategorija, string[] kategorije)
        {
            int indeks_kategorije = 0;
            for (int i = 0; i < 20; i++)
            {
                if (kategorija == kategorije[i])
                    indeks_kategorije = i + 1;
            }
            return indeks_kategorije;
        }
        //
        static bool ProveraPonavljanjaPitanja(int broj_pitanja, int[] brojevi_pitanja,int indeks_broja_pitanja_u_nizu)
        {
            for (int i = 0; i < brojevi_pitanja.Length; i++)
            {
                if (broj_pitanja == brojevi_pitanja[i]&&indeks_broja_pitanja_u_nizu!=i)
                {
                    return true;
                }
                    
            }
            return false;
        }
        static void IspisNiza(int[] niz)
        {
            for (int i = 0; i < niz.Length; i++)
            {
                if(i==niz.Length-1)
                    Console.WriteLine(niz[1]);
                else
                    Console.Write(niz[1]);
            }
        }
        //izmenjena metoda
        static bool IspisPitanja(Pitanja[] pitanja, int brPitanja, int indeks_kategorije, string[] kategorije, out bool izlaz,out int broj_poena)
        {
            var s = Console.ReadKey();
            izlaz = PrekidKvizaKorisnik(s, kategorije, pitanja);
            DateTime ispis=DateTime.Now;
            Console.WriteLine(pitanja[indeks_kategorije].pitanje[brPitanja]);
            Console.WriteLine("[ ] {0}\n[ ] {1}\n[ ] {2}\n[ ] {3}", pitanja[indeks_kategorije].odgovor1[brPitanja],
            pitanja[indeks_kategorije].odgovor2[brPitanja], pitanja[indeks_kategorije].odgovor3[brPitanja],
            pitanja[indeks_kategorije].odgovor4[brPitanja]);
            int poz = Console.CursorTop;
            Console.SetCursorPosition(1, Console.CursorTop - 4);
            PomerajKursora(0, 3, 0, 1);
            Console.Write("*");
            Console.SetCursorPosition(0, poz + 1);
            TimeSpan odgovor = ispis-DateTime.Now;
            TimeSpan vreme1 = new TimeSpan(0, 0, 0, 10);
            TimeSpan vreme2 = new TimeSpan(0, 0, 0, 13);
            TimeSpan vreme3 = new TimeSpan(0, 0, 0, 16);
            TimeSpan vreme4 = new TimeSpan(0, 0, 0, 19);
            TimeSpan vreme5 = new TimeSpan(0, 0, 0, 21);
            if (kursorY == pitanja[indeks_kategorije].indeks_tacnog_odg[brPitanja] - 1)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("Tacno!");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                if (odgovor < vreme1)
                    broj_poena = 10;
                else if (odgovor < vreme2)
                    broj_poena = 9;
                else if (odgovor < vreme3)
                    broj_poena = 8;
                else if (odgovor < vreme4)
                    broj_poena = 7;
                else if (odgovor < vreme5)
                    broj_poena = 6;
                else
                    broj_poena = 5;
                return true;
                
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.Black;
                string tacanOdg;
                switch (pitanja[indeks_kategorije].indeks_tacnog_odg[brPitanja])
                {
                    case 1: tacanOdg = pitanja[indeks_kategorije].odgovor1[brPitanja]; break;
                    case 2: tacanOdg = pitanja[indeks_kategorije].odgovor2[brPitanja]; break;
                    case 3: tacanOdg = pitanja[indeks_kategorije].odgovor3[brPitanja]; break;
                    default: tacanOdg = pitanja[indeks_kategorije].odgovor4[brPitanja]; break;
                }
                Console.WriteLine("Netacno! Tacan odgovor je bio " + pitanja[indeks_kategorije].indeks_tacnog_odg[brPitanja] + ". " + tacanOdg);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                broj_poena = 0;
                return false;
            }
        }
        //
        static int IspisIIzborKategorije(string[] kategorije)
        {
            for (int i = 0; i < kategorije.Length; i++)
                Console.WriteLine("[ ]" + kategorije[i]);
            Console.SetCursorPosition(1, Console.CursorTop - 20);
            PomerajKursora(0, 19, 0, 1);
            return kursorY;
        }
        //
        static void rangLista(string imetakmicara, int brojpoena, int brljudi)
        {
            RangLista[] niz = new RangLista[brljudi];
            StreamReader ranglista = new StreamReader("ranglista.txt");
            int br = 0;
            while (!ranglista.EndOfStream)
            {
                string[] pomocniniz = ranglista.ReadLine().Split('|');
                niz[br].ime = pomocniniz[0];
                niz[br].brpoena = Convert.ToInt32(pomocniniz[1]);
                br++;
            }
            niz[brljudi - 1].ime = imetakmicara;
            niz[brljudi - 1].brpoena = brojpoena;
            for (int i = 0; i < brljudi - 1; i++)
            {
                if (niz[i].brpoena < niz[brljudi - 1].brpoena)
                {
                    string pomocime = niz[i].ime;
                    int pomocbrpoena = niz[i].brpoena;
                    niz[i].brpoena = niz[brljudi - 1].brpoena;
                    niz[i].ime = niz[brljudi - 1].ime;
                    niz[brljudi - 1].brpoena = pomocbrpoena;
                    niz[brljudi - 1].ime = pomocime;
                }
            }
            StreamWriter izlaz = new StreamWriter("ranglista.txt");
            for (int j = 0; j < brljudi; j++)
                izlaz.WriteLine(niz[j].ime + "|" + niz[j].brpoena);
            izlaz.Close();
        }
        
        static void PoeniKorisnika(string ime_datoteke,string ime,string prezime,int broj_bodova)
        {
            StreamWriter datoteka = new StreamWriter(ime_datoteke + ".txt",true);
            datoteka.WriteLine(ime_datoteke+"|"+ime+"|"+prezime+"|"+broj_bodova);
            datoteka.Close();

        }
        public static void Main(string[] args)
        {
            bool korisnik_izlaz = true;
            while (korisnik_izlaz)
            {
                string[] kategorije = {
            "geografija",
            "filmovi",
            "muzika",
            "sport",
            "istorija",
            "poreklo reci",
            "moda",
            "domaca muzika",
            "zivotinje",
            "igrice",
            "glavni gradovi",
            "opste znaje",
            "informatika",
            "hrana",
            "pisci i njihova dela",
            "umetnost",
            "mitologija",
            "nauka",
            "poznate licnosti",
            "razno"
            };
                Console.WriteLine("KVIZ ZNANJA");
                Console.WriteLine("Unesite ime i prezime razdvojene jednim razmakom:");
                string s = Console.ReadLine();
                string[] ime_prezime = s.Split(' ');
                string ime = ime_prezime[0];
                string prezime = ime_prezime[1];
                Console.WriteLine("Izaberite kategoriju:");
                int broj_poena = 0;
                int ukupno_poena = 0;
                int katego = IspisIIzborKategorije(kategorije);
                string izborkategorije = kategorije[katego];
                int broj_kategorije = IndeksKategorije(izborkategorije, kategorije);
                Pitanja[] kat = new Pitanja[20];
                UnosPitanjaIzDatoteke(izborkategorije, broj_kategorije-1, kat);
                //izbor random pitanja iz kategorije
                int[] random_pitanja_indeksi = new int[10];
                int random_broj;
                Random rnd = new Random();
                for (int i = 0; i < 10; i++)
                {
                    random_pitanja_indeksi[i] = rnd.Next(0, kat[broj_kategorije].brojpitanja-1);
                    Console.WriteLine(random_pitanja_indeksi[i]+"*");
                    while (ProveraPonavljanjaPitanja(random_pitanja_indeksi[i], random_pitanja_indeksi,i) == true)
                    {
                        random_broj = rnd.Next(0, kat[broj_kategorije].brojpitanja - 1);
                        random_pitanja_indeksi[i] = random_broj;
                    }
                }
                Console.Clear();
                bool izlaz = false;
                for (int i = 0; i < 10; i++)
                {
                    IspisPitanja(kat, random_pitanja_indeksi[i], broj_kategorije-1, kategorije, out izlaz,out broj_poena);
                    ukupno_poena += broj_poena;
                    if (izlaz == true)
                    {
                        korisnik_izlaz = false;
                        return;
                    }

                }
                PoeniKorisnika("rezultati", ime, prezime, ukupno_poena);
            }

        }
    }
}
