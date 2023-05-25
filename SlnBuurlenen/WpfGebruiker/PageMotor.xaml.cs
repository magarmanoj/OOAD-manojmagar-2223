using MyClassLibrary;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for PageMotor.xaml
    /// </summary>
    public partial class PageMotor : Page
    {
        private Voertuig selectedVoertuig;
        private int userId;
        public PageMotor(Voertuig voertuig, int userID)
        {
            InitializeComponent();
            selectedVoertuig = voertuig;
            userId = userID;
            Fotolist();
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
            Gebruiker naam = Gebruiker.GetGebruikerById(selectedVoertuig.Id);
            name.Text = selectedVoertuig.Naam;
            beschrijving.Text = $"Beschrijving: {selectedVoertuig.Beschrijving}";
            merk.Text = selectedVoertuig.Merk;
            bouwjaar.Text = selectedVoertuig.Bouwjaar.HasValue ? selectedVoertuig.Bouwjaar.Value.ToString() : "N/A";
            model.Text = selectedVoertuig.Model;
            eignaar.Text = naam != null ? $"{naam.Voornaam} {naam.Achternaam}" : "N/A";
            transmissie.Text = selectedVoertuig.Transmissie.HasValue ? selectedVoertuig.Transmissie.ToString() : "N/A";
            brandstof.Text = selectedVoertuig.Brandstof.HasValue ? selectedVoertuig.Brandstof.ToString() : "N/A";
        }

        private void BtnVerzenden_Click(object sender, RoutedEventArgs e)
        {
            if (vanDateP.SelectedDate.HasValue && totDateP.SelectedDate.HasValue)
            {
                if (totDateP.SelectedDate.Value >= vanDateP.SelectedDate.Value)
                {
                    Ontlening nieuweOntlening = new Ontlening
                    {
                        Id = selectedVoertuig.Id,
                        Vanaf = vanDateP.SelectedDate.Value.Date,
                        Tot = totDateP.SelectedDate.Value.Date,
                        Bericht = tbBericht.Text,
                        Status = Enums.OntleningStatus.InAanvraag,
                        Aanvrager = Gebruiker.GetGebruikerById(userId)
                    };

                    Ontlening.AddOntlening(nieuweOntlening);

                    vanDateP.SelectedDate = null;
                    totDateP.SelectedDate = null;
                    tbBericht.Text = string.Empty;
                }
                else
                {
                    MessageBox.Show("De einddatum moet na de begindatum liggen.", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Vul de velden 'Van' en 'Tot' in.", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
