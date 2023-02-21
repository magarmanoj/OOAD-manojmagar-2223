using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
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
            //string dinfo = new DirectoryInfo(@"C:\Users\magar\OneDrive\Desktop\New folder\New folder\");
            EersteFile();
            TweedeFile();

        }

        private void EersteFile()
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string startfolder = System.IO.Path.Combine(folderPath, "text1");

            string[] files = Directory.GetFiles(startfolder, "*.txt");
            foreach (string filePath in files)
            {
                string fileName = Path.GetFileName(filePath);
                lb1.Items.Add(fileName);

            }
            lb1.SelectionChanged += Items_SelectionChanged;
        }

        private void TweedeFile()
        {

            string folderPath1 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string startfolder1 = System.IO.Path.Combine(folderPath1, "text2");

            string[] files1 = Directory.GetFiles(startfolder1, "*.txt");
            foreach (string filePath in files1)
            {
                string fileName = Path.GetFileName(filePath);
                lb2.Items.Add(fileName);
            }
            lb2.SelectionChanged += Items_SelectionChanged;

        }


        private void Items_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            if (listBox == lb1 && lb1.SelectedItem != null)
            {
                string folderPath1 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string startfolder1 = System.IO.Path.Combine(folderPath1, "text1", lb1.SelectedItem.ToString());
                string fileContent = File.ReadAllText(startfolder1);
                lbl1.Content = fileContent;
            }
            else if (listBox == lb2 && lb2.SelectedItem != null)
            {
                string folderPath2 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string startfolder2 = System.IO.Path.Combine(folderPath2, "text2", lb2.SelectedItem.ToString());
                string fileContent = File.ReadAllText(startfolder2);
                lbl2.Content = fileContent;
            }
        }

        private void BtnCompare_Click(object sender, RoutedEventArgs e)
        {
            if (lb1.SelectedItem != null && lb2.SelectedItem != null)
            {
                string folderPath1 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string startfolder1 = System.IO.Path.Combine(folderPath1, "text1", lb1.SelectedItem.ToString());
                string startfolder2 = System.IO.Path.Combine(folderPath1, "text2", lb2.SelectedItem.ToString());

                string fileContent1 = File.ReadAllText(startfolder1);
                string fileContent2 = File.ReadAllText(startfolder2);

                StringBuilder diffContent = new StringBuilder();
                int length = Math.Min(fileContent1.Length, fileContent2.Length);

                for (int i = 0; i < length; i++)
                {
                    if (fileContent1[i] != fileContent2[i])
                    {
                        diffContent.Append("<span style='color:red'>" + fileContent1[i] + "</span>");
                    }
                    else
                    {
                        diffContent.Append(fileContent1[i]);
                    }
                }

                if (fileContent1.Length > length)
                {
                    diffContent.Append("<span style='color:red'>" + fileContent1.Substring(length) + "</span>");
                }
                else if (fileContent2.Length > length)
                {
                    diffContent.Append("<span style='color:red'>" + fileContent2.Substring(length) + "</span>");
                }

                lbl1.Content = diffContent.ToString();
            }
        }

    }
}
