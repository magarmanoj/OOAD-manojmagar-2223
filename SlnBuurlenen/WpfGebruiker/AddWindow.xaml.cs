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
using System.Windows.Shapes;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        private Gebruiker currentUser;
        public AddWindow(Gebruiker gebruiker)
        {
            InitializeComponent();
            currentUser = gebruiker;
        }

        private void Gemotoriseerd_Click(object sender, RoutedEventArgs e)
        {
            Addgemotoriseerd addWindowGemotor = new Addgemotoriseerd(currentUser);
            addWindowGemotor.Show();
            this.Close();
        }

        private void Getrokken_Click(object sender, RoutedEventArgs e)
        {
            Addgetrokken addWindowGetrokken = new Addgetrokken(currentUser);
            addWindowGetrokken.Show();
            this.Close();
        }
    }
}
