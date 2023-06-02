using MyClassLibrary;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for PageHome.xaml
    /// </summary>
    public partial class PageHome : Page
    {
        public List<Voertuig> VoertuigList { get; set; }
        private Gebruiker currentUser;

        public PageHome(Gebruiker userId)
        {
            InitializeComponent();
            this.currentUser = Gebruiker.GetGebruikerById(userId.Id);
            DataContext = this;
            ShowPhotoAndInfo();
        }

        private void ChType_Checked(object sender, RoutedEventArgs e)
        {
            ShowPhotoAndInfo();
        }

        private void ChType_Unchecked(object sender, RoutedEventArgs e)
        {
            ShowPhotoAndInfo();
        }

        private void ShowPhotoAndInfo()
        {
            lbox.Children.Clear();
            bool isGetrokken = chGetrokken?.IsChecked == true;
            bool isGemotoriseerd = chGemotoriseerd?.IsChecked == true;

            if ((!isGetrokken && !isGemotoriseerd) || (isGetrokken && isGemotoriseerd))
            {
                VoertuigList = Voertuig.GetAllVoertuigNotOwnedByUser(currentUser.Id);
            }
            else
            {
                VoertuigList = Voertuig.GetGetrokkenOrMotor(isGetrokken, currentUser.Id);
            }

            for (int i = 0; i < VoertuigList.Count; i++)
            {
                Voertuig voertuig = VoertuigList[i];

                Foto foto = Foto.GetFotoByVoertuigId(voertuig.Id);
                if (foto == null)
                {
                    CreatePanel(null, voertuig);
                    continue;
                }

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
            img.Width = 150;
            img.Height = 150;
            Grid.SetColumn(img, 0);

            Border imgBorder = new Border();
            imgBorder.BorderThickness = new Thickness(2);
            imgBorder.Margin = new Thickness(5);
            imgBorder.BorderBrush = Brushes.Black;
            imgBorder.Child = img;
            grid.Children.Add(imgBorder);

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
            merkTextBlock.Text = $"Merk: {voertuig.Merk}";
            stackPanel.Children.Add(merkTextBlock);

            // Model
            TextBlock modelTextBlock = new TextBlock();
            modelTextBlock.Text = $"Model: {voertuig.Model}";
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

            Border border = new Border();
            border.BorderThickness = new Thickness(2);
            border.Margin = new Thickness(5);
            border.BorderBrush = Brushes.Black;
            border.Child = grid;

            btn.Click += BtnDetails_Click;

            lbox.Children.Add(border);
        }

        private void BtnDetails_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Grid parentGrid = (Grid)btn.Parent;
            Border parentBorder = (Border)parentGrid.Parent;
            int selectedIndex = lbox.Children.IndexOf(parentBorder);

            if (selectedIndex >= 0 && selectedIndex < VoertuigList.Count)
            {
                Voertuig selectedVoertuig = VoertuigList[selectedIndex];

                Window detailsWindow = new Window();
                detailsWindow.Width = 800;
                detailsWindow.Height = 750;

                if (selectedVoertuig.Type == 1)
                {
                    detailsWindow.Content = new PageMotor(selectedVoertuig, currentUser.Id);
                    detailsWindow.Title = "Motor Details";
                }
                else if (selectedVoertuig.Type == 2)
                {
                    detailsWindow.Content = new PageGetrokken(selectedVoertuig, currentUser.Id);
                    detailsWindow.Title = "Getrokken Details";
                }

                // Show the new window
                detailsWindow.ShowDialog();
            }
        }
    }
}
