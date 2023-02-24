using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;

namespace WpfKerstverlichting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer = new DispatcherTimer();
        private bool lightsON = false;
        private List<Ellipse> lights = new List<Ellipse>();

        public MainWindow()
        {
            // code uit ppt
            InitializeComponent();
            Random rand = new Random();

            // maak het lampje met een ellips (cirkel)
            for (int i = 0; i < 40; i++)
            {
                // maak het lampje met een ellips (cirkel)
                Ellipse newLight = new Ellipse();
                newLight.Width = 10;
                newLight.Height = 10;
                newLight.Fill = Brushes.Gray;
                newLight.Stroke = Brushes.Black;

                double xPos = rand.Next(0, 300);
                double yPos = rand.Next(0, 425);

                // while loop die 40 nieuwe licht na kijkt of het in witte pixel ligt of niet en al het ligt terug een nieuwe random coorinaat creeêren
                while (PixelIsWhite(imgTree, (int)xPos, (int)yPos))
                {
                    xPos = rand.Next(0, 300);
                    yPos = rand.Next(0, 425);
                }

                newLight.SetValue(Canvas.LeftProperty, (double)xPos);
                newLight.SetValue(Canvas.TopProperty, (double)yPos);

                // voeg ellips toe aan het canvas
                cnvTree.Children.Add(newLight);
                
                // voeg newlight is list lights
                lights.Add(newLight);
            }
        }

        public static bool PixelIsWhite(Image img, int x, int y)
        {
            BitmapSource source = img.Source as BitmapSource;
            Color color = Colors.White;
            CroppedBitmap cb = new CroppedBitmap(source, new Int32Rect(x, y, 1, 1));
            byte[] pixels = new byte[4];
            cb.CopyPixels(pixels, 4, 0);
            color = Color.FromRgb(pixels[2], pixels[1], pixels[0]);
            return color.ToString() == "#FFFFFFFF";
        }

        private void SwitchOnOff_Click(object sender, RoutedEventArgs e)
        {
            if (!lightsON)
            {
                lightsON = true;
                btnSwitch.Content = "Switch Off";
                timer.Interval = TimeSpan.FromMilliseconds(500);
                timer.Tick += ColorChange;
                timer.Start();
            }
            else
            {
                lightsON = false;
                btnSwitch.Content = "Switch On";
                timer.Stop();
                foreach (Ellipse light in lights)
                {
                    light.Fill = Brushes.Gray;
                }
            }     
        }

        // randomize de kleueren
        private void ColorChange(object sender, EventArgs e)
        {
            Random rand = new Random();
            foreach (Ellipse light in lights)
            {
                List<Brush> colors = new List<Brush>() {Brushes.White, Brushes.Red, Brushes.Yellow, Brushes.Blue, Brushes.Purple };
                if (rand.NextDouble() >= 0.5)
                {
                    light.Fill = colors[rand.Next(0, colors.Count)];
                }
                else
                {
                    light.Fill = colors[rand.Next(0, colors.Count)];
                }
            }
        }
    }
}
