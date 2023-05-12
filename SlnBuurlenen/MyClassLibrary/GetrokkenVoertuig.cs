using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary
{
    public class GetrokkenVoertuig : Voertuig
    {
        public int Gewicht { get; set; }
        public int MaxBelasting { get; set; }
        public string Afmetingen { get; set; }
        public bool Geremd { get; set; }
    }
}
