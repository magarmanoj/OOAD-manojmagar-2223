using System;
using System.Collections.Generic;

namespace ConsoleVeiling
{
    internal class Item
    {
        private string _naamItem;
        private decimal _minimumbod;
        private decimal _huidigebod;
        private string _winaar;
        private bool _isVerkocht;
        private List<Bod> _biedingen = new List<Bod>();

        public bool IsVerkocht
        {
            get { return _isVerkocht; }
        }

        public string Naam {
            get
            {
                return _naamItem;
            }
        }

        public string Winaar
        {
            get { return _winaar; }
        }

        public decimal Huidigebod {
            get { return _huidigebod; }
            set { _huidigebod = value; }
        }

        public decimal Minimumbod { 
            get{ return _minimumbod; }
        }

        public Item(string naamItem, decimal minimumbod)
        {
            _naamItem = naamItem;
            _minimumbod = minimumbod;
        }

        public Bod WinnendeBod()
        {
            if (_isVerkocht)
            {
                Bod winnendBod = null;
                foreach (Bod bod in _biedingen)
                {
                    if (winnendBod == null || bod.Bedrag > winnendBod.Bedrag)
                    {
                        winnendBod = bod;
                    }
                }
                return winnendBod;
            }
            else
            {
                return null;
            }
        }

        public void Bieden(Koper koper, Bod bedrag)
        {
            if (IsVerkocht)
            {
                throw new InvalidOperationException("Dit item is al verkocht.");
            }
            if (bedrag.Bedrag < Minimumbod)
            {
                throw new InvalidOperationException("Bedrag is lager dan minimum waarde");
            }
            if (bedrag.Bedrag > Huidigebod)
            {
                if (_winaar != null)
                {
                    koper.Aangeschafte.Remove(this);
                }
                Huidigebod = bedrag.Bedrag;
                _winaar = koper.Name;
                koper.Aangeschafte.Add(this);
                _biedingen.Add(bedrag);
            }
        }
        public void SluitKoop()
        {
            _isVerkocht = true;
        }
    }
}
