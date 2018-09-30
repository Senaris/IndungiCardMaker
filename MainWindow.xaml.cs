using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;

namespace LoL_Card_Maker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitStuff();
        }

        private void InitStuff()
        {
            inputSummName.Text = "Completeaza Nume.";
            if (!Directory.Exists("Champions"))
                Directory.CreateDirectory("Champions");

            List<string> ImagesFound = new List<string>();
            foreach (var item in Directory.GetFiles("Champions"))
            {
                if(item.ToLower().EndsWith(".png") || item.ToLower().EndsWith(".jpg") || item.ToLower().EndsWith(".jpeg"))
                {
                    ImagesFound.Add(item);
                }
            }

            if(ImagesFound.Count() == 0)
            {
                MessageBox.Show("Adauga imagini in directorul Champions! Altfel nu putem face nimic, logic nu?", "ZinGuard te Bantuie!");
                this.Close();
            }

            // Sort
            ImagesFound.Sort();

            // Adaugam in lista.
            foreach (var item in ImagesFound)
                inputChampSelect.Items.Add(item);
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            // Check
            if(inputSummName.Text.Count() < 3)
            {
                MessageBox.Show("Nu se accepta mai putin de 3 caractere in numele jucatorului.");
                return;
            }

            if(inputChampSelect.SelectedIndex == -1)
            {
                MessageBox.Show("Alege domne un campion...");
                return;
            }

            // Check image
            if(!File.Exists((string)inputChampSelect.SelectedItem))
            {
                MessageBox.Show("Nu gasesc imaginea: " + (string)inputChampSelect.SelectedItem);
                return;
            }

            // Process
            CreateCard(inputSummName.Text, (string)inputChampSelect.SelectedItem).Save(inputSummName.Text.Trim().Replace(" ", "") + ".png");

            // Inform
            MessageBox.Show("Gata! Ai fisierul in directorul aplicatiei.");
        }

        static Bitmap CreateCard(string userName, string ImagePath)
        {
            System.Drawing.Image finalImage = new Bitmap(Properties.Resources.cadru);
            System.Drawing.Image championImage = System.Drawing.Image.FromFile(ImagePath);
            System.Drawing.Image logoImage = new Bitmap(Properties.Resources.logo);

            Bitmap b = new Bitmap(268, 479);
            using (var test = Graphics.FromImage(b))
            {
                // Draw image
                test.DrawImage(championImage, 15, 17, b.Size.Width - 30, b.Size.Height - 103);
                test.DrawImage(finalImage, 0, 0, b.Size.Width, b.Size.Height);
                test.DrawImage(logoImage, 23, b.Size.Height - 73, 43, 48);

                var Name = userName;


                var drawMinusCalculation = 0;
                if (Name.Count() >= 7 && Name.Count() < 12)
                    drawMinusCalculation = Name.Count() * 2;

                if (Name.Count() >= 12 && Name.Count() <= 15)
                    drawMinusCalculation = Name.Count() * 3;

                if (Name.Count() > 15)
                    drawMinusCalculation = Name.Count() * 4;

                test.DrawString(Name, new Font("Tahoma", 12, System.Drawing.FontStyle.Bold), Brushes.White, 116 - drawMinusCalculation, b.Size.Height - 100);
            }

            return b;
        }
    }
}
