using System.Windows;

namespace WpfVcardEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Ben je zeker dat je de applicatie wil afsluiten", "Toepassing sluiten", MessageBoxButton.OKCancel);
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

        // private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        // {
        //    OpenFileDialog dialog = new OpenFileDialog();
        //    dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        //    dialog.Filter = "VCFBestand|*.VCF";
        //    string chosenFileName;
        //    bool? dialogResult = dialog.ShowDialog();
        //    if (dialogResult == true)
        //    {
        //        // user picked a file and pressed OK
        //        chosenFileName = dialog.FileName;
        //        txtAchternaam.Text = chosenFileName;

        // try
        //        {
        //            string filePath = System.IO.Path.Combine(folderPath, chosenFileName);
        //            string txtContent = File.ReadAllText(filePath);
        //        }
        //        catch (FileNotFoundException ex)
        //        {
        //            MessageBox.Show(
        //                $"{ex.FileName} niet gevonden", // boodschap
        //                "Oeps!", // titel
        //                MessageBoxButton.OK, // buttons
        //                MessageBoxImage.Error);
        //        }

        // // Hier moet ook try catch komen
        //        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        //        string startfolder = System.IO.Path.Combine(folderPath, "vcf");
        //        string[] fileLines = File.ReadAllLines(startfolder);
        //        foreach (string line in fileLines)
        //        {
        //            if (line == lblachternaam.ToString())
        //            {
        //                txtAchternaam.Text = line;
        //            }

        // }
        //    }
        //  }
    }
}
