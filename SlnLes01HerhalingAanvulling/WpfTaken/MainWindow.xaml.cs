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
        Stack<ListBoxItem> LijstItems = new Stack<ListBoxItem>();
        public MainWindow()
        {
            InitializeComponent();

        }

        private bool CheckForm()
        {
            bool FormCheckLeeg = true;
            if(FormCheckLeeg == true)
            {
                Fout_taak.Text = "";
                Fout_Prioriteit.Text = "";
                Fout_deadline.Text = "";
                Fout_door.Text = "";
            }
            if (Txtbox_Taak.Text == "")
            {
                Fout_taak.Text = "gelieve een taak in te vullen";
                Fout_taak.Foreground = Brushes.Red;
                FormCheckLeeg = false;
            }

            if(ComboBox_Prio.SelectedIndex == -1)
            {
                Fout_Prioriteit.Text = "gelieve een prioriteit te kiezen";
                Fout_Prioriteit.Foreground = Brushes.Red;
                FormCheckLeeg = false;
            }
            if(DatePicker_Deadline.SelectedDate == null)
            {
                Fout_deadline.Text = "gelieve een datum te kiezen";
                Fout_deadline.Foreground = Brushes.Red;
                FormCheckLeeg = false;
            }
            if((RadioB_Adam.IsChecked == false && RadioB_Bilal.IsChecked == false && RadioB_Chelsey.IsChecked == false))
            {
                
                Fout_door.Text = "gelieve een persoon te kiezen";
                Fout_door.Foreground = Brushes.Red;
                FormCheckLeeg = false;
            }
            return FormCheckLeeg;

        }

        private void Button_Toevoegen_Click(object sender, RoutedEventArgs e)
        {
            if (CheckForm())
            {
                ListBoxItem lijsten = new ListBoxItem();
                RadioButton rb = new RadioButton();

                if (RadioB_Adam.IsChecked == true)
                {
                    rb = RadioB_Adam;
                }
                if (RadioB_Bilal.IsChecked == true)
                {
                    rb = RadioB_Bilal;
                }
                if (RadioB_Chelsey.IsChecked == true)
                {
                    rb = RadioB_Chelsey;
                }
                lijsten.Content = $"{Txtbox_Taak.Text} (deadline: {DatePicker_Deadline.SelectedDate.Value.ToShortDateString()}; door: {rb.Content})";
                Lijst_box.Items.Add(lijsten);
                if(ComboBox_Prio.SelectedIndex == 0)
                {
                    lijsten.Background = Brushes.Red;
                }else if (ComboBox_Prio.SelectedIndex == 1)
                {
                    lijsten.Background = Brushes.Green;
                }
                else
                {
                    lijsten.Background = Brushes.Yellow;
                }

                if(lijsten.Content != null)
                {
                    Button_Verwijderen.IsEnabled = true;
                }

            }
            Txtbox_Taak.Text = "";
            ComboBox_Prio.SelectedIndex = -1;
            DatePicker_Deadline.SelectedDate = null;
            RadioB_Adam.IsChecked = false;
            RadioB_Bilal.IsChecked = false;
            RadioB_Chelsey.IsChecked = false;
            CheckForm();
        }

        // Deze event handler zorgt voor live form checking.
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

        private void Button_terugzetten_Click(object sender, RoutedEventArgs e)
        {
            //als stack lijst meer dan 0 waarde heeft dan ga je die waarde terug poping in een oorspronkelijke lijst.
            if (LijstItems.Count > 0)
            {
                ListBoxItem item = LijstItems.Pop();
                Lijst_box.Items.Add(item);
                           
            }           
        }

        private void Button_Verwijderen_Click(object sender, RoutedEventArgs e)
        {
            //Nieuwe lijst maken die de content van oude lijst 'kopieert' en daarna push je die waarde in stack lijst
            //en verwijderen van oorspronkelijke lijst.
            ListBoxItem selectedItem = Lijst_box.SelectedItem as ListBoxItem;
            if (selectedItem != null)
            {
                LijstItems.Push(selectedItem);
                Lijst_box.Items.Remove(selectedItem);
            }
        }

        private void Lijst_box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Button_Verwijderen.IsEnabled = Lijst_box.SelectedItem != null;
            Button_terugzetten.IsEnabled = Button_Verwijderen.IsEnabled != true;

        }
    }
}
