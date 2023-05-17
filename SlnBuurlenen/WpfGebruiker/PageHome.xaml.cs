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

        private void RbType_Checked(object sender, RoutedEventArgs e)
        {
            ShowPhotoAndInfo();
        }

        private void RbType_Unchecked(object sender, RoutedEventArgs e)
        {
            ShowPhotoAndInfo();
        }

        private void ShowPhotoAndInfo()
        {
            lbox.Items.Clear();

            bool isGetrokken = rbGetrokken.IsChecked == true;
            if (!isGetrokken && !rbGemotoriseerd.IsChecked.Value)
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
            Image img = new Image();
            img.Source = bitmap;
            img.Width = 100;
            img.Height = 100;
            img.Margin = new Thickness(0, 0, 10, 0);

            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;
            stackPanel.Children.Add(img);
            stackPanel.Children.Add(new TextBlock() { Text = voertuig.Naam, FontWeight = FontWeights.Bold });
            stackPanel.Children.Add(new TextBlock() { Text = voertuig.Merk });
            stackPanel.Children.Add(new TextBlock() { Text = voertuig.Model });

            // Chatgpt om icon te zetten in een button
            Button btn = new Button();
            StackPanel btnContent = new StackPanel();
            TextBlock icon = new TextBlock();
            icon.FontFamily = new FontFamily("Segoe MDL2 Assets"); // Set the font family for Material Icons
            icon.Text = "\uE946"; // Replace with the Google icon code for the desired icon
            btnContent.Children.Add(icon);

            btn.Content = btnContent;
            btn.Width = 30;
            btn.Height = 30;

            Border btnBorder = new Border();
            btnBorder.CornerRadius = new CornerRadius(btn.Width / 2);
            btnBorder.Child = btn;

            stackPanel.Children.Add(btnBorder);
            lbox.Items.Add(stackPanel);
        }
    }
}
