using MyClassLibrary;
using System.Windows;

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
