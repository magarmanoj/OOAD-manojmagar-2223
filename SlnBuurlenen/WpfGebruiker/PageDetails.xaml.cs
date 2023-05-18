using System.Windows;
using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using MyClassLibrary;
using System.Collections.Generic;
using System.IO;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for PageDetails.xaml
    /// </summary>
    public partial class PageDetails : Page
    {
        private int voertuigId;

        public PageDetails(int voertuigId)
        {
            InitializeComponent();
            this.voertuigId = voertuigId;
            CreateDynamicGrid();
        }
        private BitmapImage ByteArrayToBitmapImage(byte[] byteArray)
        {
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = stream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                return bitmapImage;
            }
        }

        private void CreateDynamicGrid()
        {
            Grid grid = new Grid();
            grid.Width = 850;
            grid.Height = 800;

            // Define row definitions
            for (int i = 0; i < 7; i++)
            {
                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = GridLength.Auto;
                grid.RowDefinitions.Add(rowDefinition);
            }

            // Title
            TextBlock titleTextBlock = new TextBlock();
            titleTextBlock.Text = "Liche vrachtwagen"; 
            titleTextBlock.FontSize = 24;
            titleTextBlock.FontWeight = FontWeights.Bold;
            titleTextBlock.Margin = new Thickness(10, 10, 0, 5);
            Grid.SetRow(titleTextBlock, 0);
            grid.Children.Add(titleTextBlock);

            // Three Photos
            StackPanel photosStackPanel = new StackPanel();
            photosStackPanel.Orientation = Orientation.Horizontal;
            photosStackPanel.Margin = new Thickness(0, 0, 0, 10);
            Grid.SetRow(photosStackPanel, 1);
            grid.Children.Add(photosStackPanel);

            List<Foto> fotoList = Foto.GetFotoListByVoertuigId(this.voertuigId);

            // Create and add three images to the photos stack panel
            foreach (Foto foto in fotoList)
            {
                Image photoImage = new Image();
                photoImage.Source = ByteArrayToBitmapImage(foto.Data);
                photoImage.Width = 150;
                photoImage.Height = 100;
                photoImage.Margin = new Thickness(0, 0, 0, 5);
                photosStackPanel.Children.Add(photoImage);
            }
            
            StackPanel beschrijvingStackPanel = new StackPanel();
            beschrijvingStackPanel.Margin = new Thickness(10, 10, 0, 5);
            Grid.SetRow(beschrijvingStackPanel, 2);
            grid.Children.Add(beschrijvingStackPanel);

            // Create and add the text blocks for Beschrijving
            AddTextBlockToStackPanel(beschrijvingStackPanel, "Beschrijving:", new Thickness(0, 0, 0, 10));

            // First nested StackPanel
            StackPanel firstNestedStackPanel = new StackPanel();
            firstNestedStackPanel.Orientation = Orientation.Horizontal;
            beschrijvingStackPanel.Children.Add(firstNestedStackPanel);

            AddTextBlockToStackPanel(firstNestedStackPanel, "Merk:");
            AddTextBlockToStackPanel(firstNestedStackPanel, "Your Brand Name", new Thickness(0, 0, 10, 10));
            AddTextBlockToStackPanel(firstNestedStackPanel, "Bouwjouw:");
            AddTextBlockToStackPanel(firstNestedStackPanel, "Construction Details", new Thickness(0, 0, 10, 10));

            // Second nested StackPanel
            StackPanel secondNestedStackPanel = new StackPanel();
            secondNestedStackPanel.Orientation = Orientation.Horizontal;
            beschrijvingStackPanel.Children.Add(secondNestedStackPanel);

            AddTextBlockToStackPanel(secondNestedStackPanel, "Model:");
            AddTextBlockToStackPanel(secondNestedStackPanel, "Model Name", new Thickness(0, 0, 10, 10));
            AddTextBlockToStackPanel(secondNestedStackPanel, "Transmissie:");
            AddTextBlockToStackPanel(secondNestedStackPanel, "Transmission Type", new Thickness(0, 0, 10, 10));

            // Third nested StackPanel
            StackPanel thirdNestedStackPanel = new StackPanel();
            thirdNestedStackPanel.Orientation = Orientation.Horizontal;
            beschrijvingStackPanel.Children.Add(thirdNestedStackPanel);

            AddTextBlockToStackPanel(thirdNestedStackPanel, "Brandstof:");
            AddTextBlockToStackPanel(thirdNestedStackPanel, "Fuel Type", new Thickness(0, 0, 10, 10));
            AddTextBlockToStackPanel(thirdNestedStackPanel, "Eignaar:");
            AddTextBlockToStackPanel(thirdNestedStackPanel, "Owner", new Thickness(0, 0, 10, 10));

            // Dit voertuig lenen
            TextBlock lenenTextBlock = new TextBlock();
            lenenTextBlock.Text = "Dit voertuig lenen?";
            lenenTextBlock.FontSize = 16;
            lenenTextBlock.FontWeight = FontWeights.Bold;
            lenenTextBlock.Margin = new Thickness(10, 10, 0, 5) ;
            Grid.SetRow(lenenTextBlock, 3);
            grid.Children.Add(lenenTextBlock);

            // Date Picker
            StackPanel datePickerStackPanel = new StackPanel();
            datePickerStackPanel.Margin = new Thickness(10);
            datePickerStackPanel.Orientation = Orientation.Horizontal;
            Grid.SetRow(datePickerStackPanel, 4);
            grid.Children.Add(datePickerStackPanel);

            TextBlock vanTextBlock = new TextBlock();
            vanTextBlock.Text = "van:";
            vanTextBlock.FontWeight = FontWeights.Bold;
            vanTextBlock.VerticalAlignment = VerticalAlignment.Center;
            datePickerStackPanel.Children.Add(vanTextBlock);

            DatePicker fromDatePicker = new DatePicker();
            fromDatePicker.Width = 150;
            datePickerStackPanel.Children.Add(fromDatePicker);

            TextBlock totTextBlock = new TextBlock();
            totTextBlock.Text = "tot:";
            totTextBlock.FontWeight = FontWeights.Bold;
            totTextBlock.VerticalAlignment = VerticalAlignment.Center;
            totTextBlock.Margin = new Thickness(10, 0, 0, 0);
            datePickerStackPanel.Children.Add(totTextBlock);

            DatePicker toDatePicker = new DatePicker();
            toDatePicker.Width = 150;
            datePickerStackPanel.Children.Add(toDatePicker);

            // Bericht
            StackPanel berichtStackPanel = new StackPanel();
            berichtStackPanel.Orientation = Orientation.Vertical;
            berichtStackPanel.Margin = new Thickness(10 ,0, 0, 10);
            Grid.SetRow(berichtStackPanel, 5);
            grid.Children.Add(berichtStackPanel);

            AddTextBlockToStackPanel(berichtStackPanel, "Bericht aan eigenaar:", new Thickness(0, 10, 0, 5));

            TextBox berichtTextBox = new TextBox();
            berichtTextBox.Height = 150;
            berichtTextBox.Width = 356;
            berichtTextBox.Margin = new Thickness(0, 10, 0, 5);
            berichtTextBox.HorizontalAlignment = HorizontalAlignment.Left;
            berichtStackPanel.Children.Add(berichtTextBox);

            // Verzenden Button
            Button verzendenButton = new Button();
            verzendenButton.Content = "Bevestigen";
            verzendenButton.Width = 60;
            verzendenButton.Height = 30;
            Grid.SetRow(verzendenButton, 5);
            verzendenButton.VerticalAlignment = VerticalAlignment.Top;
            verzendenButton.Margin = new Thickness(375, 160, 375, 0);
            grid.Children.Add(verzendenButton);

            // Add the dynamic grid to the page
            lbox.Children.Add(grid);
        }

        private void AddTextBlockToStackPanel(StackPanel stackPanel, string text, Thickness margin)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.Margin = margin;
            stackPanel.Children.Add(textBlock);
        }

        private void AddTextBlockToStackPanel(StackPanel stackPanel, string text)
        {
            AddTextBlockToStackPanel(stackPanel, text, new Thickness(0, 0, 0, 0));
        }
    }
}
