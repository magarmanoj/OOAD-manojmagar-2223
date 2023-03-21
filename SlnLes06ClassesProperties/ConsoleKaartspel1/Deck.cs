using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleKaartspel1
{
    internal class Deck
    {
        private List<Kaart> kaarten;

        public List<Kaart> Kaarten
        {
            get { return kaarten; }
        }

        public Deck()
        {
            kaarten = new List<Kaart>();
            for (int i = 1; i <= 13; i++)
            {
                kaarten.Add(new Kaart(i, "C"));
                kaarten.Add(new Kaart(i, "S"));
                kaarten.Add(new Kaart(i, "H"));
                kaarten.Add(new Kaart(i, "D"));
            }
        }

        public void Schudden()
        {
            Random rnd = new Random();
            int count = kaarten.Count;
            while (count > 1)
            {
                count--;
                int i = rnd.Next(count + 1);
                Kaart kaart = kaarten[i];
                kaarten[i] = kaarten[count];
                kaarten[count] = kaart;
            }
        }

        public Kaart NeemKaart()
        {
            if (kaarten.Count == 0)
            {
                throw new InvalidOperationException("Geen kaarten meer in het deck.");
            }
            Kaart kaart = kaarten[0];
            kaarten.RemoveAt(0);
            return kaart;
        }
    }
}
