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
