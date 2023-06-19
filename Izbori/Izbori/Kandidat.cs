using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izbori
{
    public class Kandidat
    {
        public Kandidat(ImenaEnum ime, PrezimenaEnum prezime, char oznaka, OsobineEnum osobina)
        {
            Ime = ime;
            Prezime = prezime;
            Oznaka = oznaka;
            Osobina = osobina;
        }
        public ImenaEnum Ime { get; set; }
        public PrezimenaEnum Prezime { get; set; }
        public char Oznaka { get; set; }
        public OsobineEnum Osobina { get; set; }
    }
}
