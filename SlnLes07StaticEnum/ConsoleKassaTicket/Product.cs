﻿using System;
using System.Globalization;

namespace ConsoleKassaTicket
{
    internal class Product
    {
        private string _code;
        public string Naam { get; set; }
        public decimal Eenheidsprijs { get; set; }

        public string Code
        {
            get { return _code; }
        }
        public static bool ValideerCode(string valide)
        {
            return valide.Length == 6 && valide.StartsWith("P");
        }

        public override string ToString()
        {
            return $"({Code}) {Naam}: {Eenheidsprijs}";
        }

        public Product(string code, string name, decimal eenheidsprijs)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Naam moet niet leeg zijn");
            }

            if (eenheidsprijs < 0)
            {
                throw new ArgumentException("Eenheidsprijs moet positief zijn.");
            }

            if (!ValideerCode(code))
            {
                throw new ArgumentException("Ongeldige code");
            }
            _code = code;
            Naam = name;
            Eenheidsprijs = eenheidsprijs;
        }
    }
}
