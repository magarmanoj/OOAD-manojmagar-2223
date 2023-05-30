using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MyClassLibrary;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for PageVoertuigen.xaml
    /// </summary>
    public partial class PageVoertuigen : Page
    {
        private Gebruiker currentUser;
        List<Voertuig> voertuigList;
        public PageVoertuigen(Gebruiker userID)
        {
            InitializeComponent();
            this.currentUser = userID;
            ShowPhotoAndInfo();
        }

        private void DynamischUI(BitmapImage bitmap, Voertuig voertuig)
        {
            // Create the main grid
            Grid mainGrid = new Grid();
            mainGrid.Background = new SolidColorBrush(Colors.LightCyan);
            mainGrid.Margin = new Thickness(0, 0, 0, 0);

            // Create the row definition
            RowDefinition rowDef = new RowDefinition();
            mainGrid.RowDefinitions.Add(rowDef);

            // Create the column definitions
            ColumnDefinition col1 = new ColumnDefinition();
            ColumnDefinition col2 = new ColumnDefinition();
            ColumnDefinition col3 = new ColumnDefinition();
            col1.Width = new GridLength(1, GridUnitType.Auto);
            col2.Width = new GridLength(1, GridUnitType.Auto);
            col3.Width = new GridLength(1, GridUnitType.Auto);
            mainGrid.ColumnDefinitions.Add(col1);
            mainGrid.ColumnDefinitions.Add(col2);
            mainGrid.ColumnDefinitions.Add(col3);

            // Create the image
            Image img = new Image();
            img.Source = bitmap;
            img.Name = "img";
            img.Width = 80;
            img.Height = 80;
            Grid.SetRow(img, 1);
            Grid.SetColumn(img, 0);

            Border imgBorder = new Border();
            imgBorder.BorderThickness = new Thickness(2);
            imgBorder.Margin = new Thickness(5);
            imgBorder.BorderBrush = Brushes.Black;
            imgBorder.Child = img;
            mainGrid.Children.Add(imgBorder);

            // Create the stack panel for the labels
            StackPanel textBStackPanel = new StackPanel();
            textBStackPanel.Name = "labelStackPanel";
            Grid.SetRow(textBStackPanel, 1);
            Grid.SetColumn(textBStackPanel, 1);
            textBStackPanel.VerticalAlignment = VerticalAlignment.Center;
            mainGrid.Children.Add(textBStackPanel);

            // Create the labels
            TextBlock title = new TextBlock();
            title.Width = 100;
            title.Text = voertuig.Naam;
            title.TextWrapping = TextWrapping.Wrap;
            textBStackPanel.Children.Add(title);

            TextBlock merk = new TextBlock();
            merk.Name = "merk";
            merk.Text = voertuig.Merk;
            textBStackPanel.Children.Add(merk);

            TextBlock model = new TextBlock();
            model.Name = "model";
            model.Text = voertuig.Model;
            textBStackPanel.Children.Add(model);

            // Create the stack panel for the buttons
            StackPanel buttonStackPanel = new StackPanel();
            buttonStackPanel.Name = "buttonStackPanel";
            Grid.SetRow(buttonStackPanel, 1);
            Grid.SetColumn(buttonStackPanel, 2);
            buttonStackPanel.Orientation = Orientation.Horizontal;
            buttonStackPanel.VerticalAlignment = VerticalAlignment.Bottom;
            buttonStackPanel.HorizontalAlignment = HorizontalAlignment.Right;
            mainGrid.Children.Add(buttonStackPanel);

            Button deleteButton = new Button();
            deleteButton.Name = "delete";
            deleteButton.Content = "\uE74D";
            deleteButton.FontFamily = new FontFamily("Segoe MDL2 Assets");
            deleteButton.Width = 25;
            deleteButton.Height = 25;
            deleteButton.Margin = new Thickness(0, 0, 10, 10);
            buttonStackPanel.Children.Add(deleteButton);

            Button editButton = new Button();
            editButton.Name = "edit";
            editButton.Content = "\uE70F";
            editButton.FontFamily = new FontFamily("Segoe MDL2 Assets");
            editButton.Width = 25;
            editButton.Height = 25;
            editButton.Margin = new Thickness(0, 0, 10, 10);
            buttonStackPanel.Children.Add(editButton);

            Button infoButton = new Button();
            infoButton.Name = "info";
            infoButton.Content = "\uE946";
            infoButton.FontFamily = new FontFamily("Segoe MDL2 Assets");
            infoButton.Width = 25;
            infoButton.Height = 25;
            infoButton.Margin = new Thickness(0, 0, 10, 10);
            buttonStackPanel.Children.Add(infoButton);

            Border border = new Border();
            border.BorderThickness = new Thickness(2);
            border.Margin = new Thickness(5);
            border.BorderBrush = Brushes.Black;
            border.Child = mainGrid;
            wrapP.Children.Add(border);

            infoButton.Click += BtnInfo_Click;
            deleteButton.Click += BtnDelete_Click;
            editButton.Click += BtnEdit_Click;
        }

        private void ShowPhotoAndInfo()
        {
            voertuigList = Voertuig.GetAllVoertuigOwnedByUser(currentUser.Id);

            for (int i = 0; i < voertuigList.Count; i++)
            {
                Voertuig voertuig = voertuigList[i];

                Foto foto = Foto.GetFotoByVoertuigId(voertuig.Id);
                if (foto == null)
                {
                    DynamischUI(null, voertuig);
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

                DynamischUI(bitmap, voertuig);
            }
        }

        private void BtnInfo_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            StackPanel buttonStackPanel = (StackPanel)btn.Parent;
            Grid mainGrid = (Grid)buttonStackPanel.Parent;
            Border parentBorder = (Border)mainGrid.Parent;
            int selectedIndex = wrapP.Children.IndexOf(parentBorder);
            voertuigList = Voertuig.GetAllVoertuigOwnedByUser(currentUser.Id);

            if (selectedIndex >= 0 && selectedIndex < voertuigList.Count)
            {
                Voertuig selectedVoertuig = voertuigList[selectedIndex];

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
                detailsWindow.ShowDialog();
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            StackPanel buttonStackPanel = (StackPanel)btn.Parent;
            Grid mainGrid = (Grid)buttonStackPanel.Parent;
            Border parentBorder = (Border)mainGrid.Parent;
            int selectedIndex = wrapP.Children.IndexOf(parentBorder);
            voertuigList = Voertuig.GetAllVoertuigOwnedByUser(currentUser.Id);

            if (selectedIndex >= 0 && selectedIndex < voertuigList.Count)
            {
                Voertuig selectedVoertuig = voertuigList[selectedIndex];
                selectedVoertuig.DeleteVoertuig(selectedVoertuig.Id);
                wrapP.Children.Remove(parentBorder);
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            StackPanel buttonStackPanel = (StackPanel)btn.Parent;
            Grid mainGrid = (Grid)buttonStackPanel.Parent;
            Border parentBorder = (Border)mainGrid.Parent;
            int selectedIndex = wrapP.Children.IndexOf(parentBorder);
            voertuigList = Voertuig.GetAllVoertuigOwnedByUser(currentUser.Id);

            if (selectedIndex >= 0 && selectedIndex < voertuigList.Count)
            {
                Voertuig selectedVoertuig = voertuigList[selectedIndex];

                Window detailsWindow = new Window();
                detailsWindow.Width = 800;
                detailsWindow.Height = 750;

                if (selectedVoertuig.Type == 1)
                {
                    detailsWindow.Content = new EditGemotor(selectedVoertuig, currentUser.Id);
                    detailsWindow.Title = "Motor Details";
                }
                else if (selectedVoertuig.Type == 2)
                {
                    detailsWindow.Content = new EditGetrokken(selectedVoertuig, currentUser.Id);
                    detailsWindow.Title = "Getrokken Details";
                }

                // Show the new window
                detailsWindow.ShowDialog();
            }
        }

        private void Toevoegen_Click(object sender, RoutedEventArgs e)
        {
            new AddWindow(currentUser).Show();
        }
    }
}
