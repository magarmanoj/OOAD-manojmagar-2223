using System;
using System.IO;
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
        public BitmapSource Photo { get; set; }

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

        public string GenerateVcardCode()
        {
            string content = "BEGIN:VCARD" + Environment.NewLine;
            content += "VERSION:3.0" + Environment.NewLine;
            if (FirstName != null && LastName != null)
            {
                content += $"FN;CHARSET=UTF-8:{FirstName} {LastName}" + Environment.NewLine;
                content += $"N;CHARSET=UTF-8:{LastName};{FirstName};;;;" + Environment.NewLine;
            }
            content += $"EMAIL;CHARSET=UTF-8;type=HOME,INTERNET:{PEmail}" + Environment.NewLine;
            content += $"EMAIL;CHARSET=UTF-8;type=WORK,INTERNET:{WEmail}" + Environment.NewLine;
            content += $"TEL;TYPE=HOME,VOICE:{PTelefoon}" + Environment.NewLine;
            content += $"TEL;TYPE=WORK,VOICE:{WTelefoon}" + Environment.NewLine;
            content += $"ROLE;CHARSET=UTF-8:{JobTitel}" + Environment.NewLine;
            content += $"ORG;CHARSET=UTF-8:{Bedrijf}" + Environment.NewLine;
            content += $"X-SOCIALPROFILE;TYPE=facebook:{Facebook}" + Environment.NewLine;
            content += $"X-SOCIALPROFILE;TYPE=linkedin:{Linkedin}" + Environment.NewLine;
            content += $"X-SOCIALPROFILE;TYPE=instagram:{Instagram}" + Environment.NewLine;
            content += $"X-SOCIALPROFILE;TYPE=youtube:{Youtube}" + Environment.NewLine;

            byte[] imageBytes;
            using (MemoryStream ms = new MemoryStream())
            {
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(Photo as BitmapSource));
                encoder.Save(ms);
                imageBytes = ms.ToArray();
            }
            string base64String = Convert.ToBase64String(imageBytes);
            content += $"PHOTO;ENCODING=b;TYPE=JPEG:{base64String}" + Environment.NewLine;

            DateTime birthDate = (DateTime)BirthDate;
            string date = birthDate.ToString("yyyyMMdd");
            content += $"BDAY:{date}" + Environment.NewLine;
            if (!string.IsNullOrEmpty(_gender))
            {
                content += $"GENDER:{_gender}" + Environment.NewLine;
            }
            content += "END:VCARD";

            return content;
        }
    }
}
