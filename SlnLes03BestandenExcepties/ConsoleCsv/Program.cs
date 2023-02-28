using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCsv
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random rand = new Random();
            string[] spelers = { "Zakaria", "Saleha", "Indra", "Ralph", "Francisco", "Marie" };
            string[] games = { "schaak", "dammen", "backgammon" }; 

            // creeert een CSV bestand en slaagt op in desktop met naam "wedstrijden.csv"
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = System.IO.Path.Combine(folderPath , "wedstrijden.csv");
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // 100 spelers 
                for (int i = 0; i < 100; i++)
                {
                    // 2 randoms spelers
                    string speler1 = spelers[rand.Next(spelers.Length)];
                    string speler2 = spelers[rand.Next(spelers.Length)];
                    while (speler2 == speler1)
                    {
                        speler1 = spelers[rand.Next(spelers.Length)];
                    }

                    // random games
                    string game = games[rand.Next(games.Length)];
                    int score1 = rand.Next(3);
                    int score2 = rand.Next(3);
                    while (score1 == score2)
                    {
                        score2 = rand.Next(3);
                    }
                    writer.WriteLine($"{speler1};{speler2};{game};{score1}-{score2}");
                }
            }
            Console.WriteLine("Data saved to " + filePath);
            Console.ReadKey();
        }
    }
}

