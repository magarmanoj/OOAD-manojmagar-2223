using Microsoft.Win32;
using MyClassLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for EditGemotor.xaml
    /// </summary>
    public partial class EditGemotor : Page
    {
        List<byte[]> photoList = new List<byte[]>();
        private Voertuig selectedVoertuig;
        private int userId;
        private bool textChanged = false;
        private bool selectionChanged = false;
        private List<int> deletedPhotoIndices = new List<int>();

        OpenFileDialog openFileDialog = new OpenFileDialog();

        public EditGemotor(Voertuig voertuig, int userID)
        {
            InitializeComponent();
            selectedVoertuig = voertuig;
            userId = userID;
            Fotolist();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {

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
                    }
                }
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

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            if (textChanged && selectionChanged)
            {
                if (string.IsNullOrEmpty(name.Text))
                {
                    MessageBox.Show("Vul een geldig naam in.", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (string.IsNullOrEmpty(beschrijving.Text))
                {
                    MessageBox.Show("Vul een geldig beschrijving in.", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (string.IsNullOrEmpty(bouwjaar.Text))
                {
                    MessageBox.Show("Vul een geldig bouwjaar in.", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                selectedVoertuig.Merk = merk.Text;
                selectedVoertuig.Model = model.Text;

                selectedVoertuig.Naam = name.Text;
                selectedVoertuig.Beschrijving = beschrijving.Text;
                selectedVoertuig.Bouwjaar = (int)Convert.ToInt64(bouwjaar.Text);
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
                selectedVoertuig.UpdateVoertuig();
            }

            Foto foto = new Foto();

            int startIndex = photoList.Count - openFileDialog.FileNames.Length;

            for (int i = startIndex; i < photoList.Count; i++)
            {
                byte[] imageData = photoList[i];
                int newPhotoId = foto.AddPhotos(imageData, selectedVoertuig.Id);
                foto.UpdatePhoto(imageData, newPhotoId);
            }
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
            transmissieComboBox.Text = selectedVoertuig.Transmissie.HasValue ? selectedVoertuig.Transmissie.ToString() : "N/A";
            transmissieComboBox.SelectedItem = selectedVoertuig.Transmissie;

            brandstofComboBox.Text = selectedVoertuig.Brandstof.HasValue ? selectedVoertuig.Brandstof.ToString() : "N/A";
            brandstofComboBox.SelectedItem = selectedVoertuig.Brandstof;

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
    }
}
