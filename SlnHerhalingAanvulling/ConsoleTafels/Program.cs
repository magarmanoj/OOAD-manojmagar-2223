using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTafels
{
    internal class Program
    {
        
        static void Main(string[] args)
        {

 
            Console.WriteLine("4x8 tafel: ");
            DrukTafel(4, 8);
            Console.WriteLine("\n2x5 tafel: ");
            DrukTafel(2, 5);


            Console.Write("\nGeef een getal:");
            int getal = VraagPositiefGetal();
            Console.Write("Geef de lengte van de tafel:");
            int lengte = VraagPositiefGetal();
            Console.WriteLine("Tafel van " + getal + ":");
            DrukTafel(getal, lengte);
            Console.ReadKey(true);

        }

        static void DrukTafel(int getal, int lengte )
        {
            int result;
            for(int i = 1; i <= lengte; i++)
            {
                result = getal * i;
                Console.WriteLine($"{ getal} x {i} = {result}");
            }
           
        }

        static int VraagPositiefGetal()
        {
            int result;
            while (!int.TryParse(Console.ReadLine(), out result) || result <= 0)
            {
                Console.WriteLine("Ongeldige invoer, geef een positief getal:");
            }
            return result;
            
        }
        
        
    }
}
