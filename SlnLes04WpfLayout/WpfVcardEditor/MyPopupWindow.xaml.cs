using System.Windows;

namespace WpfVcardEditor
{
    /// <summary>
    /// Interaction logic for MyPopupWindow.xaml
    /// </summary>
    public partial class MyPopupWindow : Window
    {
        public MyPopupWindow()
        {
            InitializeComponent();
        }

        public void Sluiten_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
