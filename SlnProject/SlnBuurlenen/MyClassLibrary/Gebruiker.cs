﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary
{
    internal class Gebruiker
    {
        public int Id { get; set; }
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime Aanmaakdatum { get; set; }
        public bool IsAdmin { get; set; }
        public string Status { get; set; }
        public string Profielfoto { get; set; }
        public string Geslacht { get; set; }
    }
}
