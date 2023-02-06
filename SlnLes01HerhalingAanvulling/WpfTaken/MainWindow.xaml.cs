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
            if(Txtbox_Taak.Text == "" )
            {
                Fout_taak.Text = "schijf iets";
            }
            else
            {
                Fout_taak.Text = "";
            }
            if (ComboBox_Prio.SelectedIndex == -1 )
            {
                Fout_Prioriteit.Text = "Selecteer iets";
            }
            else
            {
                Fout_Prioriteit.Text = "";
            }
            if (DatePicker_Deadline.SelectedDate == null)
            {
                Fout_deadline.Text = "Selecteer datum";
            }
            else
            {
                Fout_deadline.Text = "";
            }
            if (RadioB_Adam.IsChecked == false || RadioB_Bilal.IsChecked == false || RadioB_Chelsey.IsChecked == false)
            {
                Fout_door.Text = "Selecteer iets";
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
    }
}
