using System;
using System.Collections.Generic;

namespace ConsoleKassaTicket
{
    public enum Betaalwijze 
    { 
        Visa, 
        Cash, 
        Bancontact 
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Product> producten = new List<Product>()
            {
                // https://zetcode.com/csharp/decimal/#:~:text=C%23%20decimal%20literal,without%20a%20suffix%20are%20double%20.&text=float%20n1%20%3D%201.234f%3B%20double,WriteLine(n1)%3B%20Console.
                new Product("P02384", "bananen", 1.75m),
                new Product("P01820", "brood", 2.10m),
                new Product("P45612", "keas", 3.99m),
                new Product("P98754", "koffie", 4.10m)
            };

            // Maak een ticket aan en voeg de producten toe
            Ticket ticket = new Ticket("Annie", Betaalwijze.Visa);
            foreach (Product product in producten)
            {
                ticket.VoegProductToe(product);
            }

            // Druk het ticket af
            ticket.DrukTicket();

            Console.ReadLine();
        }
    }
}
