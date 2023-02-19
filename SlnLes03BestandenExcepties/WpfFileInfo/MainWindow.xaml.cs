using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
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
                string fileContent = File.ReadAllText(chosenFileName);
                string[] words = fileContent.Split(new char[] { ' ', '.',','}, StringSplitOptions.RemoveEmptyEntries);
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


                // maakt een loop om string key (dus word) terug te geven 
                // voorbeeld code opgezocht in internet/chatgpt en aangepast om in hier te laten werken
                string wordCountString = " ";
                foreach (KeyValuePair<string, int> pair in wordCounts)
                {
                    wordCountString += $"{pair.Key}, ";
                }

                FileInfo fi = new FileInfo(chosenFileName);
                lblVenster.Content = $"bestandsnaam: {fi.Name}\nextensie: {fi.Extension}\ngemaakt op: {fi.CreationTime}\nmapnaam: {fi.Directory.Name}\naantal woorden: {wordCount}\nmeest voorkomend: {wordCountString}";
            }
        }
    }
}
