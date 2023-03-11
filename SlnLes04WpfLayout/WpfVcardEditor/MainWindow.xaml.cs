using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;

namespace WpfVcardEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

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

                try
                {
                    string[] lines = File.ReadAllLines(chosenFileName);
                    //string[] lines = txtContent.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string line in lines)
                    {
                        if (line.StartsWith("N"))
                        {
                            string[] words = line.Split(':', ';');
                            string naam = words[3];
                            string achternaam = words[2];
                            txtAchternaam.Text = achternaam;
                            txtName.Text = naam;
                        } 
                        else if (line.StartsWith("EMAIL"))
                        {
                            txtEmail.Text = line.Substring(39);
                        }
                        else if (line.StartsWith("BDAY"))
                        {
                            dateBirth.SelectedDate = line.Substring(1);
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
    }
}
