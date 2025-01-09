using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_za_biblioteku.Entities
{
    public class Posudba
    {
        public Korisnik Korisnik { get; set; }
        public Knjiga Knjiga { get; set; }
        public DateTime DatumPosudbe { get; set; }
        public DateTime DatumVracanja { get; set; }
        public bool Status
        {
            get
            {
                return DateTime.Now <= DatumVracanja;
            }
        }
    }
}
