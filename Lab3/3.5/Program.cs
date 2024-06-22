using System;
using System.Drawing;
using System.Windows.Forms;

namespace SinusoidPlotApp
{
    public partial class MainForm : Form
    {
        private const int plotWidth = 600;
        private const int plotHeight = 400;
        private const int offsetX = 50;
        private const int offsetY = 200;
        private const float step = 0.1f;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            // Tworzenie obiektu Graphics z kontrolki rysującej (PictureBox lub Form)
            Graphics g = e.Graphics;

            // Ustawienie koloru linii wykresu
            Pen pen = new Pen(Color.Blue);

            // Rysowanie osi X i Y
            g.DrawLine(Pens.Black, offsetX, 0, offsetX, plotHeight);
            g.DrawLine(Pens.Black, offsetX, offsetY, plotWidth, offsetY);

            // Rysowanie sinusoidy
            float prevX = 0, prevY = 0;
            for (float x = 0; x < plotWidth - offsetX; x += step)
            {
                float y = (float)(Math.Sin(x / 100) * 100); // funkcja sinus

                // Przeliczenie współrzędnych na piksele
                float pixelX = x + offsetX;
                float pixelY = -y + offsetY;

                // Rysowanie linii łączącej punkty
                if (x > 0)
                    g.DrawLine(pen, prevX, prevY, pixelX, pixelY);

                prevX = pixelX;
                prevY = pixelY;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Ustawienie rozmiaru okna
            this.Size = new Size(plotWidth, plotHeight + 50);
        }
    }
}
