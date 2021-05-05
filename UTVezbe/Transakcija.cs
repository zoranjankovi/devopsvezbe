using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTVezbe.Exceptions;

namespace UTVezbe
{
	public class Transakcija: ITransakcija
	{
		public Transakcija(VrstaTransakcije tipTransakcije, IRacun racunUplatioca, IRacun racunPrimaoca, Decimal iznos)
		{
			if (racunUplatioca == null || racunPrimaoca == null)
            {
				throw new ArgumentNullException();
            }
			if (tipTransakcije != VrstaTransakcije.Prenos || iznos <= 0)
            {
				throw new InvalidTransactionException();
			}
			this.TipTransakcije = tipTransakcije;
			this.RacunUplatioca = racunUplatioca;
			this.RacunPrimaoca = racunPrimaoca;
			this.Iznos = iznos;
		}

		public Transakcija(VrstaTransakcije tipTransakcije, IRacun racunUplatioca, Decimal iznos)
		{
			if (racunUplatioca == null)
			{
				throw new ArgumentNullException();
			}
			if (!(tipTransakcije == VrstaTransakcije.Uplata ||tipTransakcije == VrstaTransakcije.Isplata) || iznos <= 0)
			{
				throw new InvalidTransactionException();
			}

			this.TipTransakcije = tipTransakcije;
			this.RacunUplatioca = racunUplatioca;
			this.Iznos = iznos;
		}

		public DateTime VremeTransakcije { get; set; }
        public VrstaTransakcije TipTransakcije { get; set; }
        public IRacun RacunUplatioca { get; set; }
		public IRacun RacunPrimaoca { get; set; }
		public decimal Iznos { get; set; }
	}
}
