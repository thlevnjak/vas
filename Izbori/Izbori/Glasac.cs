using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izbori
{
    public class Glasac
    {
        public Glasac(int id, OsobineEnum osobina)
        {
            ID = id;
            Osobina = osobina;
        }
        public int ID { get; set; }
        public OsobineEnum Osobina { get; set; }
    }
}
