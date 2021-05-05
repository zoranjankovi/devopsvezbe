using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTVezbe
{
	public interface IRacun
	{
		IOsoba Vlasnik { get; set; }
		string BrojRacuna {get; set; }
		Decimal Stanje { get; set; }
		Decimal DozvoljeniMinus { get; set; }
		bool ProveriBrojRacuna();
		void PostaviDozvoljeniMinusZaSeniore();
		void Uplata(decimal iznos);
		void Isplata(decimal iznos);
		bool Prenos(IRacun racunUplatioca, decimal iznos);

		decimal GetTransactionSum();
	}
}
