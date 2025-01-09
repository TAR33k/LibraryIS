using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_za_biblioteku.Entities
{
    public class Korisnik
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string ImePrezime => $"{Ime} {Prezime}";
        public string BrojTelefona { get; set; }
        public string Email { get; set; }
        public Clanarina Clanarina { get; set; }
        public string VrstaClanarine
        {
            get
            {
                return Clanarina?.Naziv ?? "Nema članarine";
            }
        }

        public bool Aktivni { get; set; } 
        public DateTime? DatumIsteka { get; set; }

        public List<Knjiga> PosudjeneKnjige { get; set; } = new List<Knjiga>();

        public string StatusClanarine
        {
            get
            {
                if (DatumIsteka < DateTime.Now)
                {
                    return "Istekla";
                }
                return "Aktivna";
            }
        }

        public override string ToString()
        {
            return $"{Ime} {Prezime}";
        }
    }
}
