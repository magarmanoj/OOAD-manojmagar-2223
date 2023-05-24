using MyClassLibrary;
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

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for PageOntlening.xaml
    /// </summary>
    public partial class PageOntlening : Page
    {
        private DateTime vanDate;
        private DateTime totDate;
        private string bericht;

        public PageOntlening(DateTime vanDate, DateTime totDate, string bericht)
        {
            InitializeComponent();

            this.vanDate = vanDate;
            this.totDate = totDate;
            this.bericht = bericht;

            // Create a new Ontlening object with the selected date and textbox content
            Ontlening newOntlening = new Ontlening
            {
                Vanaf = vanDate,
                Tot = totDate,
                Bericht = bericht
            };

            // Add the new Ontlening object to the ListBox
            lbOntleend.Items.Add(newOntlening);
        }
    }
}
