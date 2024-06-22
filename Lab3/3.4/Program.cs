using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main()
    {
        // Generowanie kluczy RSA
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            try
            {
                // Wygenerowanie kluczy
                RSAParameters publicKey = rsa.ExportParameters(false); // klucz publiczny
                RSAParameters privateKey = rsa.ExportParameters(true); // klucz prywatny

                // Zapis klucza publicznego do pliku (opcjonalnie)
                string publicKeyPath = "publicKey.xml";
                File.WriteAllText(publicKeyPath, rsa.ToXmlString(false));
                Console.WriteLine($"Public key saved to {publicKeyPath}");

                // Zapis klucza prywatnego do pliku (opcjonalnie)
                string privateKeyPath = "privateKey.xml";
                File.WriteAllText(privateKeyPath, rsa.ToXmlString(true));
                Console.WriteLine($"Private key saved to {privateKeyPath}");

                // Ścieżki plików wejściowego i wynikowego
                string inputFile = "plaintext.txt";
                string encryptedFile = "encryptedFile.encrypted";
                string decryptedFile = "decryptedFile.decrypted";

                // Szyfrowanie pliku
                EncryptFile(inputFile, encryptedFile, publicKey);

                // Deszyfrowanie pliku
                DecryptFile(encryptedFile, decryptedFile, privateKey);

                Console.WriteLine("Encryption and decryption completed successfully.");
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        Console.ReadLine();
    }

    // Funkcja szyfrująca plik za pomocą klucza publicznego RSA
    static void EncryptFile(string inputFile, string outputFile, RSAParameters publicKey)
    {
        try
        {
            // Wczytanie danych do zaszyfrowania
            byte[] dataToEncrypt = File.ReadAllBytes(inputFile);

            // Utworzenie obiektu RSA z klucza publicznego
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(publicKey);

                // Szyfrowanie danych
                byte[] encryptedData = rsa.Encrypt(dataToEncrypt, false);

                // Zapis zaszyfrowanych danych do pliku
                File.WriteAllBytes(outputFile, encryptedData);

                Console.WriteLine($"File encrypted and saved as {outputFile}");
            }
        }
        catch (IOException e)
        {
            Console.WriteLine($"Error reading or writing file: {e.Message}");
        }
        catch (CryptographicException e)
        {
            Console.WriteLine($"Encryption error: {e.Message}");
        }
    }

    // Funkcja deszyfrująca plik za pomocą klucza prywatnego RSA
    static void DecryptFile(string inputFile, string outputFile, RSAParameters privateKey)
    {
        try
        {
            // Wczytanie zaszyfrowanych danych
            byte[] encryptedData = File.ReadAllBytes(inputFile);

            // Utworzenie obiektu RSA z klucza prywatnego
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(privateKey);

                // Deszyfrowanie danych
                byte[] decryptedData = rsa.Decrypt(encryptedData, false);

                // Zapis odszyfrowanych danych do pliku
                File.WriteAllBytes(outputFile, decryptedData);

                Console.WriteLine($"File decrypted and saved as {outputFile}");
            }
        }
        catch (IOException e)
        {
            Console.WriteLine($"Error reading or writing file: {e.Message}");
        }
        catch (CryptographicException e)
        {
            Console.WriteLine($"Decryption error: {e.Message}");
        }
    }
}
