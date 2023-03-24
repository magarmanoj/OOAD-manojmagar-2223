using System;
using System.Collections.Generic;

namespace ConsoleVeiling
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // max 3 keren bieden       

            Koper eersteKoper = new Koper("Jan");
            Koper tweedKoper = new Koper("Piet");
            Koper derdeKoper = new Koper("Klaas");

            // Maak een item aan met een minimumprijs van 100 euro en een looptijd van 1 minuut
            Item eersteItem = new Item("Laptop", 700);
            Item tweedeItem = new Item("Vaas", 90);

            Bod eersteBod = new Bod(eersteKoper, 900);
            Bod tweedeBod = new Bod(tweedKoper, 750);
            Bod derdeBod = new Bod(derdeKoper, 800);

            Bod vBod = new Bod(eersteKoper, 99);
            Bod vijfBod = new Bod(tweedKoper, 750);
            Bod zesBod = new Bod(derdeKoper, 800);

            // Laat de kopers bieden op het item
            eersteItem.Bieden(eersteKoper, eersteBod);
            eersteItem.Bieden(tweedKoper, tweedeBod);
            eersteItem.Bieden(derdeKoper, derdeBod);

            tweedeItem.Bieden(eersteKoper, vBod);
            tweedeItem.Bieden(tweedKoper, vijfBod);
            tweedeItem.Bieden(derdeKoper, zesBod);

            // Sluit de veiling en bepaal de winnaar

            // Toon de winnaar van de veiling en de lijst met items in bezit van elke koper
            Console.WriteLine($"De winnaar van de veiling is: {eersteItem.Winaar.Name}");
            Console.WriteLine($"De winnaar van de veiling is: {tweedeItem.Winaar.Name}");
            Console.WriteLine(Environment.NewLine);
            List<Koper> Kopers = new List<Koper> { eersteKoper, tweedKoper, derdeKoper };
            foreach (Koper koper in Kopers)
            {
                // toon allen die iets hebben in zijn ItemLijst.
                if (koper.Aangeschafte.Count > 0)
                {
                    Console.WriteLine($"Koper {koper.Name} heeft de volgende items in bezit:");

                    foreach (Item item in koper.Aangeschafte)
                    {
                        Console.WriteLine(item.Naam);
                    }
                }
            }
            Console.ReadLine();
        }
    }
}
