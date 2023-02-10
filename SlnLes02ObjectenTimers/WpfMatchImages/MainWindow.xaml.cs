using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfMatchImages
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Stopwatch watch = new Stopwatch();
        private int ticks;
        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            ticks = 0;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += DoSomething;
            timer.Start();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            watch.Start();
            Button clickedButton = (Button)sender;

            while (clickedButton != null)
            {
                if (clickedButton.Tag == hiveB.Tag)
                {
                    beeIm.Opacity = 0.5;
                    hiveIm.Opacity = 0.5;
                }
                if (clickedButton.Tag == dogB.Tag)
                {
                    leashIm.Opacity = 0.5;
                    dogIm.Opacity = 0.5;
                }
                if (clickedButton.Tag == catB.Tag)
                {
                    catIm.Opacity = 0.5;
                    postB.Opacity = 0.5;
                }
                if (clickedButton.Tag == penguinB.Tag)
                {
                    penguinIm.Opacity = 0.5;
                    snowIm.Opacity = 0.5;
                }
                if (clickedButton.Tag == owlB.Tag)
                {
                    owlIm.Opacity = 0.5;
                    nightIm.Opacity = 0.5;
                }
                if (clickedButton.Tag == cowB.Tag)
                {
                    cowIm.Opacity = 0.5;
                    milkIm.Opacity = 0.5;
                }
                if (clickedButton.Tag == chickenB.Tag)
                {
                    chickenIm.Opacity = 0.5;
                    eegIm.Opacity = 0.5;
                }
                if (clickedButton.Tag == birdB.Tag)
                {
                    birdIm.Opacity = 0.5;
                    houseIm.Opacity = 0.5;
                }

            }



            lblTimer.Content = watch;
        }
        private void DoSomething(object sender, EventArgs e)
        {
            // elke 200 milliseconden uitgevoerd
            ticks++;
            // lblTimer.Content = ticks;
            
        }
    }
}
