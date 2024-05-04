using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using System;
using System.IO;

class Program
{
    static void Main()
    {
      
        string filePath = "sciezka/do/pliku.txt";

        try
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                
                while ((line = sr.ReadLine()) != null)
                {
                    Console.WriteLine(ReverseString(line));
                }
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Plik nie został znaleziony.");
        }
        catch (IOException)
        {
            Console.WriteLine("Wystąpił błąd wejścia/wyjścia podczas odczytu pliku.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Wystąpił nieoczekiwany błąd: " + ex.Message);
        }
    }

    // Metoda do odwracania ciągu znaków
    static string ReverseString(string str)
    {
        char[] charArray = str.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }
}
