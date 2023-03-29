using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
        int totaalField = 13;
        int totaalIngevuld = 0;
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
                Vcard card = ReadVcardFromFile(dialog.FileName);
                ShowVcard(card);
                save.IsEnabled = true;               
                double totaalPercentage = (double)totaalIngevuld / totaalField * 100;
                PercentageLevel(totaalPercentage);
                chosenFileName = dialog.FileName;
                HuidigeMap(chosenFileName);
            }
        }

        private Vcard ReadVcardFromFile(string filePath)
        {
            Vcard vcard = new Vcard();
            Dictionary<string, string> pair = new Dictionary<string, string>()
                {
                    { "EMAIL;CHARSET=UTF-8;type=WORK,INTERNET:", vcard.WEmail },
                    { "EMAIL;CHARSET=UTF-8;type=HOME,INTERNET:", vcard.PEmail },
                    { "TEL;TYPE=WORK,VOICE:", vcard.WTelefoon },
                    { "TEL;TYPE=HOME,VOICE:", vcard.PTelefoon },
                    { "ROLE;CHARSET=UTF-8:", vcard.JobTitel },
                    { "ORG;CHARSET=UTF-8:", vcard.Bedrijf },
                    { "X-SOCIALPROFILE;TYPE=facebook:", vcard.Facebook },
                    { "X-SOCIALPROFILE;TYPE=linkedin:", vcard.Linkedin },
                    { "X-SOCIALPROFILE;TYPE=instagram:", vcard.Instagram },
                    { "X-SOCIALPROFILE;TYPE=youtube:", vcard.Youtube }
                };
            string[] lines = { "" };

            try
            {
                // dictonary https://www.tutorialsteacher.com/csharp/csharp-dictionary 
                lines = File.ReadAllLines(filePath);
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
                return vcard;
            }
            catch (Exception)
            {
                MessageBox.Show($"Ongekende fouten");
                return vcard;
            }

            foreach (string line in lines)
            {
                string[] words = line.Split(':', ';');
                if (line.StartsWith("N;"))
                {
                    string naam = words[3];
                    string achternaam = words[2];
                    vcard.LastName = achternaam;
                    vcard.FirstName = naam;
                    if (!string.IsNullOrEmpty(naam) && !string.IsNullOrEmpty(achternaam))
                    {
                        totaalIngevuld += 1;
                    }
                }

                foreach (KeyValuePair<string, string> ky in pair)
                {
                    string prefix = ky.Key;
                    string txtBox = ky.Value;

                    if (line.StartsWith(prefix))
                    {
                        string value = line.Substring(prefix.Length);
                        if (!string.IsNullOrEmpty(value))
                        {
                            txtBox = value;
                            totaalIngevuld += 1;
                        }
                        switch (prefix)
                        {
                            case "EMAIL;CHARSET=UTF-8;type=WORK,INTERNET:":
                                vcard.WEmail = value;
                                break;
                            case "EMAIL;CHARSET=UTF-8;type=HOME,INTERNET:":
                                vcard.PEmail = value;
                                break;
                            case "TEL;TYPE=WORK,VOICE:":
                                vcard.WTelefoon = value;
                                break;
                            case "TEL;TYPE=HOME,VOICE:":
                                vcard.PTelefoon = value;
                                break;
                            case "ROLE;CHARSET=UTF-8:":
                                vcard.JobTitel = value;
                                break;
                            case "ORG;CHARSET=UTF-8:":
                                vcard.Bedrijf = value;
                                break;
                            case "X-SOCIALPROFILE;TYPE=facebook:":
                                vcard.Facebook = value;
                                break;
                            case "X-SOCIALPROFILE;TYPE=linkedin:":
                                vcard.Linkedin = value;
                                break;
                            case "X-SOCIALPROFILE;TYPE=instagram:":
                                vcard.Instagram = value;
                                break;
                            case "X-SOCIALPROFILE;TYPE=youtube:":
                                vcard.Youtube = value;
                                break;
                        }
                    }
                }
                if (line.StartsWith("GENDER"))
                {
                    Geslacht(line);
                    totaalIngevuld += 1;
                }
                else if (line.StartsWith("BDAY"))
                {
                    string dateString = words[1];

                    // https://learn.microsoft.com/en-us/dotnet/api/system.datetime.parseexact?view=net-7.0#system-datetime-parseexact(system-string-system-string-system-iformatprovider) 
                    DateTime date = DateTime.ParseExact(dateString, "yyyyMMdd", CultureInfo.InvariantCulture);
                    vcard.BirthDate = date;
                    totaalIngevuld += 1;
                }
                else if (line.StartsWith("PHOTO;"))
                {
                    string photo = line.Replace("PHOTO;ENCODING=b;TYPE=JPEG:", "");
                    byte[] bytes = Convert.FromBase64String(photo);
                    BitmapImage image = new BitmapImage();
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        ms.Position = 0;
                        image.BeginInit();
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.StreamSource = ms;
                        image.EndInit();
                    }
                    vcard.Photo = image;
                    totaalIngevuld += 1;
                }         
            }
            return vcard;
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

        private void ShowVcard(Vcard vcard)
        {
            txtName.Text = vcard.FirstName;
            txtAchternaam.Text = vcard.LastName;
            txtWerkE.Text = vcard.WEmail;
            txtEmail.Text = vcard.PEmail;
            txtWerkT.Text = vcard.WTelefoon;
            txtTelefoon.Text = vcard.PTelefoon;
            txtJobtitel.Text = vcard.JobTitel;
            txtBedrijf.Text = vcard.Bedrijf;
            txtFacebook.Text = vcard.Facebook;
            txtLinkedin.Text = vcard.Linkedin;
            txtInsta.Text = vcard.Instagram;
            txtYoutube.Text = vcard.Youtube;
            dateBirth.SelectedDate = vcard.BirthDate;
            imgFoto.Source = vcard.Photo;
            switch (vcard.Gender)
            {
                case "F":
                    rbVrouw.IsChecked = true;
                    break;
                case "M":
                    rbMan.IsChecked = true;
                    break;
                case "O":
                    rbOnbekend.IsChecked = true;
                    break;
            }
        }

        private Vcard ToVcard()
        {
            Vcard card = new Vcard();
            card.FirstName = txtName.Text;
            card.LastName = txtAchternaam.Text;
            card.PEmail = txtEmail.Text;
            card.WEmail = txtWerkE.Text;
            card.PTelefoon = txtTelefoon.Text;
            card.WTelefoon = txtWerkT.Text;
            card.JobTitel = txtJobtitel.Text;
            card.Bedrijf = txtBedrijf.Text;
            card.Facebook = txtFacebook.Text;
            card.Linkedin = txtFacebook.Text;
            card.Instagram = txtFacebook.Text;
            card.Youtube = txtFacebook.Text;
            card.BirthDate = dateBirth.SelectedDate.Value;

            BitmapImage bitmap = (BitmapImage)imgFoto.Source;
            MemoryStream memoryStream = new MemoryStream();
            BitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            encoder.Save(memoryStream);
            byte[] bytes = memoryStream.ToArray();
            string base64String = Convert.ToBase64String(bytes);
            card.Photo = (BitmapSource)new ImageSourceConverter().ConvertFrom(Convert.FromBase64String(base64String));
            if (rbMan.IsChecked == true)
            {
                card.Gender = "M";
            }
            else if (rbVrouw.IsChecked == true)
            {
                card.Gender = "F";
            }
            else if (rbOnbekend.IsChecked == true)
            {
                card.Gender = "O";
            }
            return card;
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            List<string> words = new List<string>();

            try
            {
                Vcard card = ToVcard();
                File.WriteAllText(chosenFileName, card.GenerateVcardCode());
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
            imgFoto.Source = null;
            lblMessage.Content = "(geen geselecteerd)";
            huidigeMap.Content = "huidige kaart: (geen geopend)";
        }

        private void Selecteer(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files (*.jpg;*.jpeg;*.png;*.gif)|*.jpg;*.jpeg;*.png;*.gif"; ;
            if (dialog.ShowDialog() == true)
            {
                BitmapImage bitmap = new BitmapImage(new System.Uri(dialog.FileName));
                imgFoto.Source = bitmap;
                lblMessage.Content = dialog.FileName;
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
