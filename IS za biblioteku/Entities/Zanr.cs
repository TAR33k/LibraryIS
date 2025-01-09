using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_za_biblioteku.Entities
{
    public class Zanr
    {
      public int Id { get; set; }
      public string Naziv { get; set; }

      public override string ToString()
      {
         return Naziv;
      }
  
    }
}
