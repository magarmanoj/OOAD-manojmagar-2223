using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.TextFormatting;
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
        string chosenFileName = null;
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
            bool? dialogResult = dialog.ShowDialog();
            if (dialogResult == true)
            {
                // user picked a file and pressed OK
                chosenFileName = dialog.FileName;
                save.IsEnabled = true;

                try
                {
                    // probeer dictonary 
                    string[] lines = File.ReadAllLines(chosenFileName);

                    // string[] lines = txtContent.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string line in lines)
                    {
                        string[] words = line.Split(':', ';');
                        if (line.StartsWith("N;"))
                        {
                            string naam = words[3];
                            string achternaam = words[2];
                            txtAchternaam.Text = achternaam;
                            txtName.Text = naam;
                        }

                        // can be in methode
                        else if (line.StartsWith("GENDER"))
                        {
                            Geslacht(line);
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
                            txtTelefoon.Text = line.Substring(20);
                        }
                        Work(line);
                        Social(line);          
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

        private void Geslacht(string line)
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

        private void Social(string line)
        {
            if (line.Contains("TYPE=facebook"))
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

        private void Work(string line)
        {
            if (line.StartsWith("TEL") && line.Contains("TYPE=WORK"))
            {
                txtWerkT.Text = line.Substring(20);
            }
            else if (line.StartsWith("ROLE"))
            {
                txtJobtitel.Text = line.Substring(19);
            }
            else if (line.StartsWith("ORG"))
            {
                txtBedrijf.Text = line.Substring(18);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (txtBoxChanged == true)
            {
                MessageBoxResult result = MessageBox.Show("Weet je zeker dat je de naam wil wijzigen?", "Naam wijzigen", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.Cancel)
                {
                    txtBoxChanged = false;
                }
            }
            using (StreamWriter sw = new StreamWriter(chosenFileName))
            {
                if (txtName.Text != " " && txtAchternaam.Text != "" && txtEmail.Text != "" && txtTelefoon.Text != ""
&& (rbMan.IsEnabled == true || rbVrouw.IsEnabled == true || rbOnbekend.IsEnabled == true) && dateBirth.SelectedDate != null && txtEmail.Text != "" && txtTelefoon.Text != "")
                {                  
                    sw.WriteLine($"BEGIN:VCARD");
                    sw.WriteLine($"VERSION:3.0");
                    sw.WriteLine($"FN;CHARSET=UTF-8:{txtName.Text} {txtAchternaam.Text}");
                    sw.WriteLine($"N;CHARSET=UTF-8:{txtAchternaam.Text};{txtName.Text};;;");
                    sw.WriteLine($"NICKNAME;CHARSET=UTF-8:{txtName.Text}");
                    sw.WriteLine("GENDER:M");
                    sw.WriteLine("GENDER:F");
                    sw.WriteLine("GENDER:O");
                    sw.WriteLine($"BDAY:{dateBirth}");
                    sw.WriteLine($"EMAIL;CHARSET=UTF-8;type=HOME,INTERNET:{txtEmail.Text}");
                    sw.WriteLine($"TEL;TYPE=HOME,VOICE:{txtTelefoon.Text}");
                    sw.WriteLine("END:VCARD"); 
                    MessageBox.Show("Bestand is opgeslagen");
                }
            }
        }

        private void Card_Changed(object sender, RoutedEventArgs e)
        {
            txtBoxChanged = true;
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.Filter = "VCFBestand|*.VCF";
            dialog.FileName = "Save a vcf file";
            if (dialog.ShowDialog() != true) return;
            string txtSource;
            txtSource = File.ReadAllText(chosenFileName);
            File.WriteAllText(dialog.FileName, txtSource);
        }
    }
}
