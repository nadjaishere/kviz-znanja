using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    internal class Program
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
        struct RangLista
        {
            public string ime;
            public int poeni;
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
        static void UnosPitanjaIzDatoteke(string ime_kategorije, int broj_kategorije, Pitanja[] kat)
        {
            BrojPitanjaPoKattegorijama("pitanja", kat);
            int brojac_pitanja = 0;
            kat[broj_kategorije].pitanje = new string[kat[broj_kategorije].brojpitanja];
            kat[broj_kategorije].odgovor1 = new string[kat[broj_kategorije].brojpitanja];
            kat[broj_kategorije].odgovor2 = new string[kat[broj_kategorije].brojpitanja];
            kat[broj_kategorije].odgovor3 = new string[kat[broj_kategorije].brojpitanja];
            kat[broj_kategorije].odgovor4 = new string[kat[broj_kategorije].brojpitanja];
            kat[broj_kategorije].indeks_tacnog_odg = new int[kat[broj_kategorije].brojpitanja];
            StreamReader ulaz = new StreamReader(ime_kategorije + ".txt");
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
            ulaz.Close();
        }
        //
        //
        static bool PrekidKvizaKorisnik(ConsoleKeyInfo x, string[] kategorije, Pitanja[] kat)
        {
            if (x.Key == ConsoleKey.End)
            {
                return true;
            }
            else if (x.Key == ConsoleKey.Home)
            {
                UpisPitanja(kategorije, kat);
                for (int i = 0; i < 7; i++)
                    UnosPitanjaIzDatoteke(kategorije[i], i, kat);
            }
            return false;
        }
        static void UpisPitanja(string[] kategorije, Pitanja[] kat)
    {
        Console.WriteLine("Koliko pitanja zelite da unesete?");
        int brojpitanja;
        int broj = 0;
        while (!int.TryParse(Console.ReadLine(), out brojpitanja) || brojpitanja < 0)
            Console.WriteLine("Unesite ponovo broj pitanja");
        for (int i = 0; i < brojpitanja; i++)
        {
            broj = i + 1;
            Console.WriteLine("Unesite ime kategorije kojoj zelite da dodelite pitanje koje unostite:");
            int katego = IspisIIzborKategorije(kategorije);
            string izborkategorije = kategorije[katego];
            int broj_kategorije = IndeksKategorije(izborkategorije, kategorije);
            Console.Clear();
            Console.WriteLine("Unesite pitanje "+broj+".");
            string pitanje = Console.ReadLine();
            while (pitanje.Length<1)
            {
                pitanje = Console.ReadLine();
            }
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
            int indeks;
            while (!int.TryParse(Console.ReadLine(), out indeks) || indeks < 1 || indeks > 4)
                Console.WriteLine("Unesite ponovo indeks tacnog odgovora ponovo!");
            StreamWriter upis = new StreamWriter(izborkategorije + ".txt", true);
            upis.WriteLine(pitanje + "|" + ponudjeni_odgovori[0] + "|" + ponudjeni_odgovori[1] + "|" + ponudjeni_odgovori[2] + "|" + ponudjeni_odgovori[3] + "|" + indeks);
            upis.Close();
            kat[broj_kategorije - 1].brojpitanja++;
            PromenaBrojaPitanja("pitanja", broj_kategorije - 1, kat);
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
                red[indeks_kategorije] = Convert.ToString(kat[indeks_kategorije].brojpitanja);

            }
            br.Close();
            StreamWriter broj_pitanja = new StreamWriter(ime_datoteke + ".txt");
            for (int i = 0; i < 7; i++)
            {
                if (i != 6)
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
            for (int i = 0; i < 7; i++)
            {
                if (kategorija == kategorije[i])
                    indeks_kategorije = i + 1;
            }
            return indeks_kategorije;
        }
        //
        static bool ProveraPonavljanjaPitanja(int broj_pitanja, int[] brojevi_pitanja, int indeks_broja_pitanja_u_nizu)
        {
            for (int i = 0; i < brojevi_pitanja.Length; i++)
            {
                if (broj_pitanja == brojevi_pitanja[i] && indeks_broja_pitanja_u_nizu != i)
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
                if (i == niz.Length - 1)
                    Console.WriteLine(niz[1]);
                else
                    Console.Write(niz[1]);
            }
        }
        //izmenjena metoda
        static bool IspisPitanja(Pitanja[] pitanja, int brPitanja, int indeks_kategorije, string[] kategorije, out bool izlaz, out int broj_poena)
        {
            var s = Console.ReadKey();
            izlaz = PrekidKvizaKorisnik(s, kategorije, pitanja);
            DateTime ispis = DateTime.Now;
            Console.WriteLine(pitanja[indeks_kategorije].pitanje[brPitanja]);
            Console.WriteLine("[ ] {0}\n[ ] {1}\n[ ] {2}\n[ ] {3}", pitanja[indeks_kategorije].odgovor1[brPitanja],
            pitanja[indeks_kategorije].odgovor2[brPitanja], pitanja[indeks_kategorije].odgovor3[brPitanja],
            pitanja[indeks_kategorije].odgovor4[brPitanja]);
            int poz = Console.CursorTop;
            Console.SetCursorPosition(1, Console.CursorTop - 4);
            PomerajKursora(0, 3, 0, 1);
            Console.Write("*");
            Console.SetCursorPosition(0, poz + 1);
            TimeSpan odgovor = DateTime.Now-ispis;
            TimeSpan vreme1 = new TimeSpan(0, 0, 0, 10);
            TimeSpan vreme2 = new TimeSpan(0, 0, 0, 13);
            TimeSpan vreme3 = new TimeSpan(0, 0, 0, 16);
            TimeSpan vreme4 = new TimeSpan(0, 0, 0, 19);
            TimeSpan vreme5 = new TimeSpan(0, 0, 0, 21);
            if (kursorY == pitanja[indeks_kategorije].indeks_tacnog_odg[brPitanja] - 1)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Tacno!");
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
                Console.ForegroundColor = ConsoleColor.Red;
                string tacanOdg;
                switch (pitanja[indeks_kategorije].indeks_tacnog_odg[brPitanja])
                {
                    case 1: tacanOdg = pitanja[indeks_kategorije].odgovor1[brPitanja]; break;
                    case 2: tacanOdg = pitanja[indeks_kategorije].odgovor2[brPitanja]; break;
                    case 3: tacanOdg = pitanja[indeks_kategorije].odgovor3[brPitanja]; break;
                    default: tacanOdg = pitanja[indeks_kategorije].odgovor4[brPitanja]; break;
                }
                Console.WriteLine("Netacno! Tacan odgovor je bio " + pitanja[indeks_kategorije].indeks_tacnog_odg[brPitanja] + ". " + tacanOdg);
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
            Console.SetCursorPosition(1, Console.CursorTop - 7);
            PomerajKursora(0, 6, 0, 1);
            return kursorY;
        }
        //
      static void Ranglista(string fajl, string ime, int brpoena)
    {
        StreamReader prethodniFajl = new StreamReader(fajl);
        RangLista[] niz = new RangLista[1000];
        int br = 0;
        while (!prethodniFajl.EndOfStream)
        {
            if (br == niz.Length) Array.Resize(ref niz, niz.Length * 2);
            string red = prethodniFajl.ReadLine();
            string[] pomoc = red.Split('|');
            niz[br].ime = pomoc[0];
            niz[br].poeni = Convert.ToInt32(pomoc[1]);
            br++;
        }
        Array.Resize(ref niz, br);
        prethodniFajl.Close();
        StreamWriter izlaz = new StreamWriter(fajl);
        if (niz.Length != 0)
        {
            bool upisan = false;
            for (int i = 0; i < niz.Length;)
            {
                if (!upisan && brpoena > niz[i].poeni)
                {
                    izlaz.WriteLine(ime + "|" + brpoena);
                    upisan = true;
                }
                else
                {
                    izlaz.WriteLine(niz[i].ime + "|" + niz[i].poeni);
                    i++;
                }
            }
        }
        else
        {
            izlaz.WriteLine(ime + "|" + brpoena);
        }
        izlaz.Close();
    }
      static void PoeniKorisnika(string ime_datoteke, string ime, int broj_bodova)
        {
            StreamWriter datoteka = new StreamWriter(ime_datoteke + ".txt", true);
            datoteka.WriteLine(ime_datoteke + "|" + ime + "|" + broj_bodova);
            datoteka.Close();

        }
        public static void Main(string[] args)
        {
            bool korisnik_izlaz = true;
            while (korisnik_izlaz==true)
            {
            string[] kategorije = 
            {
            "geografija",
            "filmovi",
            "muzika",
            "sport",
            "istorija",
            "biologija",
            "opsteznanje",
              
            };
                Console.WriteLine("KVIZ ZNANJA");
                Console.WriteLine("Unesite ime i prezime :");
                string ime=Console.ReadLine();
                while (ime.Length<1)
                {
                    ime = Console.ReadLine();
                }
                Console.WriteLine("Izaberite kategoriju:");
                int broj_poena = 0;
                int ukupno_poena = 0;
                int katego = IspisIIzborKategorije(kategorije);
                string izborkategorije = kategorije[katego];
                int broj_kategorije = IndeksKategorije(izborkategorije, kategorije);
                Pitanja[] kat = new Pitanja[20];
                UnosPitanjaIzDatoteke(izborkategorije, broj_kategorije - 1, kat);
                //izbor random pitanja iz kategorije
                int[] random_pitanja_indeksi = new int[10];
                int random_broj;
                Random rnd = new Random();
                for (int i = 0; i < 10; i++)
                {
                    random_pitanja_indeksi[i] = rnd.Next(0, kat[broj_kategorije-1].brojpitanja - 1);
                    while (ProveraPonavljanjaPitanja(random_pitanja_indeksi[i], random_pitanja_indeksi, i) == true)
                    {
                        random_broj = rnd.Next(0, kat[broj_kategorije-1].brojpitanja - 1);
                        random_pitanja_indeksi[i] = random_broj;
                    }
                }
                Console.Clear();
                bool izlaz = false;
                for (int i = 0; i < 10; i++)
                {
                    IspisPitanja(kat, random_pitanja_indeksi[i], broj_kategorije - 1, kategorije, out izlaz, out broj_poena);
                    ukupno_poena += broj_poena;
                    if (izlaz == true)
                    {
                        korisnik_izlaz = false;
                        Console.WriteLine("Izasli ste iz kviza.");
                        return; 
                    }

                }
                Console.WriteLine("Vas ukupan broj poena je :"+ukupno_poena+" od mogucih 100.");
                
              Ranglista("ranglista.txt",ime,ukupno_poena);
              Console.ReadKey();
              Console.Clear();
              
              
            }

        }
    }

