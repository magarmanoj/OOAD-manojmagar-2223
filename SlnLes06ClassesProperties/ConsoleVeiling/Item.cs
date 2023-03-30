using System;
using System.Collections.Generic;

namespace ConsoleVeiling
{
    internal class Item
    {  
        private List<Bod> Biedingen{ get; set; } = new List<Bod>();
        public bool IsVerkocht { get; set; }
        public string Naam { get; set; }
        public string Winaar { get; set; }
        public decimal Huidigebod { get; set; }
        public decimal Minimumbod { get; set; }

        public Item(string naamItem, decimal minimumbod)
        {
            Naam = naamItem;
            Minimumbod = minimumbod;
        }

        public Bod WinnendeBod
        {
            get
            {
                if (IsVerkocht)
                {
                    Bod winnendBod = null;
                    foreach (Bod bod in Biedingen)
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
        }

        public void Bieden(Bod bod)
        {
            if (IsVerkocht)
            {
                throw new InvalidOperationException("Dit item is al verkocht.");
            }
            if (bod.Bedrag < Minimumbod)
            {
                throw new InvalidOperationException("Bedrag is lager dan minimum waarde");
            }
            if (bod.Bedrag > Huidigebod)
            {
                if (Winaar != null)
                {
                    bod.Koper.Aangeschafte.Remove(this);
                }
                Huidigebod = bod.Bedrag;
                Winaar = bod.Koper.Name;
                bod.Koper.Aangeschafte.Add(this);
                Biedingen.Add(bod);
            }
        }
        public void SluitKoop()
        {
            IsVerkocht = true;
        }
    }
}
