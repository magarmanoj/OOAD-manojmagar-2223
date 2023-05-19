using MyClassLibrary;
using System.Collections.Generic;
using System.IO;
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
        public PageGetrokken(Voertuig voertuig)
        {
            InitializeComponent();
            selectedVoertuig = voertuig;
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
            Name.Text = !string.IsNullOrEmpty(selectedVoertuig.Naam) ? selectedVoertuig.Naam : "n.v.t";
            Beschrijving.Text = $"Beschrijving: {selectedVoertuig.Beschrijving}";
            Merk.Text = !string.IsNullOrEmpty(selectedVoertuig.Merk) ? selectedVoertuig.Merk : "n.v.t";
            Bouwjaar.Text = selectedVoertuig.Bouwjaar.HasValue ? selectedVoertuig.Bouwjaar.Value.ToString() : "N/A";
            Model.Text = !string.IsNullOrEmpty(selectedVoertuig.Model) ? selectedVoertuig.Model : "n.v.t";
            Eignaar.Text = naam != null ? $"{naam.Voornaam} {naam.Achternaam}" : "N/A";
            Geremd.Text = selectedVoertuig.Geremd.HasValue ? (selectedVoertuig.Geremd.Value ? "Ja" : "Nee") : "N/A";
            Gewicht.Text = selectedVoertuig.Gewicht.HasValue ? selectedVoertuig.Gewicht.Value.ToString() + " kg" : "N/A";
            Afmetingen.Text = !string.IsNullOrEmpty(selectedVoertuig.Afmetingen) ? selectedVoertuig.Afmetingen : "n.v.t";
            MaxBelasting.Text = selectedVoertuig.MaxBelasting.HasValue ? selectedVoertuig.MaxBelasting.Value.ToString() + " kg" : "N/A";
        }


    }
}
