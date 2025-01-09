using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_za_biblioteku.Entities
{
    public class Autor
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }

        public override string ToString()
        {
            return $"{Ime} {Prezime}";
        }
    }
}
