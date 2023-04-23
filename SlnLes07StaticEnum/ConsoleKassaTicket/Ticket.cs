using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleKassaTicket
{
    internal class Ticket
    {
        public List<Product> Producten { get; set; } = new List<Product>();
        public Betaalwijze BetaaldMet { get; set; } = Betaalwijze.Cash;

        public string Kassier { get; set; }

        public void VoegProductToe(Product product)
        {
            if (string.IsNullOrEmpty(Kassier))
            {
                throw new ArgumentException("Kassier en betaalwijze moeten worden opgegeven om een product aan het ticket toe te voegen.");
            }
            Producten.Add(product);
        }

        public Ticket(string kassier, Betaalwijze betaalWijze)
        {
            Kassier = kassier;
            BetaaldMet = betaalWijze;
        }

        public void DrukTicket()
        {
            Console.WriteLine("KASSATICKET");
            Console.WriteLine("==========================");
            Console.WriteLine("Kassier: " + Kassier);
            Console.Write(Environment.NewLine);

            foreach (Product p in Producten)
            {
                Console.WriteLine(p.ToString());
            }

            Console.WriteLine("==========================");
            if (BetaaldMet == Betaalwijze.Visa)
            {
                Console.WriteLine("Visa kosten: €0,12");
            }
           
            Console.WriteLine("Totaalprijs: " + TotaalPrijs.ToString("C"));
        }

        public decimal TotaalPrijs
        {
            get { return Producten.Sum(p => p.Eenheidsprijs); }
        }
    }
}
