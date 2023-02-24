using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;

namespace WpfCompare
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadFiles(lb1);
            LoadFiles(lb2);
        }

        private void LoadFiles(ListBox lijst)
        {
            lijst.Items.Clear();
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string startfolder = Path.Combine(folderPath, "text");
            if (Directory.Exists(startfolder))
            {
                string[] files = Directory.GetFiles(startfolder, "*.txt");
                foreach (string filePath in files)
                {
                    string fileName = Path.GetFileName(filePath);
                    lijst.Items.Add(fileName);
                }
            }
        }

        private void Items_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             ListBox listBox = (ListBox)sender;
             if (listBox == lb1 && lb1.SelectedItem != null)
            {
                lbMsg1.Items.Clear();
                string folderPath1 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string startfolder1 = System.IO.Path.Combine(folderPath1, "text", lb1.SelectedItem.ToString());
                string[] fileLines = File.ReadAllLines(startfolder1);
                foreach (string line in fileLines)
                {
                    lbMsg1.Items.Add(line);
                }
            }
            else if (listBox == lb2 && lb2.SelectedItem != null)
            {
                lbMsg2.Items.Clear();
                string folderPath2 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string startfolder2 = System.IO.Path.Combine(folderPath2, "text", lb2.SelectedItem.ToString());
                string[] fileLines = File.ReadAllLines(startfolder2);
                foreach (string line in fileLines)
                {
                    lbMsg2.Items.Add(line);
                }
            }
        }

        private void BtnCompare_Click(object sender, RoutedEventArgs e)
        {
            // loopt door al de items (woorden) van lbmsg1 en lbMsg2 en vergelijkt dezelfde items met elkaar om te zien of de value er van zelfde is 
            // dus (vb woord gelijk - gelik) je ziet dat item = gelijk maar hun value is anders gelijk = 6 en gelik = 5 dus fout= rode kleur
            for (int i = 0; i < lbMsg1.Items.Count; i++)
            {
                if (i < lbMsg2.Items.Count && lbMsg1.Items[i].ToString() != lbMsg2.Items[i].ToString())
                {
                    // juist regels aan duiden leukt maar niet om kleuren aan te passen dus heb via chatgpt volgende code gevonden
                    ListBoxItem listBoxItem = lbMsg2.ItemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;
                    if (listBoxItem != null)
                    {
                        listBoxItem.Background = Brushes.Red;
                    }
                }
            }
        }
    }
}
