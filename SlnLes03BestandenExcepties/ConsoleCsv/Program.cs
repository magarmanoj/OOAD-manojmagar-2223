using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCsv
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] spelers = { "Zakaria", "Saleha", "Indra", "Ralph", "Francisco", "Marie" };
            string[] games = { "schaak", "dammen", "backgammon" };
            Random rand = new Random();

            // creeert een CSV bestand en slaagt op in desktop met naam "wedstrijden.csv"
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\wedstrijden.csv";
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // Generate 100 random wedstrijd
                for (int i = 0; i < 100; i++)
                {
                    // twee random spelers
                    string speler1 = spelers[rand.Next(spelers.Length)];
                    string speler2 = spelers[rand.Next(spelers.Length)];
                    while (speler2 == speler1)
                    {
                        speler1 = spelers[rand.Next(spelers.Length)];
                    }
                    string game = games[rand.Next(games.Length)];
                    int score1 = rand.Next(3);
                    int score2 = rand.Next(3);
                    while (score1 == score2)
                    {
                        score2 = rand.Next(3);
                    }
                    Console.WriteLine($"{speler1};{speler2};{game};{score1}-{score2}");
                }
            }
            Console.WriteLine("Data saved to " + filePath);
            Console.ReadKey();
        }
    }
}

