using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace WpfFileInfo
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
                string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string txtContent;
                try
                {
                    string filePath = System.IO.Path.Combine(folderPath, chosenFileName);
                    txtContent = File.ReadAllText(filePath);
                }
                catch (FileNotFoundException ex)
                {
                    MessageBox.Show(
                        $"{ex.FileName} niet gevonden", // boodschap
                        "Oeps!", // titel
                        MessageBoxButton.OK, // buttons
                        MessageBoxImage.Error);
                }

                string fileContent = File.ReadAllText(chosenFileName);
                string[] words = fileContent.Split(new char[] { ' ', '.', ',' }, StringSplitOptions.RemoveEmptyEntries);
                int wordCount = words.Length;

                // sortedDictionary is een collectie of key-values pairs (vb apple: 1 apples = key en 1 = values) 
                // uitleg opgezoekt in internet/chatgpt
                SortedDictionary<string, int> wordCounts = new SortedDictionary<string, int>();

                // maakt een loop door alle woorden in de array en telt het aantal van die woorden op 
                foreach (string word in words)
                {
                    if (wordCounts.ContainsKey(word))
                    {
                        wordCounts[word]++;
                    }
                    else
                    {
                        wordCounts[word] = 1;
                    }
                }

                //// van Chatgpt 

                string[] mostCommonWords = new string[3];
                int[] mostCommonCounts = new int[3];
                foreach (KeyValuePair<string, int> pair in wordCounts)
                {
                    int count = pair.Value;
                    if (count > mostCommonCounts[0])
                    {
                        mostCommonCounts[2] = mostCommonCounts[1];
                        mostCommonCounts[1] = mostCommonCounts[0];
                        mostCommonCounts[0] = count;
                        mostCommonWords[2] = mostCommonWords[1];
                        mostCommonWords[1] = mostCommonWords[0];
                        mostCommonWords[0] = pair.Key;
                    }
                    else if (count > mostCommonCounts[1])
                    {
                        mostCommonCounts[2] = mostCommonCounts[1];
                        mostCommonCounts[1] = count;
                        mostCommonWords[2] = mostCommonWords[1];
                        mostCommonWords[1] = pair.Key;
                    }
                    else if (count > mostCommonCounts[2])
                    {
                        mostCommonCounts[2] = count;
                        mostCommonWords[2] = pair.Key;
                    }
                }
                string wordCountString = string.Join(", ", mostCommonWords);

                FileInfo fi = new FileInfo(chosenFileName);
                lblVenster.Content = $"bestandsnaam: {fi.Name}\nextensie: {fi.Extension}\ngemaakt op: {fi.CreationTime}\nmapnaam: {fi.Directory.Name}\naantal woorden: {wordCount}\nmeest voorkomend: {wordCountString}";
            }
        }
    }
}
