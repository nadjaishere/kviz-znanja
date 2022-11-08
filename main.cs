using System;
using System.IO;

class Program {
  static void UpisPodataka(string ime_datoteke_kategorije,string pitanje,string[] ponudjeni_odgovori,int 
                           indeks_tacnog_odg)
        {
            StreamWriter upis = new StreamWriter(ime_datoteke_kategorije, true);
            upis.WriteLine(pitanje+"|"+ponudjeni_odgovori[0]+ "|"+ ponudjeni_odgovori[1]+ "|"+ 
            ponudjeni_odgovori[2]+ "|"+ponudjeni_odgovori[3]+ "|"+indeks_tacnog_odg);
            upis.Close();
        }
  public static void Main (string[] args) 
  {
    Console.WriteLine ("Hello World");
  }
}