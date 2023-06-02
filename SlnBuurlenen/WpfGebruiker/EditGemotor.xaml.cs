using Microsoft.Win32;
using MyClassLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for EditGemotor.xaml
    /// </summary>
    public partial class EditGemotor : Page
    {
        List<byte[]> photoList = new List<byte[]>();
        private Voertuig selectedVoertuig;
        private bool textChanged = false;
        private bool selectionChanged = false;
        private List<int> photosToDelete = new List<int>();

        OpenFileDialog openFileDialog = new OpenFileDialog();

        public EditGemotor(Voertuig voertuig)
        {
            InitializeComponent();
            selectedVoertuig = voertuig;
            Fotolist();

            if (photoList.Count >= 3)
            {
                btnAdd.IsEnabled = false;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
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
                    int photoId = GetPhotoIdByIndex(photoIndex);
                    photosToDelete.Add(photoId);
                }
                wrapPanel.Children.RemoveRange(index - 1, 2);
                photoList.RemoveAt(photoIndex);

                if (photoList.Count < 3)
                {
                    btnAdd.IsEnabled = true;
                }
            }
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            if (photoList.Count == 0)
            {
                MessageBox.Show("One or more images are missing.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (textChanged || selectionChanged)
            {
                if (string.IsNullOrEmpty(name.Text))
                {
                    MessageBox.Show("Vul een geldig naam in.", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                selectedVoertuig.Naam = name.Text;

                if (string.IsNullOrEmpty(beschrijving.Text))
                {
                    MessageBox.Show("Vul een geldig beschrijving in.", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                selectedVoertuig.Beschrijving = beschrijving.Text;

                if (string.IsNullOrEmpty(bouwjaar.Text))
                {
                    MessageBox.Show("Vul een geldig bouwjaar in.", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                selectedVoertuig.Bouwjaar = (int)Convert.ToInt64(bouwjaar.Text);

                selectedVoertuig.Merk = merk.Text;
                selectedVoertuig.Model = model.Text;

                if (brandstofComboBox.SelectedIndex != 0)
                {
                    selectedVoertuig.Brandstof = (Enums.BrandstofType)brandstofComboBox.SelectedIndex;
                }
                else
                {
                    selectedVoertuig.Brandstof = null;
                }

                if (transmissieComboBox.SelectedIndex != 0)
                {
                    selectedVoertuig.Transmissie = (Enums.TransmissieType)transmissieComboBox.SelectedIndex;
                }
                else
                {
                    selectedVoertuig.Transmissie = null;
                }
                selectedVoertuig.UpdateVoertuig(selectedVoertuig.Type);
            }

            Foto foto = new Foto();

            int startIndex = photoList.Count - openFileDialog.FileNames.Length;

            for (int i = startIndex; i < photoList.Count; i++)
            {
                byte[] imageData = photoList[i];
                int newPhotoId = foto.AddPhotos(imageData, selectedVoertuig.Id);
                foto.UpdatePhoto(imageData, newPhotoId);
            }

            foreach (int photoId in photosToDelete)
            {
                foto.DeletePhoto(selectedVoertuig.Id, photoId);
            }
            MessageBox.Show("You changes has been saved");
            PageVoertuigen.Instance.ShowPhotoAndInfo();
            Window.GetWindow(this).Close();
        }

        private int GetPhotoIdByIndex(int photoIndex)
        {
            List<Foto> fotos = Foto.GetFotoListByVoertuigId(selectedVoertuig.Id); 

            if (photoIndex >= 0 && photoIndex < fotos.Count)
            {
                return fotos[photoIndex].Id;
            }
            return -1;
        }

        private void Fotolist()
        {
            List<Foto> fotoList = Foto.GetFotoListByVoertuigId(selectedVoertuig.Id);

            wrapPanel.Children.Clear();
            photoList.Clear();

            for (int i = 0; i < fotoList.Count; i++) 
            {
                byte[] imageData = fotoList[i].Data;
                photoList.Add(imageData);
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
            Gebruiker eigenaars = Gebruiker.GetGebruikerById(selectedVoertuig.EigenaarId);
            name.Text = selectedVoertuig.Naam;
            beschrijving.Text = selectedVoertuig.Beschrijving;
            merk.Text = !string.IsNullOrEmpty(selectedVoertuig.Merk) ? selectedVoertuig.Merk : "n.v.t";
            bouwjaar.Text = selectedVoertuig.Bouwjaar.HasValue ? selectedVoertuig.Bouwjaar.Value.ToString() : "N/A";
            model.Text = !string.IsNullOrEmpty(selectedVoertuig.Model) ? selectedVoertuig.Model : "n.v.t";
            eigenaar.Text = eigenaars != null ? $"{eigenaars.Voornaam} {eigenaars.Achternaam}" : "Onbekend";
            if (selectedVoertuig.Brandstof.HasValue)
                brandstofComboBox.SelectedIndex = (int)selectedVoertuig.Brandstof;
            else
                brandstofComboBox.SelectedIndex = 0;
            if (selectedVoertuig.Transmissie.HasValue)
                transmissieComboBox.SelectedIndex = (int)selectedVoertuig.Transmissie;
            else
                transmissieComboBox.SelectedIndex = 0;

            merk.TextChanged += TextBox_TextChanged;
            model.TextChanged += TextBox_TextChanged;
            transmissieComboBox.SelectionChanged += Text_SelectionChanged;
            bouwjaar.TextChanged += TextBox_TextChanged;
            brandstofComboBox.SelectionChanged += Text_SelectionChanged;
            name.TextChanged += TextBox_TextChanged;
            beschrijving.TextChanged += TextBox_TextChanged;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textChanged = true;
        }
        private void Text_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectionChanged = true;
        }

        private void BtnAnnuleren_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
