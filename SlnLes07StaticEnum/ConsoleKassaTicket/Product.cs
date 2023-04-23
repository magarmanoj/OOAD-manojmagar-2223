using System;

namespace ConsoleKassaTicket
{
    internal class Product
    {
        public string Name { get; set; }
        public decimal Eenheidsprijs { get; set; }

        public string Code { get; set; }
        public static bool ValideerCode(string valide)
        {
            return valide != null;
        }

        public string ToString()
        {
            string product;
            product = $"({Code}) {Name}: {Eenheidsprijs}"; 
            return product;
        }

        public Product(string code, string name, decimal eenheidsprijs)
        {
            if (code.Length != 6 || !code.StartsWith("P"))
            {
                throw new ArgumentException("Productcode moet uit 6 tekens bestaan en beginnen met 'P'");
            }

            Code = code;
            Name = name;
            Eenheidsprijs = eenheidsprijs;
        }
    }
}
