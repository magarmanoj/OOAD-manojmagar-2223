using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary
{
    internal class Enums
    {
        public enum OntleningStatus
        {
            InAanvraag,
            Goedgekeurd,
            Verworpen
        }

        public enum TransmissieType
        {
            Manueel,
            Automatisch
        }

        public enum BrandstofType
        {
            Benzine,
            Diesel
        }

        public enum GeslachtType
        {
            Man,
            Vrouw
        }
    }
}
