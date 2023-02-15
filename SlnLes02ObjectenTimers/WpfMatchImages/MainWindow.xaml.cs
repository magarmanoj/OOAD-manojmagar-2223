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
        private DispatcherTimer timer;
        private Button previousButton;
        private Button currentButton;
        
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
            TimerStart();

            currentButton = (Button)sender;

            if (previousButton != null)
            {
                if ((string)previousButton.Tag == (string)currentButton.Tag)
                {
                    previousButton.Opacity = 0.5;
                    currentButton.Opacity = 0.5;
                    matchLeft--;
                    ButtonEnabled();
                    lblJuistAntw.Content = $"Jusit! nog {matchLeft} te gaan";
                }
                if (matchLeft == 0)
                {
                    lblJuistAntw.Content = $"Alles gevonden!";
                    TimerStop();
                }
                previousButton = null;
            }
            else
            {
                previousButton = currentButton;
            }
        }

        private void ButtonEnabled()
        {
            previousButton.IsEnabled = false;
            currentButton.IsEnabled = false;
        }

        private void FormatWatch(object sender, EventArgs e)
        {
            TimeSpan elapsed = watch.Elapsed;
            lblTimer.Content = $"{elapsed.Hours:00}:{elapsed.Minutes:00}:{elapsed.Seconds:00}.{elapsed.Milliseconds / 10:00}";          
        }

        private void TimerStart()
        {
            watch.Start();
            timer.Start();
        }

        private void TimerStop()
        {
            watch.Stop();
            timer.Stop();
        }
    }
}
