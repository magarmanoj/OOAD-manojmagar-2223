using Microsoft.Win32;
using MyClassLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for Addgetrokken.xaml
    /// </summary>
    public partial class Addgetrokken : Window
    {
        List<byte[]> photoList = new List<byte[]>();
        private Gebruiker currentId;

        public Addgetrokken(Gebruiker userId)
        {
            InitializeComponent();
            currentId = userId;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.Filter = "Image Files (*.jpg, *.png, *.jpeg)|*.jpg;*.png;*.jpeg";
            openFileDialog.Multiselect = true;
            bool? dialogResult = openFileDialog.ShowDialog();
            if (dialogResult == true)
            {
                int remainingSlots = 3 - photoList.Count;

                if (openFileDialog.FileNames.Length > remainingSlots)
                {
                    MessageBox.Show("You can only add up to 3 photos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                try
                {
                    foreach (string filePath in openFileDialog.FileNames)
                    {
                        byte[] imageData = File.ReadAllBytes(filePath);
                        photoList.Add(imageData);
                    }
                }
                catch (IOException)
                {
                    MessageBox.Show("FOUT: kan doelbestand niet schrijven");
                    return;
                }
                btnAdd.IsEnabled = photoList.Count < 3;
                DisplayPhotos();
            }
        }

        private void DisplayPhotos()
        {
            wrapPanel.Children.Clear();

            for (int i = 0; i < photoList.Count; i++)
            {
                byte[] imageData = photoList[i];
                using (MemoryStream stream = new MemoryStream(imageData))
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = stream;
                    bitmap.EndInit();

                    Image newImage = new Image();
                    newImage.Width = 220;
                    newImage.Height = 220;
                    newImage.Source = bitmap;
                    wrapPanel.Children.Add(newImage);

                    // Create a new Button control and add it to the WrapPanel
                    Button newButton = new Button();
                    newButton.Name = "btnVerwijder" + (i + 1);
                    newButton.Content = "X";
                    newButton.HorizontalAlignment = HorizontalAlignment.Right;
                    newButton.VerticalAlignment = VerticalAlignment.Top;
                    newButton.Background = Brushes.Transparent;
                    newButton.Click += VerwijderAfbeelding_Click;
                    wrapPanel.Children.Add(newButton);
                }
            }
        }

        private void VerwijderAfbeelding_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            int index = wrapPanel.Children.IndexOf(clickedButton);

            if (index >= 1 && index % 2 == 1)
            {
                int photoIndex = (index - 1) / 2;
                if (photoIndex >= 0 && photoIndex < photoList.Count)
                {
                    photoList.RemoveAt(photoIndex);
                }

                wrapPanel.Children.RemoveRange(index - 1, 2);
            }
        }

        private void BtnOpslaan_Click(object sender, RoutedEventArgs e)
        {
            if (photoList.Count == 0)
            {
                MessageBox.Show("One or more images are missing.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Voertuig voertuig = new Voertuig();
            voertuig.Merk = tbxMerk.Text;
            voertuig.Model = tbxModel.Text;                  
            voertuig.Afmetingen = tbxAfmeting.Text;
            voertuig.Geremd = rbJa.IsChecked ?? false;

            if (string.IsNullOrEmpty(naamTxt.Text))
            {
                MessageBox.Show("Vul een geldig naam in.", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(beschrijvingTxt.Text))
            {
                MessageBox.Show("Vul een geldig bouwjaar in.", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!int.TryParse(tboxbouwjaar.Text, out int bouwjaar))
            {
                MessageBox.Show("Vul een geldig bouwjaar in.", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            voertuig.Naam = naamTxt.Text;
            voertuig.Beschrijving = beschrijvingTxt.Text;
            voertuig.Bouwjaar = bouwjaar;
            if (string.IsNullOrEmpty(tbxgewicht.Text))
            {
                voertuig.Gewicht = null;
            }
            else if (!int.TryParse(tbxgewicht.Text, out int gewicht))
            {
                MessageBox.Show("Vul een geldig gewicht in. (Getal)", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                voertuig.Gewicht = gewicht;
            }

            if (string.IsNullOrEmpty(tbxMax.Text))
            {
                voertuig.MaxBelasting = null;
            }
            else if (!int.TryParse(tbxMax.Text, out int maxBelasting))
            {
                MessageBox.Show("Vul een geldig maxbelasting in. (Getal)", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                voertuig.MaxBelasting = maxBelasting;
            }

            try
            {
                int voertuigId = voertuig.AddGetrokkenVoertuig(voertuig, currentId.Id);
                Foto foto = new Foto();
                foreach (byte[] imageData in photoList)
                {
                    foto.AddPhotos(imageData, voertuigId);
                }
                PageVoertuigen.Instance.ShowPhotoAndInfo();
                Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("An error  while performing the SQL operation: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void BtnAnnuleren_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
