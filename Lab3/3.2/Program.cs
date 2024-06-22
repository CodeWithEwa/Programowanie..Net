using System.Diagnostics;
using System.Security.Cryptography;

class Program
{
    static int rozmiarBloku = 16;

    static void Main()
    {
        string[] algorytmy = { "AES-128-CSP", "AES-256-CSP", "AES-128-Managed", "AES-256-Managed", "Rijndael-128-Managed", "Rijndael-256-Managed", "DES", "3DES" };

        byte[] dane = new byte[1024 * 1024 * 128];
        new Random().NextBytes(dane);

        foreach (string algorytm in algorytmy)
        {
            Console.Write($"{algorytm} | ");
            double czasNaBlokRAM;
            double czasRAM = MierzWydajnosc(algorytm, dane, false, out czasNaBlokRAM);
            long bajtyNaSekundeRAM = (long)Math.Round(dane.Length / czasRAM);

            double czasNaBlokHDD;
            double czasHDD = MierzWydajnosc(algorytm, dane, true, out czasNaBlokHDD);
            long bajtyNaSekundeHDD = (long)Math.Round(dane.Length / czasHDD);

            Console.WriteLine($"{czasNaBlokRAM} | {bajtyNaSekundeRAM} | {bajtyNaSekundeHDD}");
        }
    }

    static double MierzWydajnosc(string algorytm, byte[] dane, bool zHDD, out double czasNaBlok)
    {
        Stopwatch stoper = new Stopwatch();
        byte[] zaszyfrowane = null;

        if (zHDD)
        {
            string plikTymczasowy = Path.GetTempFileName();
            File.WriteAllBytes(plikTymczasowy, dane);
            stoper.Start();
            zaszyfrowane = SzyfrujDaneZHDD(algorytm, plikTymczasowy);
            stoper.Stop();
            File.Delete(plikTymczasowy);
        }
        else
        {
            stoper.Start();
            zaszyfrowane = SzyfrujDaneWRam(algorytm, dane);
            stoper.Stop();
        }

        czasNaBlok = stoper.Elapsed.TotalSeconds / (dane.Length / (double)rozmiarBloku);

        return stoper.Elapsed.TotalSeconds;
    }

    static byte[] SzyfrujDaneWRam(string algorytm, byte[] dane)
    {
        using (var algo = StworzAlgorytm(algorytm))
        {
            algo.GenerateKey();
            algo.GenerateIV();

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, algo.CreateEncryptor(algo.Key, algo.IV), CryptoStreamMode.Write))
                {
                    cs.Write(dane, 0, dane.Length);
                    cs.FlushFinalBlock();

                    return ms.ToArray();
                }
            }
        }
    }

    static byte[] SzyfrujDaneZHDD(string algorytm, string sciezkaPliku)
    {
        using (var algo = StworzAlgorytm(algorytm))
        {
            algo.GenerateKey();
            algo.GenerateIV();

            using (FileStream fsInput = new FileStream(sciezkaPliku, FileMode.Open, FileAccess.Read))
            {
                using (MemoryStream msOutput = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(msOutput, algo.CreateEncryptor(algo.Key, algo.IV), CryptoStreamMode.Write))
                    {
                        byte[] bufor = new byte[4096];
                        int bajtyOdczytane;

                        while ((bajtyOdczytane = fsInput.Read(bufor, 0, bufor.Length)) > 0)
                            cs.Write(bufor, 0, bajtyOdczytane);

                        cs.FlushFinalBlock();

                        return msOutput.ToArray();
                    }
                }
            }
        }
    }

    static SymmetricAlgorithm StworzAlgorytm(string algorytm)
    {
        switch (algorytm)
        {
            case "AES-128-CSP":
                return new AesCryptoServiceProvider { KeySize = 128 };
            case "AES-256-CSP":
                return new AesCryptoServiceProvider { KeySize = 256 };
            case "AES-128-Managed":
                return new AesManaged { KeySize = 128 };
            case "AES-256-Managed":
                return new AesManaged { KeySize = 256 };
            case "Rijndael-128-Managed":
                return new RijndaelManaged { KeySize = 128 };
            case "Rijndael-256-Managed":
                return new RijndaelManaged { KeySize = 256 };
            case "DES":
                return new DESCryptoServiceProvider();
            case "3DES":
                return new TripleDESCryptoServiceProvider();
        }

        return null;
    }
}