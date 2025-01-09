using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_za_biblioteku.Entities
{
    public class Clanarina
    {
        public int Id { get; set; }
        public string Naziv { get; set; } 
        public double Cijena { get; set; } 

        public override string ToString()
        {
            return $"{Naziv} - {Cijena} KM";
        }
    }
}
