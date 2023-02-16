using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WpfMatchImages
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Stopwatch watch = new Stopwatch();
        private DispatcherTimer timer;
        private Button previousButton;
        
        int matchLeft = 8;

        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(2);
            timer.Tick += FormatWatch;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button clicked = (Button)sender;

            if (previousButton != null)
            {
                if ((string)previousButton.Tag == (string)clicked.Tag && previousButton != clicked)
                {                    
                    previousButton.Opacity = 0.5;
                    clicked.Opacity = 0.5;
                    matchLeft--;
                    previousButton.IsEnabled = false;
                    clicked.IsEnabled = false;
                    lblJuistAntw.Content = $"Jusit! nog {matchLeft} te gaan";
                }
                if (matchLeft == 0)
                {
                    lblJuistAntw.Content = $"Alles gevonden!";
                    watch.Stop();
                    timer.Stop();
                }
                previousButton = null;
            }
            else
            {
                previousButton = clicked;
                timer.Start();
                watch.Start();
            }
        }

        private void FormatWatch(object sender, EventArgs e)
        {
            TimeSpan elapsed = watch.Elapsed;
            lblTimer.Content = $"{elapsed.Hours:00}:{elapsed.Minutes:00}:{elapsed.Seconds:00}.{elapsed.Milliseconds / 10:00}";          
        }
    }
}
