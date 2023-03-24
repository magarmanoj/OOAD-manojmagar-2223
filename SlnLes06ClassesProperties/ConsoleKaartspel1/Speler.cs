using System;
using System.Collections.Generic;

namespace ConsoleKaartspel1
{
    internal class Speler
    {
        private string _naam;
        private List<Kaart> _kaarten;

        public Speler(string naam)
        {
            this._naam = naam;
            _kaarten = new List<Kaart>();
        }

        public Speler(string naam, List<Kaart> kaarten)
        {
            this._naam = naam;
            this._kaarten = kaarten;
        }

        public string Naam { get; set; }

        public List<Kaart> Kaarten 
        {
            get { return _kaarten; }
            set { Kaarten = value; }
        }

        public bool HeeftNogKaarten
        {
            get { return _kaarten.Count > 0; }
        }

        public Kaart LegKaart()
        {
            if (_kaarten.Count == 0)
            {
                throw new InvalidOperationException("Geen kaarten meer in de hand van de speler.");
            }
            Random random = new Random();
            int i = random.Next(_kaarten.Count);
            Kaart kaart = _kaarten[i];
            _kaarten.RemoveAt(i);
            return kaart;
        }
    }
}
