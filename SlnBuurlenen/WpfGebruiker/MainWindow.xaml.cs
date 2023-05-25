using MyClassLibrary;
using System;
using System.Windows;

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
        }
        private void BtnVoertuigen_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new PageVoertuigen();
        }
        private void BtnOntleningen_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new PageOntlening(currentUser.Id);
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new PageHome(currentUser);
        }
    }
}
