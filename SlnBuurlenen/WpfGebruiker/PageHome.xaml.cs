using MyClassLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static System.Net.WebRequestMethods;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for PageHome.xaml
    /// </summary>
    public partial class PageHome : Page
    {
        public List<Voertuig> VoertuigList { get; set; }
        public PageHome()
        {
            InitializeComponent();
            DataContext = this;

            // Populate the VoertuigList with your data
            ShowPhotoAndInfo();
        }

        private CheckBox previousCheckBox;

        private void ChType_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox currentCheckBox = (CheckBox)sender;
            if (previousCheckBox != null && previousCheckBox != currentCheckBox)
            {
                previousCheckBox.IsChecked = false;
            }
            previousCheckBox = currentCheckBox; 
            currentCheckBox.IsChecked = true;
            ShowPhotoAndInfo();
        }

        private void ChType_Unchecked(object sender, RoutedEventArgs e)
        {
            if (chGemotoriseerd.IsChecked == true) chGemotoriseerd.IsChecked = false;
            if (chGetrokken.IsChecked == true) chGetrokken.IsChecked = false;
            ShowPhotoAndInfo();
        }

        private void ShowPhotoAndInfo()
        {
            lbox.Children.Clear();

            bool isGetrokken = chGetrokken.IsChecked == true;
            if (!isGetrokken && !chGemotoriseerd.IsChecked.Value)
            {
                VoertuigList = Voertuig.GetAllVoertuig();
            }
            else
            {
                VoertuigList = Voertuig.GetGetrokkenOrMotor(isGetrokken);
            }

            for (int i = 0; i < VoertuigList.Count; i++)
            {
                Voertuig voertuig = VoertuigList[i];

                Foto foto = Foto.GetFotoByVoertuigId(voertuig.Id);
                if (foto == null) continue;

                BitmapImage bitmap = new BitmapImage();
                using (MemoryStream stream = new MemoryStream(foto.Data))
                {
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = stream;
                    bitmap.EndInit();
                }

                CreatePanel(bitmap, voertuig);
            }
        }

        private void CreatePanel(BitmapImage bitmap, Voertuig voertuig)
        {
            Grid grid = new Grid();
            grid.Width = 300;
            grid.Height = 150;
            grid.Margin = new Thickness(0, 0, 20, 0);

            // Define the columns in the grid
            ColumnDefinition col1 = new ColumnDefinition();
            ColumnDefinition col2 = new ColumnDefinition();
            ColumnDefinition col3 = new ColumnDefinition();
            grid.ColumnDefinitions.Add(col1);
            grid.ColumnDefinitions.Add(col2);
            grid.ColumnDefinitions.Add(col3);

            // Image in the first column
            Image img = new Image();
            img.Source = bitmap;
            img.Width = 110;
            img.Height = 150;
            img.Margin = new Thickness(0, 0, 10, 0);
            Grid.SetColumn(img, 0);
            grid.Children.Add(img);

            // StackPanel for Naam, Merk, and Model in the second column
            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Vertical;
            Grid.SetColumn(stackPanel, 1);
            grid.Children.Add(stackPanel);

            // Naam
            TextBlock naamTextBlock = new TextBlock();
            naamTextBlock.Text = voertuig.Naam;
            naamTextBlock.FontWeight = FontWeights.Bold;
            naamTextBlock.TextWrapping = TextWrapping.Wrap;
            naamTextBlock.Margin = new Thickness(0, 0, 0, 30);
            stackPanel.Children.Add(naamTextBlock);

            // Merk
            TextBlock merkTextBlock = new TextBlock();
            merkTextBlock.Text = voertuig.Merk;
            stackPanel.Children.Add(merkTextBlock);

            // Model
            TextBlock modelTextBlock = new TextBlock();
            modelTextBlock.Text = voertuig.Model;
            stackPanel.Children.Add(modelTextBlock);

            // Button in the third column, USED CHATGPT om icon in een button te doen
            Button btn = new Button();
            StackPanel btnContent = new StackPanel();
            TextBlock icon = new TextBlock();
            icon.FontFamily = new FontFamily("Segoe MDL2 Assets");
            icon.Text = "\uE946";
            btnContent.Children.Add(icon);
            btn.Content = btnContent;
            btn.Width = 30;
            btn.Height = 30;
            btn.HorizontalAlignment = HorizontalAlignment.Right;
            Grid.SetColumn(btn, 2);
            Grid.SetRow(btn, 0);
            Grid.SetRowSpan(btn, 4);
            grid.Children.Add(btn);

            btn.Click += BtnDetails_Click;

            lbox.Children.Add(grid);
        }

        private void BtnDetails_Click(object sender, RoutedEventArgs e)
        {
            PageDetails page = new PageDetails();

            Window detailsWindow = new Window();
            detailsWindow.Content = page;
            detailsWindow.Width = 800;
            detailsWindow.Height = 600;
            detailsWindow.Title = "Details";

            // Show the new window
            detailsWindow.ShowDialog();
        }
    }
}
