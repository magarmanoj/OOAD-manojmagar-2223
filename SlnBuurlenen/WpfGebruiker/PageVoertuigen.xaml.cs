using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MyClassLibrary;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for PageVoertuigen.xaml
    /// </summary>
    public partial class PageVoertuigen : Page
    {
        public PageVoertuigen()
        {
            InitializeComponent();
            DynamischUI();
        }

        private void DynamischUI()
        {
            // Create the main grid
            Grid mainGrid = new Grid();
            mainGrid.Background = new SolidColorBrush(Colors.LightBlue);
            mainGrid.Margin = new Thickness(0, 0, 0, 0);

            // Create the row definition
            RowDefinition rowDef = new RowDefinition();
            mainGrid.RowDefinitions.Add(rowDef);

            // Create the column definitions
            ColumnDefinition col1 = new ColumnDefinition();
            ColumnDefinition col2 = new ColumnDefinition();
            ColumnDefinition col3 = new ColumnDefinition();
            col1.Width = new GridLength(1, GridUnitType.Star);
            col2.Width = new GridLength(1, GridUnitType.Auto);
            col3.Width = new GridLength(1, GridUnitType.Auto);
            mainGrid.ColumnDefinitions.Add(col1);
            mainGrid.ColumnDefinitions.Add(col2);
            mainGrid.ColumnDefinitions.Add(col3);

            // Create the image
            Image img = new Image();
            img.Name = "img";
            img.Width = 180;
            img.Height = 150;
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
            title.Name = "title";
            title.Text = "Buick Oldtimer";
            textBStackPanel.Children.Add(title);

            TextBlock merk = new TextBlock();
            merk.Name = "merk";
            merk.Text = "Merk:";
            textBStackPanel.Children.Add(merk);

            TextBlock model = new TextBlock();
            model.Name = "model";
            model.Text = "Model:";
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
            deleteButton.Width = 50;
            deleteButton.Height = 25;
            buttonStackPanel.Children.Add(deleteButton);

            Button editButton = new Button();
            editButton.Name = "edit";
            editButton.Content = "\uE70F";
            editButton.FontFamily = new FontFamily("Segoe MDL2 Assets");
            editButton.Width = 50;
            editButton.Height = 25;
            buttonStackPanel.Children.Add(editButton);

            Button infoButton = new Button();
            infoButton.Name = "info";
            infoButton.Content = "\uE946";
            infoButton.FontFamily = new FontFamily("Segoe MDL2 Assets");
            infoButton.Width = 50;
            infoButton.Height = 25;
            buttonStackPanel.Children.Add(infoButton);

            Border border = new Border();
            border.BorderThickness = new Thickness(2);
            border.Margin = new Thickness(5);
            border.BorderBrush = Brushes.Black;
            border.Child = mainGrid;


            // Add the main grid to the page's content
            wrapP.Children.Add(border);
        }
    }
}
