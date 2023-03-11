using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace WpfVcardEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        bool txtBoxChanged = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {

            // MessageBox("Ben je zeker dat je de applicatie wil afsluiten?, Toepassing sluiten", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            MessageBoxResult result = MessageBox.Show("Ben je zeker dat je de applicatie wil afsluiten?", "Toepassing sluiten", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                Application.Current.Shutdown();
            }
        }

        private void HelpMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // create a new instance of the popUpWindow
            new MyPopupWindow().Show();
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.Filter = "VCFBestand|*.VCF";
            string chosenFileName;
            bool? dialogResult = dialog.ShowDialog();
            if (dialogResult == true)
            {
                // user picked a file and pressed OK
                chosenFileName = dialog.FileName;
                save.IsEnabled = true;

                try
                {
                    string[] lines = File.ReadAllLines(chosenFileName);

                    // string[] lines = txtContent.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string line in lines)
                    {
                        string[] words = line.Split(':', ';');
                        if (line.StartsWith("N"))
                        {
                            string naam = words[3];
                            string achternaam = words[2];
                            txtAchternaam.Text = achternaam;
                            txtName.Text = naam;
                        }
                        if (line.StartsWith("GENDER"))
                        {
                            if (line.Contains("M"))
                            {
                                rbMan.IsChecked = true;
                            }
                            else if (line.Contains("F"))
                            {
                                rbVrouw.IsChecked = true;
                            }
                            else
                            {
                                rbOnbekend.IsChecked = true;
                            }
                        }
                        else if (line.StartsWith("EMAIL") && line.Contains("type=HOME"))
                        {
                            txtEmail.Text = line.Substring(39);
                        }
                        else if (line.StartsWith("EMAIL") && line.Contains("type=WORK"))
                        {
                            txtWerkE.Text = line.Substring(39);
                        }
                        else if (line.StartsWith("BDAY"))
                        {
                            string dateString = words[1];

                            // https://learn.microsoft.com/en-us/dotnet/api/system.datetime.parseexact?view=net-7.0#system-datetime-parseexact(system-string-system-string-system-iformatprovider) 
                            DateTime date = DateTime.ParseExact(dateString, "yyyyMMdd", CultureInfo.InvariantCulture);
                            dateBirth.SelectedDate = date;
                        }
                        else if (line.StartsWith("TEL") && line.Contains("TYPE=HOME"))
                        {
                            txtTelefoon.Text = line.Substring(21);
                        }
                        else if (line.StartsWith("TEL") && line.Contains("TYPE=WORK"))
                        {
                            txtWerkT.Text = line.Substring(21);
                        }
                        else if (line.StartsWith("ROLE"))
                        {
                            txtJobtitel.Text = line.Substring(19);
                        }
                        else if (line.StartsWith("ORG"))
                        {
                            txtBedrijf.Text = line.Substring(18);
                        }
                        else if (line.Contains("TYPE=facebook"))
                        {
                            txtFacebook.Text = line.Substring(30);
                        }
                        else if (line.Contains("TYPE=linkedin"))
                        {
                            txtLinkedin.Text = line.Substring(30);
                        }
                        else if (line.Contains("TYPE=youtube"))
                        {
                            txtYoutube.Text = line.Substring(29);
                        }
                        else if (line.Contains("TYPE=instagram"))
                        {
                            txtInsta.Text = line.Substring(31);
                        }
                    }
                }
                catch (FileNotFoundException ex)
                {
                    MessageBox.Show(
                        $"{ex.FileName} niet gevonden", // boodschap
                        "Oeps!", // titel
                        MessageBoxButton.OK, // buttons
                        MessageBoxImage.Error);
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (txtBoxChanged)
            {
                MessageBoxResult result = MessageBox.Show("Weet je zeker dat je de naam wil wijzigen?", "Naam wijzigen", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.Cancel)
                {
                    txtBoxChanged = false;
                }
            }
            else
            {
                if (txtName.Text != " " && txtAchternaam.Text != "" && txtEmail.Text != "" && txtTelefoon.Text != ""
    && (rbMan.IsEnabled == true || rbVrouw.IsEnabled == true || rbOnbekend.IsEnabled == true) && dateBirth.SelectedDate != null)
                {
                    MessageBox.Show("Bestand is opgeslagen");
                    txtName.Text = "";
                    txtAchternaam.Text = "";
                    txtEmail.Text = "";
                    txtTelefoon.Text = "";
                    rbMan.IsChecked = false;
                    rbVrouw.IsChecked = false;
                    rbOnbekend.IsChecked = false;
                    dateBirth.SelectedDate = null;
                }
            }
        }

        private void Card_Changed(object sender, TextChangedEventArgs e)
        {
            txtBoxChanged = true;
        }
    }
}
