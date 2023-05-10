using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary
{
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

    internal class MotorVoertuig : Voertuig
    {
        public string Transmissie { get; set; }
        public string Brandstof { get; set; }
        public int VoertuigId { get; set; }
    }
}
