using MyClassLibrary;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for PageGetrokken.xaml
    /// </summary>
    public partial class PageGetrokken : Page
    {
        private Voertuig selectedVoertuig;
        private int userId;
        public PageGetrokken(Voertuig voertuig, int userID)
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
            name.Text = !string.IsNullOrEmpty(selectedVoertuig.Naam) ? selectedVoertuig.Naam : "n.v.t";
            beschrijving.Text = $"Beschrijving: {selectedVoertuig.Beschrijving}";
            merk.Text = !string.IsNullOrEmpty(selectedVoertuig.Merk) ? selectedVoertuig.Merk : "n.v.t";
            bouwjaar.Text = selectedVoertuig.Bouwjaar.HasValue ? selectedVoertuig.Bouwjaar.Value.ToString() : "N/A";
            model.Text = !string.IsNullOrEmpty(selectedVoertuig.Model) ? selectedVoertuig.Model : "n.v.t";
            eignaar.Text = naam != null ? $"{naam.Voornaam} {naam.Achternaam}" : "N/A";
            geremd.Text = selectedVoertuig.Geremd.HasValue ? (selectedVoertuig.Geremd.Value ? "Ja" : "Nee") : "N/A";
            gewicht.Text = selectedVoertuig.Gewicht.HasValue ? selectedVoertuig.Gewicht.Value.ToString() + " kg" : "N/A";
            afmetingen.Text = !string.IsNullOrEmpty(selectedVoertuig.Afmetingen) ? selectedVoertuig.Afmetingen : "n.v.t";
            maxBelasting.Text = selectedVoertuig.MaxBelasting.HasValue ? selectedVoertuig.MaxBelasting.Value.ToString() + " kg" : "N/A";
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
