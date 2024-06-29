using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net.Http;

namespace Projekt.Zaliczeniowy

{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string inputFile = filePathTextBox.Text;

            // Sprawdzenie, czy úcieøka pliku nie jest pusta ani null
            if (!string.IsNullOrEmpty(inputFile))
            {
                string? directoryPath = Path.GetDirectoryName(inputFile);
                string? fileNameWithoutExtension = Path.GetFileNameWithoutExtension(inputFile);
                string? fileExtension = Path.GetExtension(inputFile);

                // Sprawdzenie, czy uzyskane úcieøki nie sπ nullami
                if (!string.IsNullOrEmpty(directoryPath) && !string.IsNullOrEmpty(fileNameWithoutExtension) && !string.IsNullOrEmpty(fileExtension))
                {
                    string outputFile = Path.Combine(directoryPath, fileNameWithoutExtension + "_encrypted" + fileExtension);
                    string password = "your_password"; // ZmieÒ na has≥o uøytkownika

                    EncryptFile(inputFile, outputFile, password);

                    MessageBox.Show("Plik zaszyfrowany!");
                }
                else
                {
                    MessageBox.Show("Nieprawid≥owa úcieøka pliku wejúciowego.");
                }
            }
            else
            {
                MessageBox.Show("Wybierz plik do zaszyfrowania.");
            }
        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void WybierzPlik_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePathTextBox.Text = openFileDialog.FileName;
            }
        }

        private void wybierzFolderButton_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    folderPathTextBox.Text = folderBrowserDialog.SelectedPath;
                }
            }
        }
        private void DecryptFile(string inputFile, string outputFile, string password)
        {
            try
            {
                if (inputFile == null || outputFile == null)
                {
                    throw new ArgumentNullException("åcieøki plikÛw wejúciowego lub wyjúciowego sπ nullami.");
                }

                // Implementacja deszyfrowania pliku
                // Upewnij siÍ, øe ca≥y kod deszyfrowania jest prawid≥owo umieszczony tutaj
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wystπpi≥ b≥πd podczas deszyfrowania pliku: " + ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string inputFile = filePathTextBox.Text;

            // Sprawdzenie, czy úcieøka wejúciowa nie jest pusta
            if (!string.IsNullOrEmpty(inputFile))
            {
                string? directoryPath = Path.GetDirectoryName(inputFile);
                string? fileNameWithoutExtension = Path.GetFileNameWithoutExtension(inputFile);
                string? fileExtension = Path.GetExtension(inputFile);

                // Sprawdzenie, czy uzyskane úcieøki nie sπ nullami
                if (!string.IsNullOrEmpty(directoryPath) && !string.IsNullOrEmpty(fileNameWithoutExtension) && !string.IsNullOrEmpty(fileExtension))
                {
                    string outputFile = Path.Combine(directoryPath!, fileNameWithoutExtension! + "_decrypted" + fileExtension!);
                    string password = "your_password"; // ZmieÒ na has≥o uøytkownika

                    DecryptFile(inputFile, outputFile, password);

                    MessageBox.Show("Plik odszyfrowany!");
                }
                else
                {
                    MessageBox.Show("Nieprawid≥owa úcieøka pliku wejúciowego.");
                }
            }
            else
            {
                MessageBox.Show("Wybierz plik do odszyfrowania.");
            }
        }

        private byte[] GenerateSalt()
        {
            byte[] salt = new byte[32];

        
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return salt;
        }
        private void EncryptFile(string inputFile, string outputFile, string password)
        {
            byte[] salt = GenerateSalt();

            using (FileStream fsCrypt = new FileStream(outputFile, FileMode.Create))
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // UtwÛrz obiekt Aes z zalecanymi parametrami
                using (Aes AES = Aes.Create())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    // Uøyj zalecanego konstruktora Rfc2898DeriveBytes
                    using (var key = new Rfc2898DeriveBytes(passwordBytes, salt, iterations: 10000, HashAlgorithmName.SHA256))
                    {
                        AES.Key = key.GetBytes(AES.KeySize / 8);
                        AES.IV = key.GetBytes(AES.BlockSize / 8);
                    }

                    AES.Padding = PaddingMode.PKCS7;
                    AES.Mode = CipherMode.CFB;

                    // Zapisz sÛl do pliku wynikowego
                    fsCrypt.Write(salt, 0, salt.Length);

                    // UtwÛrz strumieÒ szyfrujπcy
                    using (CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        // OtwÛrz plik wejúciowy
                        using (FileStream fsIn = new FileStream(inputFile, FileMode.Open))
                        {
                            byte[] buffer = new byte[1048576];
                            int read;
                            while ((read = fsIn.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                cs.Write(buffer, 0, read);
                            }
                        }
                    }
                }
            }
        }
        private async Task UploadFileAsync(string filePath, string url)
        {
            using (HttpClient client = new HttpClient())
            {
                using (MultipartFormDataContent content = new MultipartFormDataContent())
                {
                    byte[] fileBytes = File.ReadAllBytes(filePath);
                    content.Add(new ByteArrayContent(fileBytes, 0, fileBytes.Length), "file", Path.GetFileName(filePath));
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    response.EnsureSuccessStatusCode();
                }
            }
        }

        private async void uploadButton_Click(object sender, EventArgs e)
        {
            string inputFile = filePathTextBox.Text;

            // Sprawdzenie, czy úcieøka pliku nie jest pusta ani null
            if (!string.IsNullOrEmpty(inputFile))
            {
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        using (var content = new MultipartFormDataContent())
                        {
                            using (var fileStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
                            {
                                // Dodaj plik do treúci øπdania
                                content.Add(new StreamContent(fileStream), "file", Path.GetFileName(inputFile));

                                // Wyúlij øπdanie POST do serwera
                                var response = await httpClient.PostAsync("https://your-server-url/upload", content);

                                // Sprawdü, czy øπdanie zosta≥o pomyúlnie wykonane
                                if (response.IsSuccessStatusCode)
                                {
                                    MessageBox.Show("Plik zosta≥ pomyúlnie przes≥any na serwer.");
                                }
                                else
                                {
                                    MessageBox.Show($"Wystπpi≥ problem podczas przesy≥ania pliku. Kod odpowiedzi: {response.StatusCode}");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Wystπpi≥ b≥πd: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Wybierz plik do przes≥ania.");
            }
        }

    }
}