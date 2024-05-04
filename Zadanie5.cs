using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {

        string zrodlo = "plik_zrodlowy.txt";
      
        string docelowy = "plik_docelowy.txt";

        try
        {
            using (FileStream fsZrodlo = new FileStream(zrodlo, FileMode.Open, FileAccess.Read))
          
            using (FileStream fsDocelowy = new FileStream(docelowy, FileMode.Create, FileAccess.Write))
            {
                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = fsZrodlo.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fsDocelowy.Write(buffer, 0, bytesRead);
                }
            }

            Console.WriteLine("Kopiowanie zakończone.");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Plik źródłowy nie został znaleziony.");
        }
        catch (IOException ex)
        {
            Console.WriteLine("Wystąpił błąd wejścia/wyjścia: " + ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Wystąpił nieoczekiwany błąd: " + ex.Message);
        }
    }
}
