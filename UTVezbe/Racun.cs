using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTVezbe.Exceptions;

namespace UTVezbe
{
	public class Racun : IRacun
	{
		private readonly int ACCOUNT_NUMBER_LENGTH = 19;
		private readonly string INVALID_ACCOUNT_NUMBER_START = "000";
		private IOsoba vlasnik;
		private decimal stanje;
		private decimal dozvoljeniMinus;
		private string brojRacuna;
		private readonly List<Transakcija> transactionHistory = new List<Transakcija>();
		private int transactionCounter = 0;

		public IOsoba Vlasnik
		{
			get { return vlasnik; }
			set { vlasnik = value; }
		}

		public string BrojRacuna
		{
			get { return brojRacuna; }
			set { brojRacuna = value; }
		}

		public decimal Stanje
		{
			get { return stanje; }
			set { stanje = value; }
		}

		public decimal DozvoljeniMinus
		{
			get { return dozvoljeniMinus; }
			set { dozvoljeniMinus = value; }
		}

		public Racun(IOsoba vlasnik, string brojRacuna, decimal dozvoljenMinus)
		{
			if (brojRacuna == null || vlasnik == null || dozvoljenMinus <= 0)
			{
				throw new ArgumentException();
			}
			this.Vlasnik = vlasnik;
			this.BrojRacuna = brojRacuna;
			this.DozvoljeniMinus = dozvoljenMinus;
		}

		private bool AccountNumberLengthValid()
		{
			return brojRacuna.Length == ACCOUNT_NUMBER_LENGTH;
		}

		private bool JMBGFitsAccountNumber()
		{
			string jmbg = vlasnik.JMBG;
			return brojRacuna.Substring(3).Contains(jmbg);
		}

		private bool CheckAccountNumberStart()
		{
			return (!brojRacuna.StartsWith(INVALID_ACCOUNT_NUMBER_START));
		}


		public bool ProveriBrojRacuna()
		{
			return AccountNumberLengthValid() && JMBGFitsAccountNumber() && CheckAccountNumberStart();
		}

		public void PostaviDozvoljeniMinusZaSeniore()
		{
			try
			{
				double starost = vlasnik.GetStarost();
				if (starost > 64)
				{
					dozvoljeniMinus = 10000;
				}
			}
			catch (InvalidAgeException)
			{
				dozvoljeniMinus = 0;
			}
		}


		public void Uplata(decimal iznos)
		{
			if (iznos < 0)
			{
				throw new ArgumentException();
			}
			transactionHistory.Add(new Transakcija(VrstaTransakcije.Uplata, this, iznos));
			stanje += iznos;
			Console.WriteLine("Nova uplata: " + iznos.ToString());

		}

		public void Isplata(decimal iznos)
		{
			if (iznos < 0)
			{
				throw new ArgumentException();
			}
			transactionHistory.Add(new Transakcija(VrstaTransakcije.Isplata, this, iznos));
			stanje -= iznos;
			Console.WriteLine("Nova uplata: " + iznos.ToString());
		}

		public bool Prenos(IRacun racunUplate, decimal iznos)
		{
			if (iznos < 0)
			{
				throw new ArgumentException();
			}
			transactionHistory.Add(new Transakcija(VrstaTransakcije.Prenos, racunUplate, this, iznos));
			decimal provizija = 1;
			if (!vlasnik.Equals(racunUplate.Vlasnik))
			{
				provizija = 13;
			}
			decimal iznosProvizije = (iznos * (provizija / 100));
			if ((stanje + dozvoljeniMinus) < iznos + iznosProvizije)
			{
				return false;
			}
			Isplata(iznos * (1 + (provizija / 100)));
			racunUplate.Uplata(iznos);
			transactionCounter++;
			Kamata(iznos);
			return true;
		}

		private void Kamata(decimal iznos)
		{
			if (transactionCounter == 10)
			{
				Uplata(iznos * (decimal)0.05);
				transactionCounter = 0;
			}
		}

		public decimal GetTransactionSum()
        {
			return transactionHistory.Where(t => t.TipTransakcije == VrstaTransakcije.Uplata).Sum(t1 => t1.Iznos) -
				transactionHistory.Where(t => t.TipTransakcije == VrstaTransakcije.Isplata).Sum(t => t.Iznos);
        }
	}
}
