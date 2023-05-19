using MyClassLibrary;
using System.Collections.Generic;
using System.IO;
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
        public PageMotor(Voertuig voertuig)
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
            Gebruiker naam = new Gebruiker();
            naam.Id = selectedVoertuig.EigenaarId;

            Name.Text = selectedVoertuig.Naam;
            Beschrijving.Text = $"Beschrijving: {selectedVoertuig.Beschrijving}";
            Merk.Text = selectedVoertuig.Merk;
            if (selectedVoertuig.Bouwjaar.HasValue)
            {
                Bouwjaar.Text = selectedVoertuig.Bouwjaar.Value.ToString("yyyy");
            }
            else
            {
                Bouwjaar.Text = "N/A";
            }
            Model.Text = selectedVoertuig.Model;
            Eignaar.Text = naam.Voornaam.ToString();
        }
    }
}
