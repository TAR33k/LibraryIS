using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_za_biblioteku.Entities
{
    public class Knjiga
    {
        public int Id { get; set; }
        public string Naslov { get; set; }
        public int DostupnaKolicina { get; set; }
        public bool Dostupna { get; set; } 
        public Autor Autor { get; set; }
        public Zanr Zanr { get; set; }
        public int GodinaIzdavanja { get; set; }

        public override string ToString()
        {
            return $"{Naslov} ({GodinaIzdavanja}) - {Autor}";
        }
    }
}
