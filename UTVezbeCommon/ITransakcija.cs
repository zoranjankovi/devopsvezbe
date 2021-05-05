using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTVezbe;

namespace UTVezbe
{
    public interface ITransakcija
    {
        VrstaTransakcije TipTransakcije { get; set; }
        IRacun RacunUplatioca { get; set; }

        IRacun RacunPrimaoca { get; set; }

        Decimal Iznos { get; set; }
    }
}
