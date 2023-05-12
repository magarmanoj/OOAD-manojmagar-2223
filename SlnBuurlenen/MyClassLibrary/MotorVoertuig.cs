using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary
{
    public class MotorVoertuig : Voertuig
    {
        public Enums.TransmissieType Transmissie { get; set; }

        public Enums.BrandstofType Brandstof { get; set; }
    }
}
