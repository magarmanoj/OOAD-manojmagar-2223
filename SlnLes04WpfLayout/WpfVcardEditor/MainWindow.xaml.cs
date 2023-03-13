using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;
using System.Xml.XPath;
using Microsoft.Win32;

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

            string workEPrefix = "EMAIL;CHARSET=UTF-8;type=WORK,INTERNET:";
            string emailPrefix = "EMAIL;CHARSET=UTF-8;type=HOME,INTERNET:";
            string workTPrefix = "TEL;TYPE=WORK,VOICE:";
            string telefoonPrefix = "TEL;TYPE=HOME,VOICE:";
            string bedrijfPrefix = "ORG;CHARSET=UTF-8:";
            string titlePrefix = "ROLE;CHARSET=UTF-8:";
            string facebookPrefix = "X-SOCIALPROFILE;TYPE=facebook:";
            string linkedinPrefix = "X-SOCIALPROFILE;TYPE=linkedin:";
            string instagramPrefix = "X-SOCIALPROFILE;TYPE=instagram:";
            string youtubePrefix = "X-SOCIALPROFILE;TYPE=youtube:";

            bool? dialogResult = dialog.ShowDialog();
            if (dialogResult == true)
            {
                // user picked a file and pressed OK
                chosenFileName = dialog.FileName;
                save.IsEnabled = true;

                try
                {
                    // dictonary https://www.tutorialsteacher.com/csharp/csharp-dictionary 
                    string[] lines = File.ReadAllLines(chosenFileName);
                    Dictionary<string, TextBox> pair = new Dictionary<string, TextBox>();
                    {
                        // Werk
                        pair.Add(workEPrefix, txtWerkE);
                        pair.Add(workTPrefix, txtWerkT);
                        pair.Add(bedrijfPrefix, txtBedrijf);
                        pair.Add(titlePrefix, txtJobtitel);
                        
                        // Persoonlijk
                        pair.Add(emailPrefix, txtEmail);
                        pair.Add(telefoonPrefix, txtTelefoon);

                        // Sociaal
                        pair.Add(youtubePrefix, txtYoutube);
                        pair.Add(facebookPrefix, txtFacebook);
                        pair.Add(instagramPrefix, txtInsta);
                        pair.Add(linkedinPrefix, txtLinkedin);
                    }

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

                        foreach (KeyValuePair<string, TextBox> ky in pair)
                        {
                            string prefix = ky.Key;
                            TextBox txtBox = ky.Value;

                            if (line.StartsWith(prefix))
                            {
                                string value = line.Substring(prefix.Length);
                                txtBox.Text = value;
                            }
                        }
                        if (line.StartsWith("GENDER"))
                        {
                            Geslacht(line);
                        }
                        else if (line.StartsWith("BDAY"))
                        {
                            string dateString = words[1];

                            // https://learn.microsoft.com/en-us/dotnet/api/system.datetime.parseexact?view=net-7.0#system-datetime-parseexact(system-string-system-string-system-iformatprovider) 
                            DateTime date = DateTime.ParseExact(dateString, "yyyyMMdd", CultureInfo.InvariantCulture);
                            dateBirth.SelectedDate = date;
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
        // Gender 
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

        private void Save_Click(object sender, RoutedEventArgs e)
        {
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
                    sw.WriteLine($"BDAY:{dateBirth.SelectedDate}");
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

        private void New_Click(object sender, RoutedEventArgs e)
        {
            if (txtBoxChanged == true)
            {
                MessageBoxResult result = MessageBox.Show("Weet je zeker dat je de naam wil wijzigen?", "Naam wijzigen", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.Cancel)
                {
                    txtBoxChanged = false;
                }
            }
        }
    }
}
