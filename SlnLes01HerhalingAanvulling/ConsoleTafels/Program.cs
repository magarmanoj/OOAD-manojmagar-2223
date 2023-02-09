using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTafels
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("4x8 tafel: ");
            Console.WriteLine(DrukTafel(4, 8));
            Console.WriteLine("\n2x5 tafel: ");
            Console.WriteLine(DrukTafel(2, 5));

            int getal = VraagPositiefGetal("\nGeef een getal: ");
            int lengte = VraagPositiefGetal("Geef de lengte: ");
            Console.WriteLine($"{getal}x{lengte} tafel: ");
            Console.Write(DrukTafel(getal, lengte));
            Console.ReadKey(true);
        }

        private static string DrukTafel(int getal, int lengte)
        {
            int result;
            string tafel = "";
            for (int i = 1; i <= lengte; i++)
            {
                result = getal * i;
                tafel += $"{getal} x {i} = {result}\n";
            }
            return tafel;
        }

        private static int VraagPositiefGetal(string geef)
        { 
            Console.Write(geef);
            int result = int.Parse(Console.ReadLine());
            while (result < 0)
            {
                Console.Write("Het geal moet positief zijn! Geef een getal: ");
                result = int.Parse(Console.ReadLine());
            }
            return result;
        }
    }
}
