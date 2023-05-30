using MyClassLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for EditGetrokken.xaml
    /// </summary>
    public partial class EditGetrokken : Page
    {
        private Voertuig selectedVoertuig;
        private int userId;
        private bool textChanged = false;
        private bool selectionChanged = false;

        public EditGetrokken(Voertuig voertuig, int userID)
        {
            InitializeComponent();
            selectedVoertuig = voertuig;
            userId = userID;
            Fotolist();
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
        }

        private void Fotolist()
        {
            List<Foto> fotoList = Foto.GetFotoListByVoertuigId(selectedVoertuig.Id);

            // Update the Source property of the existing Image elements
            for (int i = 0; i < fotoList.Count; i++)
            {
                if (stpFoto.Children[i] is Image photoImage)
                {
                    using (MemoryStream stream = new MemoryStream(fotoList[i].Data))
                    {
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.StreamSource = stream;
                        bitmap.EndInit();
                        photoImage.Source = bitmap;
                    }
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
