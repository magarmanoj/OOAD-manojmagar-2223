using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace WpfCopy
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

        private void BtnKies_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.Filter = "Tekstbestanden|*.TXT;*.TEXT";
            string chosenFileName;
            bool? dialogResult = dialog.ShowDialog();
            if (dialogResult == true)
            {
                // user picked a file and pressed OK
                chosenFileName = dialog.FileName;
                txtBoxvenster.Text = chosenFileName;

                string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string txtContent = "";
                try
                {
                    string filePath = System.IO.Path.Combine(folderPath, chosenFileName);
                    txtContent = File.ReadAllText(filePath);
                }
                catch (FileNotFoundException ex)
                {
                    MessageBox.Show(
                        $"{ex.FileName} kon niet gevonden worden", // boodschap
                        "Oeps!", // titel
                        MessageBoxButton.OK, // buttons
                        MessageBoxImage.Error);
                }
            }
        }

        private void BtnGo_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.Filter = "Tekstbestanden|*.TXT;*.TEXT";
            dialog.FileName = "savedfile.txt";
            if (dialog.ShowDialog() != true) return;

            string txtSource = "";
            try
            {
                txtSource = File.ReadAllText(txtSource);
            }
            catch (IOException)
            {
                lblMsg.Content = "FOUT: kan bronbestand niet lezen";
                return;
            }
            try
            {
                File.WriteAllText(dialog.FileName, txtSource);
            }
            catch (IOException)
            {
                lblMsg.Content = "FOUT: kan doelbestand niet schrijven";
                return;
            }
            txtBoxvenster.Text = "";
            btnGo.IsEnabled = false;              
        }
    }
}
