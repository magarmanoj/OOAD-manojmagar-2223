using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WpfVcardEditor
{
    internal class Vcard
    {

        private string _gender;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender
        {
            get 
            { 
                return _gender; 
            }
            set
            {
                if (value == "F" || value == "M" || value == "O")
                {
                    _gender = value;
                }
                else
                {
                    throw new ArgumentException("Invalid gender. Gender must be either 'F', 'M', or 'O'.");
                }
            }
        }
        public string PEmail { get; set; }
        public string PTelefoon { get; set; }
        public BitmapImage Photo { get; set; }

        // Werk
        public string Bedrijf { get; set; }
        public string JobTitel { get; set; }
        public string WEmail { get; set; }
        public string WTelefoon { get; set; }

        // Sociaal
        public string Linkedin { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Youtube { get; set; }
    }
}
