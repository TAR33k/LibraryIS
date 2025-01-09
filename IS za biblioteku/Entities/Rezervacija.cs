using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_za_biblioteku.Entities
{
    public class Rezervacija
    {
        public int Id { get; set; }
        public Korisnik Korisnik { get; set; }
        public Knjiga Knjiga { get; set; }
        public DateTime DatumRezervacije { get; set; }
        public bool Notified { get; set; }
        public bool IsComplete { get; set; }
    }
}
