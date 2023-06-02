using MyClassLibrary;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Gebruiker currentUser;

        public MainWindow(Gebruiker gebruiker)
        {
            InitializeComponent();
            currentUser = gebruiker;

            Main.Content = new PageHome(currentUser);
            ConvertImg();
        }
        private void BtnVoertuigen_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new PageVoertuigen(currentUser);
        }
        private void BtnOntleningen_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new PageOntlening(currentUser.Id);
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new PageHome(currentUser);
        }

        private void ConvertImg()
        {
            byte[] imageData = currentUser.Profielfoto;
            if (imageData != null && imageData.Length > 0)
            {
                BitmapImage imageSource = new BitmapImage();
                using (MemoryStream stream = new MemoryStream(imageData))
                {
                    imageSource.BeginInit();
                    imageSource.CacheOption = BitmapCacheOption.OnLoad;
                    imageSource.StreamSource = stream;
                    imageSource.EndInit();
                }
                imgProfiel.Source = imageSource;
            }
        }
    }
}
