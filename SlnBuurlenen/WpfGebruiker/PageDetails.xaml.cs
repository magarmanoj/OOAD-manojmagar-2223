using System.Windows;
using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using MyClassLibrary;
using System.Collections.Generic;
using System.IO;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for PageDetails.xaml
    /// </summary>
    public partial class PageDetails : Page
    {
        private int voertuigId;

        public PageDetails(int voertuigId)
        {
            InitializeComponent();
            this.voertuigId = voertuigId;

        }
        private BitmapImage ByteArrayToBitmapImage(byte[] byteArray)
        {
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = stream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                return bitmapImage;
            }
        }

        private void CreateDynamicGrid()
        {
            List<Foto> fotoList = Foto.GetFotoListByVoertuigId(this.voertuigId);

            // Create and add three images to the photos stack panel
            foreach (Foto foto in fotoList)
            {
                Image photoImage = new Image();
                photoImage.Source = ByteArrayToBitmapImage(foto.Data);
                photoImage.Width = 150;
                photoImage.Height = 100;
                photoImage.Margin = new Thickness(0, 0, 0, 5);
                photosStackPanel.Children.Add(photoImage);
            }
        }
    }
}
