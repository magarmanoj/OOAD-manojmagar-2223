using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary
{
    internal class Ontlening
    {
        public int Id { get; set; }
        public DateTime Vanaf { get; set; }
        public DateTime Tot { get; set; }
        public string Bericht { get; set; }
        public string Status { get; set; }
        public int VoertuigId { get; set; }
        public int AanvragerId { get; set; }
    }
}
