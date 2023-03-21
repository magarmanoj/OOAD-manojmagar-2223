using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection.Emit;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using Path = System.IO.Path;

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
            int totaalField = 13;
            int totaalLvl = 0;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.Filter = "VCFBestand|*.VCF";

            bool? dialogResult = dialog.ShowDialog();
            if (dialogResult == true)
            {
                // user picked a file and pressed OK
                chosenFileName = dialog.FileName;
                string[] lines = { "" };

                try
                {
                    // dictonary https://www.tutorialsteacher.com/csharp/csharp-dictionary 
                    lines = File.ReadAllLines(chosenFileName);
                }
                catch (FileNotFoundException ex)
                {
                    MessageBox.Show(
                        $"{ex.FileName} niet gevonden", // boodschap
                        "Oeps!", // titel
                        MessageBoxButton.OK, // buttons
                        MessageBoxImage.Error);
                }
                catch (IOException ex)
                {
                    MessageBox.Show($"Fout: Kan doelbestand niet schrijven!{ex.Message}", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                catch (Exception)
                {
                    MessageBox.Show($"Ongekende fouten");
                    return;
                }
                Dictionary<string, TextBox> pair = new Dictionary<string, TextBox>()
                {
                    { "EMAIL;CHARSET=UTF-8;type=WORK,INTERNET:", txtWerkE },
                    { "EMAIL;CHARSET=UTF-8;type=HOME,INTERNET:", txtEmail },
                    { "TEL;TYPE=WORK,VOICE:", txtWerkT },
                    { "TEL;TYPE=HOME,VOICE:", txtTelefoon },
                    { "ROLE;CHARSET=UTF-8:", txtJobtitel },
                    { "ORG;CHARSET=UTF-8:", txtBedrijf },
                    { "X-SOCIALPROFILE;TYPE=facebook:", txtFacebook },
                    { "X-SOCIALPROFILE;TYPE=linkedin:", txtLinkedin },
                    { "X-SOCIALPROFILE;TYPE=instagram:", txtInsta },
                    { "X-SOCIALPROFILE;TYPE=youtube:", txtYoutube }
                };

                foreach (string line in lines)
                {
                    string[] words = line.Split(':', ';');
                    if (line.StartsWith("N;"))
                    {
                        string naam = words[3];
                        string achternaam = words[2];
                        txtAchternaam.Text = achternaam;
                        txtName.Text = naam;
                        if (!string.IsNullOrEmpty(naam) && !string.IsNullOrEmpty(achternaam))
                        {
                            totaalLvl += 1;
                        }
                    }

                    foreach (KeyValuePair<string, TextBox> ky in pair)
                    {
                        string prefix = ky.Key;
                        TextBox txtBox = ky.Value;

                        if (line.StartsWith(prefix))
                        {
                            string value = line.Substring(prefix.Length);
                            txtBox.Text = value;
                            if (!string.IsNullOrEmpty(value))
                            {
                                totaalLvl += 1;
                            }
                        }
                    }
                    if (line.StartsWith("GENDER"))
                    {
                        Geslacht(line);
                        totaalLvl += 1;
                    }
                    else if (line.StartsWith("BDAY"))
                    {
                        string dateString = words[1];

                        // https://learn.microsoft.com/en-us/dotnet/api/system.datetime.parseexact?view=net-7.0#system-datetime-parseexact(system-string-system-string-system-iformatprovider) 
                        DateTime date = DateTime.ParseExact(dateString, "yyyyMMdd", CultureInfo.InvariantCulture);
                        dateBirth.SelectedDate = date;
                        totaalLvl += 1;
                    }
                }
                save.IsEnabled = true;
                HuidigeMap(chosenFileName);
                double totaalPercentage = (double)totaalLvl / totaalField * 100;
                PercentageLevel(totaalPercentage);
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
            List<string> words = new List<string>();
            words.Add("BEGIN:VCARD");
            words.Add("VERSION:3.0");
            words.Add($"FN;CHARSET=UTF-8:{txtName.Text} {txtAchternaam.Text}");
            words.Add($"N;CHARSET=UTF-8:{txtAchternaam.Text};{txtName.Text};;;;");
            words.Add($"EMAIL;CHARSET=UTF-8;type=HOME,INTERNET:{txtEmail.Text}");
            words.Add($"EMAIL;CHARSET=UTF-8;type=WORK,INTERNET:{txtWerkE.Text}");
            words.Add($"TEL;TYPE=HOME,VOICE:{txtTelefoon.Text}");
            words.Add($"TEL;TYPE=WORK,VOICE:{txtWerkT.Text}");
            words.Add($"ROLE;CHARSET=UTF-8:{txtJobtitel.Text}");
            words.Add($"ORG;CHARSET=UTF-8:{txtBedrijf.Text}");
            words.Add($"X-SOCIALPROFILE;TYPE=facebook:{txtFacebook.Text}");
            words.Add($"X-SOCIALPROFILE;TYPE=linkedin:{txtLinkedin.Text}");
            words.Add($"X-SOCIALPROFILE;TYPE=instagram:{txtInsta.Text}");
            words.Add($"X-SOCIALPROFILE;TYPE=youtube:{txtYoutube.Text}");
            DateTime birthDate;
            string date = null;
            if (dateBirth.SelectedDate != null)
            {
                date = dateBirth.SelectedDate.Value.ToString("yyyyMMdd");
            }
            if (!string.IsNullOrEmpty(date) && DateTime.TryParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out birthDate))
            {
                words.Add($"BDAY:{birthDate:yyyyMMdd}");
            }
            if (rbMan.IsChecked == true)
            {
                words.Add("GENDER:M");
            }
            else if (rbVrouw.IsChecked == true)
            {
                words.Add("GENDER:F");
            }
            else if (rbOnbekend.IsChecked == true)
            {
                words.Add("GENDER:O");
            }
            words.Add("END:VCARD");

            try
            {
                File.WriteAllLines(chosenFileName, words);
            }
            catch (PathTooLongException)
            {
                MessageBox.Show($"Bestandsnaam {chosenFileName} te lang");
                return;
            }
            catch (IOException ex)
            {
                MessageBox.Show($"Fout: Kan doelbestand niet schrijven!{ex.Message}", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (Exception)
            {
                MessageBox.Show($"Ongekende fouten");
                return;
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
            string[] txtSource;
            try
            {
                txtSource = File.ReadAllLines(chosenFileName);
                File.WriteAllLines(dialog.FileName, txtSource);
            }
            catch (PathTooLongException)
            {
                MessageBox.Show($"Bestandsnaam {chosenFileName} te lang");
                return;
            }
            catch (IOException ex)
            {
                MessageBox.Show($"Fout: Kan doelbestand niet schrijven!{ex.Message}", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (Exception)
            {
                MessageBox.Show($"Ongekende fouten");
                return;
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
                Save_Click(sender, e);
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

        private void ShowImageName(BitmapImage img)
        {
            if (img != null && img.UriSource != null)
            {
                string imageName = Path.GetFileName(img.UriSource.LocalPath);
                lblFoto.Content = imageName;
            }
        }

        private void Selecteer(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image files (*.jpg;*.jpeg;*.png;*.gif)|*.jpg;*.jpeg;*.png;*.gif"; ;
            if (dlg.ShowDialog() == true)
            {
                BitmapImage bitmap = new BitmapImage(new System.Uri(dlg.FileName));
                imgFoto.Source = bitmap;
                string fileName = System.IO.Path.GetFileName(dlg.FileName);
                lblFoto.Content = fileName;
            }
        }
        private void HuidigeMap(string mapName)
        {
            huidigeMap.Content = "Huidige kaart: " + mapName;
        }

        private void PercentageLevel(double percentFilled)
        {
            percentage.Content = "Ingevuld: " + percentFilled.ToString("0.00") + "%";
        }
    }
}
