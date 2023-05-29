using Microsoft.Win32;
using MyClassLibrary;
using System;
using System.Collections.Generic;
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
                foreach (string filePath in openFileDialog.FileNames)
                {
                    if (photoList.Count < 3)
                    {
                        byte[] imageData = File.ReadAllBytes(filePath);
                        photoList.Add(imageData);

                        for (int i = 0; i < photoList.Count; i++)
                        {
                            if (i < wrapPanel.Children.Count && wrapPanel.Children[i] is Image image)
                            {
                                BitmapImage bitmap = new BitmapImage(new Uri(filePath));
                                image.Source = bitmap;
                            }
                            else
                            {
                                BitmapImage bitmap = new BitmapImage(new Uri(filePath));
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
                }
            }
        }

        private void VerwijderAfbeelding_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            int index = wrapPanel.Children.IndexOf(clickedButton);

            if (index >= 1 && wrapPanel.Children[index - 1] is Image image)
            {
                wrapPanel.Children.RemoveAt(index); 
                wrapPanel.Children.RemoveAt(index - 1);
                int photoIndex = (index - 1) / 2;
                if (photoIndex >= 0 && photoIndex < photoList.Count)
                {
                    photoList.RemoveAt(photoIndex);
                }
            }
        }

        private void BtnOpslaan_Click(object sender, RoutedEventArgs e)
        {
            Voertuig voertuig = new Voertuig();
            voertuig.Naam = naamTxt.Text;
            voertuig.Beschrijving = beschrijvingTxt.Text;
            voertuig.Merk = tbxMerk.Text;
            voertuig.Model = tbxModel.Text;
            voertuig.Gewicht = (int?)Convert.ToInt32(tbxgewicht.Text);
            voertuig.MaxBelasting = (int?)Convert.ToInt32(tbxMax.Text);
            voertuig.Bouwjaar = (int?)Convert.ToInt32(tboxbouwjaar.Text);
            voertuig.Afmetingen = tbxAfmeting.Text;
            voertuig.Geremd = rbJa.IsChecked ?? false;

            voertuig.AddGetrokkenVoertuig(voertuig, currentId.Id); 
            Close();
        }
    }
}
