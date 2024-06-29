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

            // Sprawdzenie, czy �cie�ka pliku nie jest pusta ani null
            if (!string.IsNullOrEmpty(inputFile))
            {
                string? directoryPath = Path.GetDirectoryName(inputFile);
                string? fileNameWithoutExtension = Path.GetFileNameWithoutExtension(inputFile);
                string? fileExtension = Path.GetExtension(inputFile);

                // Sprawdzenie, czy uzyskane �cie�ki nie s� nullami
                if (!string.IsNullOrEmpty(directoryPath) && !string.IsNullOrEmpty(fileNameWithoutExtension) && !string.IsNullOrEmpty(fileExtension))
                {
                    string outputFile = Path.Combine(directoryPath, fileNameWithoutExtension + "_encrypted" + fileExtension);
                    string password = "your_password"; // Zmie� na has�o u�ytkownika

                    EncryptFile(inputFile, outputFile, password);

                    MessageBox.Show("Plik zaszyfrowany!");
                }
                else
                {
                    MessageBox.Show("Nieprawid�owa �cie�ka pliku wej�ciowego.");
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
                    throw new ArgumentNullException("�cie�ki plik�w wej�ciowego lub wyj�ciowego s� nullami.");
                }

                // Implementacja deszyfrowania pliku
                // Upewnij si�, �e ca�y kod deszyfrowania jest prawid�owo umieszczony tutaj
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wyst�pi� b��d podczas deszyfrowania pliku: " + ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string inputFile = filePathTextBox.Text;

            // Sprawdzenie, czy �cie�ka wej�ciowa nie jest pusta
            if (!string.IsNullOrEmpty(inputFile))
            {
                string? directoryPath = Path.GetDirectoryName(inputFile);
                string? fileNameWithoutExtension = Path.GetFileNameWithoutExtension(inputFile);
                string? fileExtension = Path.GetExtension(inputFile);

                // Sprawdzenie, czy uzyskane �cie�ki nie s� nullami
                if (!string.IsNullOrEmpty(directoryPath) && !string.IsNullOrEmpty(fileNameWithoutExtension) && !string.IsNullOrEmpty(fileExtension))
                {
                    string outputFile = Path.Combine(directoryPath!, fileNameWithoutExtension! + "_decrypted" + fileExtension!);
                    string password = "your_password"; // Zmie� na has�o u�ytkownika

                    DecryptFile(inputFile, outputFile, password);

                    MessageBox.Show("Plik odszyfrowany!");
                }
                else
                {
                    MessageBox.Show("Nieprawid�owa �cie�ka pliku wej�ciowego.");
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

                // Utw�rz obiekt Aes z zalecanymi parametrami
                using (Aes AES = Aes.Create())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    // U�yj zalecanego konstruktora Rfc2898DeriveBytes
                    using (var key = new Rfc2898DeriveBytes(passwordBytes, salt, iterations: 10000, HashAlgorithmName.SHA256))
                    {
                        AES.Key = key.GetBytes(AES.KeySize / 8);
                        AES.IV = key.GetBytes(AES.BlockSize / 8);
                    }

                    AES.Padding = PaddingMode.PKCS7;
                    AES.Mode = CipherMode.CFB;

                    // Zapisz s�l do pliku wynikowego
                    fsCrypt.Write(salt, 0, salt.Length);

                    // Utw�rz strumie� szyfruj�cy
                    using (CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        // Otw�rz plik wej�ciowy
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

            // Sprawdzenie, czy �cie�ka pliku nie jest pusta ani null
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
                                // Dodaj plik do tre�ci ��dania
                                content.Add(new StreamContent(fileStream), "file", Path.GetFileName(inputFile));

                                // Wy�lij ��danie POST do serwera
                                var response = await httpClient.PostAsync("https://your-server-url/upload", content);

                                // Sprawd�, czy ��danie zosta�o pomy�lnie wykonane
                                if (response.IsSuccessStatusCode)
                                {
                                    MessageBox.Show("Plik zosta� pomy�lnie przes�any na serwer.");
                                }
                                else
                                {
                                    MessageBox.Show($"Wyst�pi� problem podczas przesy�ania pliku. Kod odpowiedzi: {response.StatusCode}");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Wyst�pi� b��d: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Wybierz plik do przes�ania.");
            }
        }

    }
}