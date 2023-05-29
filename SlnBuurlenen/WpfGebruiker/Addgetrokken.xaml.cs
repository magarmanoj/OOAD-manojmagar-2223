using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for Addgetrokken.xaml
    /// </summary>
    public partial class Addgetrokken : Window
    {
        List<string> photoList = new List<string>();

        public Addgetrokken()
        {
            InitializeComponent();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.Filter = "Image Files (*.jpg, *.png, *.jpeg)|*.jpg;*.png;*.jpeg";
            openFileDialog.Multiselect = true;
            bool? dialogResult = openFileDialog.ShowDialog();
            if (dialogResult == true)
            {
                foreach (string filePath in openFileDialog.FileNames)
                {
                    if (photoList.Count < 3)
                    {
                        // Add each selected photo to the list
                        photoList.Add(filePath);

                        // Display the photos in the Image elements
                        for (int i = 0; i < photoList.Count; i++)
                        {
                            if (i < wrapPanel.Children.Count && wrapPanel.Children[i] is Image image)
                            {
                                BitmapImage bitmap = new BitmapImage(new Uri(photoList[i]));
                                image.Source = bitmap;
                            }
                            else
                            {
                                // Create a new Image control and add it to the WrapPanel
                                BitmapImage bitmap = new BitmapImage(new Uri(photoList[i]));
                                Image newImage = new Image();
                                newImage.Width = 220;
                                newImage.Height = 220;
                                newImage.Source = bitmap;
                                wrapPanel.Children.Add(newImage);

                                // Create a new Button control and add it to the WrapPanel
                                Button newButton = new Button();
                                newButton.Name = "btnVerwijder" + (i + 1);
                                newButton.Content = "X";
                                newButton.HorizontalAlignment = HorizontalAlignment.Right;
                                newButton.VerticalAlignment = VerticalAlignment.Top;
                                newButton.Background = Brushes.Transparent;
                                newButton.Click += VerwijderAfbeelding_Click;
                                wrapPanel.Children.Add(newButton);
                            }
                        }
                    }
                }
            }
        }

        private void VerwijderAfbeelding_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            int index = wrapPanel.Children.IndexOf(clickedButton);

            // Check if the clicked button is associated with an image
            if (index >= 1 && wrapPanel.Children[index - 1] is Image image)
            {
                // Remove the image and the button from the WrapPanel
                wrapPanel.Children.RemoveAt(index); // Remove button
                wrapPanel.Children.RemoveAt(index - 1); // Remove image

                // Remove the corresponding photo from the photoList
                int photoIndex = (index - 1) / 2; // Calculate the index in photoList based on the button index
                if (photoIndex >= 0 && photoIndex < photoList.Count)
                {
                    photoList.RemoveAt(photoIndex);
                }
            }
        }

    }
}
