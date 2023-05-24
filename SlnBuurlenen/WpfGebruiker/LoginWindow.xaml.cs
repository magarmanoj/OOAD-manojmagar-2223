using MyClassLibrary;
using System.Windows;
using System.Windows.Media;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Btnlogin_Click(object sender, RoutedEventArgs e)
        {
            Gebruiker gebruiker = Gebruiker.FindByLoginAndPassword(EmailTextBox.Text, PasswordTextBox.Text);
            if (gebruiker == null)
            {
                lblErrormsg.Content = "Combination not found!";
                lblErrormsg.Foreground = Brushes.Red;
                return;                
            }
            MainWindow mainWindow = new MainWindow(gebruiker);
            mainWindow.Show();
            Close();
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            UpdateLoginForm();
        }

        private void UpdateLoginForm()
        {
            btnLogin.IsEnabled = !string.IsNullOrEmpty(EmailTextBox.Text) && !string.IsNullOrEmpty(PasswordTextBox.Text);

            if (EmailTextBox.Text == "/")
            {
                EmailTextBox.Text = "teo@cmb.be";
                PasswordTextBox.Text = "test345";
                btnLogin.Focus();
            }
        }
    }
}
