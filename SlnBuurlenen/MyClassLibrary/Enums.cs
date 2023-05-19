using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary
{
    public class Enums
    {
        public enum OntleningStatus
        {
            InAanvraag,
            Goedgekeurd,
            Verworpen
        }

        public enum TransmissieType
        {
            Manueel = 1,
            Automatisch = 2
        }

        public enum BrandstofType
        {
            Benzine = 1,
            Diesel = 2,
            LPG = 3
        }

        public enum GeslachtType
        {
            Man,
            Vrouw
        }
    }
}
