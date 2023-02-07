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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Schema;

namespace WpfTaken
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void CheckForm()
        {
            if (Txtbox_Taak.Text == "")
            {
                Fout_taak.Text = "gelieve een taak in te vullen";
                Fout_taak.Foreground = Brushes.Red;
            }
            else
            {
                Fout_taak.Text = "";

            } 
            if(ComboBox_Prio.SelectedIndex == -1)
            {
                Fout_Prioriteit.Text = "gelieve een prioriteit te kiezen";
                Fout_Prioriteit.Foreground = Brushes.Red;
            }
            else
            {
                Fout_Prioriteit.Text = "";
            }
            if(DatePicker_Deadline.SelectedDate == null)
            {
                Fout_deadline.Text = "gelieve een datum te kiezen";
                Fout_deadline.Foreground = Brushes.Red;
            }
            else
            {
                Fout_deadline.Text = "";
            }
            if((RadioB_Adam.IsChecked == false && RadioB_Bilal.IsChecked == false && RadioB_Chelsey.IsChecked == false))
            {
                
                Fout_door.Text = "gelieve een persoon te kiezen";
                Fout_door.Foreground = Brushes.Red;
            }
            else
            {
                Fout_door.Text = "";
            }

        }

        private void Button_Toevoegen_Click(object sender, RoutedEventArgs e)
        {
            CheckForm();
        }

        private void Txtbox_Taak_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckForm();
        }

        private void ComboBox_Prio_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckForm();
        }

        private void RadioB_Adam_Checked(object sender, RoutedEventArgs e)
        {
            CheckForm();
        }

        private void DatePicker_deadline_Picker(object sender, SelectionChangedEventArgs e)
        {
            CheckForm();
        }
    }
}
