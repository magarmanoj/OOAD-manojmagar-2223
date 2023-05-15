using MyClassLibrary;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(Gebruiker gebruiker)
        {
            InitializeComponent();
        }
        private void BtnVoertuigen_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new PageVoertuigen();
        }
        private void BtnOntleningen_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new PageOntlening();
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new PageHome();
        }
    }
}
