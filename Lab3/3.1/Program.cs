using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace EncryptionApp
{
    public partial class MainForm : Form
    {
        private SymmetricAlgorithm selectedAlgorithm;
        private Stopwatch encryptionTimer = new Stopwatch();
        private Stopwatch decryptionTimer = new Stopwatch();

        public MainForm()
        {
            InitializeComponent();

            // Inicjalizacja ComboBox z algorytmami szyfrowania
            comboBoxAlgorithms.Items.Add("AES");
            comboBoxAlgorithms.Items.Add("DES");
            comboBoxAlgorithms.SelectedIndex = 0; // Ustawienie domyślnego algorytmu

            // Ustawienie początkowe timera
            labelEncryptionTime.Text = "Encryption time: 0 ms";
            labelDecryptionTime.Text = "Decryption time: 0 ms";
        }

        private void buttonGenerateKeys_Click(object sender, EventArgs e)
        {
            // Wybór algorytmu na podstawie ComboBox
            switch (comboBoxAlgorithms.SelectedItem.ToString())
            {
                case "AES":
                    selectedAlgorithm = Aes.Create();
                    break;
                case "DES":
                    selectedAlgorithm = DES.Create();
                    break;
                default:
                    MessageBox.Show("Select an encryption algorithm.");
                    return;
            }

            // Generowanie kluczy
            selectedAlgorithm.GenerateKey();
            selectedAlgorithm.GenerateIV();

            // Wyświetlanie kluczy i IV
            textBoxKey.Text = Convert.ToBase64String(selectedAlgorithm.Key);
            textBoxIV.Text = Convert.ToBase64String(selectedAlgorithm.IV);
        }

        private void buttonEncrypt_Click(object sender, EventArgs e)
        {
            if (selectedAlgorithm == null)
            {
                MessageBox.Show("Select an encryption algorithm and generate keys first.");
                return;
            }

            string plainText = textBoxPlainText.Text;

            // Resetowanie timera szyfrowania
            encryptionTimer.Restart();

            // Szyfrowanie tekstu
            byte[] encryptedBytes;
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (ICryptoTransform encryptor = selectedAlgorithm.CreateEncryptor())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                }
                encryptedBytes = msEncrypt.ToArray();
            }

            // Zatrzymanie timera szyfrowania i aktualizacja interfejsu
            encryptionTimer.Stop();
            labelEncryptionTime.Text = $"Encryption time: {encryptionTimer.ElapsedMilliseconds} ms";

            // Wyświetlanie tekstu zaszyfrowanego w formacie ASCII i HEX
            textBoxEncryptedTextASCII.Text = Convert.ToBase64String(encryptedBytes);
            textBoxEncryptedTextHex.Text = BitConverter.ToString(encryptedBytes).Replace("-", "");
        }

        private void buttonDecrypt_Click(object sender, EventArgs e)
        {
            if (selectedAlgorithm == null)
            {
                MessageBox.Show("Select an encryption algorithm and generate keys first.");
                return;
            }

            // Odczytanie tekstu zaszyfrowanego
            byte[] encryptedBytes = Convert.FromBase64String(textBoxEncryptedTextASCII.Text);

            // Resetowanie timera deszyfrowania
            decryptionTimer.Restart();

            // Deszyfrowanie tekstu
            string decryptedText;
            using (MemoryStream msDecrypt = new MemoryStream(encryptedBytes))
            {
                using (ICryptoTransform decryptor = selectedAlgorithm.CreateDecryptor())
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            decryptedText = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            // Zatrzymanie timera deszyfrowania i aktualizacja interfejsu
            decryptionTimer.Stop();
            labelDecryptionTime.Text = $"Decryption time: {decryptionTimer.ElapsedMilliseconds} ms";

            // Wyświetlenie odszyfrowanego tekstu
            textBoxDecryptedText.Text = decryptedText;
        }
    }
}
