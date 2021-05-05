using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTVezbe
{
    public interface IBanka
    {
        IRacun NoviRacun(IOsoba osoba, Decimal dozvoljeniMinus);
        Dictionary<IOsoba, List<IRacun>> Racuni { get; set; }
    }
}
