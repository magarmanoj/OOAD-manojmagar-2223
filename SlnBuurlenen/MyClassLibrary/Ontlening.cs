using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary
{
    public class Ontlening
    {
        public int Id { get; set; }
        public DateTime Vanaf { get; set; }
        public DateTime Tot { get; set; }
        public string Bericht { get; set; }
        public Enums.OntleningStatus Status { get; set; }
        public Voertuig Voertuig { get; set; }
        public Gebruiker Aanvrager { get; set; }

        public static Ontlening FindById(int id)
        {
            // sql voor find ontlenning, voertuig en aanvraag
            Voertuig voertuig = new Voertuig();
            voertuig.Id = id;

            Ontlening ontlening = new Ontlening();
            ontlening.Voertuig = voertuig;

            return ontlening;
        }
    }
}
