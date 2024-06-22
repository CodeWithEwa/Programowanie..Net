using System;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        string encryptedHex = "23c73dde8faedd91413fb5dd1d7e066d70425ed1e058d0e2f7e9e43501824a95446baf28f6ce7ffd3c544f40efb5c80f235de1321214328781a6ea0c0c4c7b74be3968ca1ffb8455";

        byte[] encryptedBytes = HexStringToByteArray(encryptedHex);

        Console.WriteLine("Starting brute force attack on DES cipher...");
        Console.WriteLine();

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        bool foundPlainText = false;
        long attempts = 0;

        // Przypuszczamy, że klucz ma 6 bajtów, a ostatnie 4 to cyfry 5
        byte[] partialKey = new byte[8]; // DES używa kluczy 8 bajtowych
        byte[] fullKey = new byte[8];

        // Ustawienie pierwszych 4 bajtów klucza
        Array.Copy(Encoding.ASCII.GetBytes("test"), partialKey, 4);

        // Próba wszystkich możliwych kombinacji dla ostatnich 4 bajtów
        for (int i = 0; i <= 9999; i++)
        {
            // Ustawienie ostatnich 4 bajtów klucza jako cyfr 5
            string lastFourDigits = i.ToString("D4");
            Array.Copy(Encoding.ASCII.GetBytes(lastFourDigits), 0, partialKey, 4, 4);

            // Próba deszyfrowania z użyciem aktualnego klucza
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = partialKey;
                des.IV = new byte[8]; // W przypadku DES IV jest opcjonalne, ustawiamy na zero

                try
                {
                    using (ICryptoTransform decryptor = des.CreateDecryptor())
                    {
                        byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

                        // Sprawdzenie czy deszyfrowany tekst zaczyna się od "test"
                        string decryptedText = Encoding.UTF8.GetString(decryptedBytes);
                        if (decryptedText.StartsWith("test"))
                        {
                            foundPlainText = true;
                            fullKey = partialKey;
                            Console.WriteLine($"Found plaintext: {decryptedText}");
                            break;
                        }
                    }
                }
                catch (CryptographicException)
                {
                    // Błąd deszyfrowania (np. niepoprawny klucz), ignorujemy
                }
            }

            attempts++;
        }

        stopwatch.Stop();

        if (foundPlainText)
        {
            Console.WriteLine();
            Console.WriteLine($"Success! Key found after {attempts} attempts.");
            Console.WriteLine($"Key (HEX): {ByteArrayToHexString(fullKey)}");
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine($"Failed to find plaintext after {attempts} attempts.");
        }

        Console.WriteLine($"Elapsed time: {stopwatch.Elapsed.TotalSeconds:F2} seconds");
        Console.ReadLine();
    }

    // Konwersja stringa HEX na tablicę bajtów
    static byte[] HexStringToByteArray(string hex)
    {
        int numberChars = hex.Length;
        byte[] bytes = new byte[numberChars / 2];
        for (int i = 0; i < numberChars; i += 2)
        {
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        }
        return bytes;
    }

    // Konwersja tablicy bajtów na stringa HEX
    static string ByteArrayToHexString(byte[] bytes)
    {
        StringBuilder hex = new StringBuilder(bytes.Length * 2);
        foreach (byte b in bytes)
        {
            hex.AppendFormat("{0:x2}", b);
        }
        return hex.ToString();
    }
}
