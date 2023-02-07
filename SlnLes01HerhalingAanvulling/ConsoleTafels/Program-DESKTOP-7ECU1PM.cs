using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTafels
{
    internal class Program
    {
        //ssdsdsdsd
        
        static void Main(string[] args)
        {

 
            Console.WriteLine("4x8 tafel: ");
            DrukTafel(4, 8);

            Console.WriteLine("\n2x5 tafel: ");
            DrukTafel(2, 5);


            Console.Write("\nGeef een getal: ");
            int getal = VraagPositiefGetal();
            Console.Write("Geef de lengte: ");
            int lengte = VraagPositiefGetal();
            Console.WriteLine($"{getal}x{lengte} tafel: ");
            DrukTafel(getal, lengte);
            Console.ReadKey(true);

        }

        private static void DrukTafel(int getal, int lengte)
        {
            int result;
            for(int i = 1; i <= lengte; i++)
            {
                result = getal * i;
                Console.WriteLine($"{ getal} x {i} = {result}");
            }
           
        }

        private static int VraagPositiefGetal()
        {
            int result;
            while (!int.TryParse(Console.ReadLine(), out result) || result <= 0)
            {
                Console.Write("Het geal moet positief zijn! Geef een getal: ");
            }
            return result;
            
        }
        
        
    }
}
