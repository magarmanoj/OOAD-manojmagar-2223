using System;
using System.Collections.Generic;

namespace ConsoleVeiling
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // max 3 keren bieden       
            Console.WriteLine(@"
VINTAGE MEUBELS VEILING
=========================
");
            Koper eersteKoper = new Koper("Jan");
            Koper tweedKoper = new Koper("Piet");
            Koper derdeKoper = new Koper("Klaas");

            Item eersteItem = new Item("Laptop", 700);
            Item tweedeItem = new Item("Vaas", 90);

            List<Item> items = new List<Item>() { eersteItem, tweedeItem };

            // Laat de kopers bieden op het item
            eersteItem.Bieden(eersteKoper, new Bod(eersteKoper, 900));
            eersteItem.Bieden(tweedKoper, new Bod(tweedKoper, 750));
            eersteItem.Bieden(derdeKoper, new Bod(derdeKoper, 800));
            eersteItem.SluitKoop();

            tweedeItem.Bieden(eersteKoper, new Bod(eersteKoper, 120));
            tweedeItem.Bieden(tweedKoper, new Bod(tweedKoper, 90));
            tweedeItem.Bieden(derdeKoper, new Bod(derdeKoper, 120));
            tweedeItem.SluitKoop();

            for (int i = 0; i < items.Count; i++)
            {
                Item item = items[i];
                Bod winnendBod = eersteItem.WinnendeBod();
                if (winnendBod == null)
                {
                    Console.WriteLine("Item 1 is niet verkocht.");
                }
                else
                {
                    Console.WriteLine($"De winnaar van de eerste veiling is: {item.Winaar}");
                }
            }
           
            List<Koper> kopers = new List<Koper> { eersteKoper, tweedKoper, derdeKoper };
            foreach (Koper koper in kopers)
            {               
                Console.WriteLine($"Koper {koper.Name} heeft de volgende items in bezit:");
                foreach (Item item in koper.Aangeschafte)
                {
                    if (item.Winaar == koper.Name)
                    {
                        Console.WriteLine(item.Naam);
                    }
                }
            }
            Console.ReadLine();
        }
    }
}