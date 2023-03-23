using System;
using System.Collections.Generic;

namespace ConsoleKaartspel1
{
    internal class Speler
    {
        private string naam;
        private List<Kaart> kaarten;

        public Speler(string naam)
        {
            this.naam = naam;
            kaarten = new List<Kaart>();
        }

        public Speler(string naam, List<Kaart> kaarten)
        {
            this.naam = naam;
            this.kaarten = kaarten;
        }

        public string Naam { get; set; }

        public List<Kaart> Kaarten 
        {
            get { return kaarten; }
            set { Kaarten = value; }
        }

        public bool HeeftNogKaarten
        {
            get { return kaarten.Count > 0; }
        }

        public Kaart LegKaart()
        {
            if (kaarten.Count == 0)
            {
                throw new InvalidOperationException("Geen kaarten meer in de hand van de speler.");
            }
            Random random = new Random();
            int i = random.Next(kaarten.Count);
            Kaart kaart = kaarten[i];
            kaarten.RemoveAt(i);
            return kaart;
        }
    }
}
