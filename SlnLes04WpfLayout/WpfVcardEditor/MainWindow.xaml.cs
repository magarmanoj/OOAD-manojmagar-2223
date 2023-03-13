using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace WpfVcardEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

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
            string titlePrefix = "TITLE;CHARSET=UTF-8:";
            string facebookPrefix = "X-SOCIALPROFILE;TYPE=facebook:";
            string linkedinPrefix = "X-SOCIALPROFILE;TYPE=linkedin:";
            string instagramPrefix = "X-SOCIALPROFILE;TYPE=instagram:";
            string youtubePrefix = "X-SOCIALPROFILE;TYPE=youtube:";

            bool? dialogResult = dialog.ShowDialog();
            if (dialogResult == true)
            {
                // user picked a file and pressed OK
                chosenFileName = dialog.FileName;
                

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
                save.IsEnabled = true;
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
            Dictionary<string, string> dict = new Dictionary<string, string>();
            {
                dict.Add("FN;CHARSET=UTF-8:", $"{txtName.Text} {txtAchternaam.Text}");
                dict.Add("N;CHARSET=UTF-8:", $"{txtAchternaam.Text};{txtName.Text};;;;");
                dict.Add("EMAIL;CHARSET=UTF-8;type=HOME,INTERNET:", txtEmail.Text);
                dict.Add("EMAIL;CHARSET=UTF-8;type=WORK,INTERNET:", txtWerkE.Text);

                dict.Add("TEL;TYPE=HOME,VOICE:", txtTelefoon.Text);
                dict.Add("TEL;TYPE=WORK,VOICE:", txtWerkT.Text);

                dict.Add("ROLE;CHARSET=UTF-8:", txtJobtitel.Text);
                dict.Add("ORG;CHARSET=UTF-8:", txtBedrijf.Text);

                dict.Add("X-SOCIALPROFILE;TYPE=facebook:", txtFacebook.Text);
                dict.Add("X-SOCIALPROFILE;TYPE=linkedin:", txtLinkedin.Text);
                dict.Add("X-SOCIALPROFILE;TYPE=instagram:", txtInsta.Text);
                dict.Add("X-SOCIALPROFILE;TYPE=youtube:", txtYoutube.Text);
            }

            DateTime birthDate;
            string date = null;
            if (dateBirth.SelectedDate != null)
            {
                date = dateBirth.SelectedDate.Value.ToString("yyyyMMdd");
            }
            if (!string.IsNullOrEmpty(date) && DateTime.TryParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out birthDate))
            {
                dict.Add("BDAY:", $"{birthDate:yyyyMMdd}");
            }
            if (rbMan.IsChecked == true)
            {
                dict.Add("GENDER:", "M");
            }
            else if (rbVrouw.IsChecked == true)
            {
                dict.Add("GENDER:", "F");
            }
            else if (rbOnbekend.IsChecked == true)
            {
                dict.Add("GENDER:", "O");
            }

            try
            {
                using (StreamWriter sw = new StreamWriter(chosenFileName))
                {
                    sw.WriteLine("BEGIN:VCARD");
                    sw.WriteLine("VERSION:3.0");
                    foreach (KeyValuePair<string, string> change in dict)
                    {
                        if (!string.IsNullOrEmpty(change.Key))
                        {
                            sw.WriteLine($"{change.Key}{change.Value}");
                        }
                    }
                    sw.WriteLine("END:VCARD");
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show($"Fout: Kan doelbestand niet schrijven!{ex.Message}", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
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
            dialog.Filter = "vCard files (*.vcf)|*.vcf";
            if (dialog.ShowDialog() != true) return;
            string[] txtSource = null;          
            try
            { 
                txtSource = File.ReadAllLines(chosenFileName);             
                File.WriteAllLines(dialog.FileName, txtSource);
            }
            catch (IOException ex)
            {
                MessageBox.Show($"Fout: Kan doelbestand niet schrijven!{ex.Message}","Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            if (txtBoxChanged == true)
            {
                MessageBoxResult result = MessageBox.Show("Er zijn onopgeslagen wijzigingen. Wilt u het opslagen en door gaan?", "Waarschuwing", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
                Save_Click(sender,e);
            }
            txtName.Text = "";
            txtAchternaam.Text = "";
            dateBirth.SelectedDate = null;
            rbMan.IsChecked = rbVrouw.IsChecked = rbOnbekend.IsChecked = false;
            txtEmail.Text = "";
            txtTelefoon.Text = "";
            txtWerkT.Text = "";
            txtBedrijf.Text = "";
            txtJobtitel.Text = "";
            txtWerkE.Text = "";
            txtFacebook.Text = "";
            txtInsta.Text = "";
            txtLinkedin.Text = "";
            txtYoutube.Text = "";
            txtBoxChanged = false;
        }
    }
}
