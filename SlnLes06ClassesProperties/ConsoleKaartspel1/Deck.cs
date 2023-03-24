using System;
using System.Collections.Generic;

namespace ConsoleKaartspel1
{
    internal class Deck
    {
        private List<Kaart> _kaarten;

        public List<Kaart> Kaarten
        {
            get { return _kaarten; }
        }

        public Deck()
        {
            _kaarten = new List<Kaart>();
            for (int i = 1; i <= 13; i++)
            {
                _kaarten.Add(new Kaart(i, 'C'));
                _kaarten.Add(new Kaart(i, 'S'));
                _kaarten.Add(new Kaart(i, 'H'));
                _kaarten.Add(new Kaart(i, 'D'));
            }
        }

        public void Schudden()
        {
            Random rnd = new Random();
            int count = _kaarten.Count;
            while (count > 1)
            {
                count--;
                int i = rnd.Next(count + 1);
                Kaart kaart = _kaarten[i];
                _kaarten[i] = _kaarten[count];
                _kaarten[count] = kaart;
            }
        }

        public Kaart NeemKaart()
        {
            if (_kaarten.Count == 0)
            {
                throw new InvalidOperationException("Geen kaarten meer in het deck.");
            }
            Kaart kaart = _kaarten[0];
            _kaarten.RemoveAt(0);
            return kaart;
        }
    }
}
