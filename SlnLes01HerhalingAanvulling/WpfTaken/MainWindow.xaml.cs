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
        Stack<ListBoxItem> lijstItems = new Stack<ListBoxItem>();
        List<SolidColorBrush> color = new List<SolidColorBrush>() { Brushes.Red, Brushes.Green, Brushes.Yellow};
        bool toevoegenIsclicked = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private bool CheckForm()
        {
            fout_Melding.Foreground = Brushes.Red;
            fout_Melding.Text = "";
            bool formCheckLeeg = true;
            if (toevoegenIsclicked)
            {
                if (txtbox_Taak.Text == "")
                {
                    fout_Melding.Text += "gelieve een taak in te vullen\n";
                    formCheckLeeg = false;
                }

                if (comboBox_Prio.SelectedIndex == -1)
                {
                    fout_Melding.Text += "gelieve een prioriteit te kiezen\n";
                    formCheckLeeg = false;
                }
                if (datePicker_Deadline.SelectedDate == null)
                {
                    fout_Melding.Text += "gelieve een datum te kiezen\n";
                    formCheckLeeg = false;
                }
                if (radioB_Adam.IsChecked == false && radioB_Bilal.IsChecked == false && radioB_Chelsey.IsChecked == false)
                {
                    fout_Melding.Text += "gelieve een persoon te kiezen\n";
                    formCheckLeeg = false;
                }
            }
            return formCheckLeeg;
        }


        private void Button_Toevoegen_Click(object sender, RoutedEventArgs e)
        {
            toevoegenIsclicked = true;
            if (CheckForm())
            {
                ListBoxItem lijsten = new ListBoxItem();
                RadioButton rbutton = new RadioButton();

                if (radioB_Adam.IsChecked == true)
                {
                    rbutton = radioB_Adam;
                }
                if (radioB_Bilal.IsChecked == true)
                {
                    rbutton = radioB_Bilal;
                }
                if (radioB_Chelsey.IsChecked == true)
                {
                    rbutton = radioB_Chelsey;
                }
                lijsten.Content = $"{txtbox_Taak.Text} (deadline: {datePicker_Deadline.SelectedDate.Value.ToShortDateString()}; door: {rbutton.Content})";
                lijst_box.Items.Add(lijsten);
                lijsten.Background = color[comboBox_Prio.SelectedIndex];
                
                txtbox_Taak.Text = "";
                comboBox_Prio.SelectedIndex = -1;
                datePicker_Deadline.SelectedDate = null;
                rbutton.IsChecked = false;
                CheckForm();
                fout_Melding.Text = "";
                toevoegenIsclicked = false;               
            }
        }

        // Deze event handler zorgt voor live form checking. Met (EventArgs e) geef je voor alle andere eventhadle zelfde taak
        private void SelectionChanged(object sender, EventArgs e)
        {
            CheckForm();
        }

        private void Button_terugzetten_Click(object sender, RoutedEventArgs e)
        {
            // als stack lijst meer dan 0 waarde heeft dan ga je die waarde terug poping in een oorspronkelijke lijst.
            if (lijstItems.Count > 0)
            {
                lijst_box.Items.Add(lijstItems.Pop());
            }
            if (lijstItems.Count == 0)
            {
                button_terugzetten.IsEnabled = false;
            }
        }

        private void Button_Verwijderen_Click(object sender, RoutedEventArgs e)
        {
            // Nieuwe lijst maken die de content van oude lijst 'kopieert' en daarna push je die waarde in stack lijst
            // en verwijderen van oorspronkelijke lijst.
            ListBoxItem selectedItem = lijst_box.SelectedItem as ListBoxItem;
            if (selectedItem != null)
            {
                lijstItems.Push(selectedItem);
                lijst_box.Items.Remove(selectedItem);
            }
        }

        private void Lijst_box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            button_Verwijderen.IsEnabled = lijst_box.SelectedItem != null;
            button_terugzetten.IsEnabled = button_Verwijderen.IsEnabled != true;
        }
    }
}
