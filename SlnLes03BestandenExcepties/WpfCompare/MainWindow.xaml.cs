using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfCompare
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
            LoadFiles(lb1);
            LoadFiles(lb2);
        }

        private void LoadFiles(ListBox lijst)
        {
            lijst.Items.Clear();
            string startfolder = Path.Combine(folderPath, "text");
            string[] files = Directory.GetFiles(startfolder, "*.txt");
            foreach (string filePath in files)
            {
                string fileName = Path.GetFileName(filePath);
                lijst.Items.Add(fileName);
            }
        }

        private void Items_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            if (listBox == lb1 && lb1.SelectedItem != null)
            {
                lbMsg1.Items.Clear();
                string startfolder1 = System.IO.Path.Combine(folderPath, "text", lb1.SelectedItem.ToString());
                string[] fileLines = File.ReadAllLines(startfolder1);
                foreach (string line in fileLines)
                {
                    lbMsg1.Items.Add(line);
                }
            }
            else if (listBox == lb2 && lb2.SelectedItem != null)
            {
                lbMsg2.Items.Clear();
                string startfolder2 = System.IO.Path.Combine(folderPath, "text", lb2.SelectedItem.ToString());
                string[] fileLines = File.ReadAllLines(startfolder2);
                foreach (string line in fileLines)
                {
                    lbMsg2.Items.Add(line);
                }
            }
        }

        private void BtnCompare_Click(object sender, RoutedEventArgs e)
        {
            // // loopt door al de items (woorden) van lbmsg1 en lbMsg2 en vergelijkt dezelfde items met elkaar om te zien of de value er van zelfde is 
            //    // dus (vb woord gelijk - gelik) je ziet dat item = gelijk maar hun value is anders gelijk = 6 en gelik = 5 dus fout= rode kleur
            for (int i = 0; i < lbMsg2.Items.Count; i++)
            {
                if (i < lbMsg1.Items.Count && lbMsg1.Items[i].ToString() != lbMsg2.Items[i].ToString())
                {
                    // If the items don't match, set the background color to red
                    // https://stackoverflow.com/questions/22091772/how-to-change-background-color-of-selected-item-in-a-listbox-programmatically 
                    // zorgt voor het veranderen van backgound kleur, != item wordt in listBoxItem item toegevoegd en kleuren ervan wordt veranderd
                    ListBoxItem item = lbMsg2.ItemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;
                    if (item != null)
                    {
                        item.Background = Brushes.Red;
                    }
                }
                else if (i >= lbMsg1.Items.Count)
                {
                    // If the index is greater than or equal to the number of items in lbMsg1,
                    // set the background color to red for all the remaining items in lbMsg2
                    ListBoxItem item = lbMsg2.ItemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;
                    if (item != null)
                    {
                        item.Background = Brushes.Red;
                    }
                }
            }
        }
    }
}
