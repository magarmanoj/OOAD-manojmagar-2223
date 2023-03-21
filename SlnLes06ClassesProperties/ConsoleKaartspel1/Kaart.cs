using System;

namespace ConsoleKaartspel1
{
    internal class Kaart
    {
        private int nummer;
        private string kleur;

        public Kaart(int nummer, string kleur)
        {
            Nummer = nummer;
            Kleur = kleur;
        }

        public int Nummer
        {
            get
            {
                return nummer;
            }

            set
            {
                if (value < 1 || value > 13)
                    throw new ArgumentOutOfRangeException("Nummer moet tussen 1 en 13 zijn.");
                nummer = value;
            }
        }
        public string Kleur
        {
            get
            {
                return kleur;
            }

            set
            {
                if (value != "C" && value != "S" && value != "H" && value != "D")
                    throw new ArgumentOutOfRangeException("Kleur moet C, S, H of D zijn.");
                kleur = value;
            }
        }
    }
}
