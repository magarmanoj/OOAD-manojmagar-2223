using System;

namespace ConsoleVeiling
{
    internal class Item
    {
        private string _naamItem;
        private decimal _minimumbod;
        private decimal _huidigebod;
        private Koper winaar;

        public string Naam {
            get
            {
                return _naamItem;
            }
        }

        public Koper Winaar
        {
            get { return winaar; }
        }

        public decimal Huidigebod { get; set; }
        public decimal Minimumbod { 
            get{ return _minimumbod; }
        }

        public Item(string naamItem, decimal minimumbod)
        {
            _naamItem = naamItem;
            _minimumbod = minimumbod;
        }

        public void Bieden(Koper koper, Bod bedrag)
        {
            if (bedrag.Bedrag < Minimumbod)
            {
                throw new ArgumentException("Gegeven bod is lager dan minimumprijs");
            }
            if (bedrag.Bedrag > Huidigebod)
            {
                if (winaar != null)
                {
                    winaar.Aangeschafte.Remove(this);
                }
                Huidigebod = bedrag.Bedrag;
                winaar = koper;
                koper.Aangeschafte.Add(this);
            }
        }
    }
}
