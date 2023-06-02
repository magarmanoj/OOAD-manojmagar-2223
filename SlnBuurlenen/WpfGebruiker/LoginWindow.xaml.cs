using MyClassLibrary;
using System;
using System.Data.SqlClient;
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
            try
            {
                Gebruiker gebruiker = Gebruiker.FindByLoginAndPassword(EmailTextBox.Text, PasswordTextBox.Password);
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
            catch (SqlException ex)
            {
                lblErrormsg.Content = "An SQL error  during login: " + ex.Message;
                lblErrormsg.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {
                lblErrormsg.Content = "An error  during login: " + ex.Message;
                lblErrormsg.Foreground = Brushes.Red;
            }
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            UpdateLoginForm();
        }

        private void UpdateLoginForm()
        {
            btnLogin.IsEnabled = !string.IsNullOrEmpty(EmailTextBox.Text) && !string.IsNullOrEmpty(PasswordTextBox.Password);
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
             UpdateLoginForm();
        }
    }
}
