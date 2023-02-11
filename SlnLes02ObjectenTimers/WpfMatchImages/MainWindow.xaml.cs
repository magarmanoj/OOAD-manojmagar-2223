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
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += DoSomething;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            TimerStart();

            currentButton = (Button)sender;

            if (previousButton != null)
            {
                if (previousButton == currentButton || currentButton.Opacity == 0.5 || previousButton.Opacity == 0.5)
                {
                    return;
                }

                if (previousButton.Tag != currentButton.Tag)
                {
                    previousButton.Opacity = 0.5;
                    currentButton.Opacity = 0.5;
                    matchLeft--;
                    lblJuistAntw.Content = $"Jusit! nog {matchLeft}";
                }

                previousButton = null;
            }
            else
            {
                previousButton = currentButton;
            }



        }

        private void DoSomething(object sender, EventArgs e)
        {
            TimeSpan elapsed = watch.Elapsed;
            lblTimer.Content = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                elapsed.Hours, elapsed.Minutes, elapsed.Seconds, elapsed.Milliseconds / 10);
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
