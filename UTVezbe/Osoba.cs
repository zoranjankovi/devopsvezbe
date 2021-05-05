using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UTVezbe.Exceptions;

namespace UTVezbe
{
	public class Osoba : IOsoba
	{
		private DateTime datumRodjenja;
		private Pol polOsobe;

		public string JMBG { get; set; }
		public string Ime { get; set; }
		public string Prezime { get; set; }
		public DateTime DatumRodjenja { get { return datumRodjenja; } set { datumRodjenja = value; } }
		public Pol PolOsobe { get { return polOsobe; } set { polOsobe = value; } }

		public Osoba(string jmbg, string ime, string prezime)
		{
			if (String.IsNullOrEmpty(jmbg) || String.IsNullOrEmpty(ime) || String.IsNullOrEmpty(prezime))
			{
				throw new ArgumentException();
			}
			if (!TryGetJmbgData(jmbg, out datumRodjenja, out polOsobe))
			{
				throw new JMBGCheckException();
			}
			this.Ime = ime;
			this.Prezime = prezime;
			this.JMBG = jmbg;
		}

		private bool TryGetPol(string jmbg, out Pol pol)
		{
			pol = Pol.Undefined;
			string oznakaZaPol = jmbg.Substring(10, 3);
			Int32.TryParse(oznakaZaPol, out int nOznakaZaPol);
			if (nOznakaZaPol > 499)
			{
				pol = Pol.Zensko;
			}
			else if (nOznakaZaPol >= 0)
			{
				pol = Pol.Musko;
			}
			else
			{
				return false;
			}
			return true;
		}

		private bool TryGetJmbgData(string jmbg, out DateTime datumRodjenja, out Pol pol)
		{
			pol = Pol.Undefined;
			return TryGetBirthDate(jmbg, out datumRodjenja) && TryGetPol(jmbg, out pol);
		}


		private bool TryGetBirthDate(string jmbg, out DateTime datumRodjenja)
		{
			datumRodjenja = new DateTime();
			if (jmbg.Length != 13)
			{
				return false;
			}
			string sDan = jmbg.Substring(0, 2);
			string sMesec = jmbg.Substring(2, 2);
			string sGodina = jmbg.Substring(4, 3);
			if (!(Int32.TryParse(sDan, out int nDan) && Int32.TryParse(sMesec, out int nMesec) && Int32.TryParse(sGodina, out _)))
			{
				return false;
			}

			int nGodina;
			if (sGodina[0] == '9')
			{
				nGodina = Int32.Parse("1" + sGodina);
			}
			else
			{
				nGodina = Int32.Parse("2" + sGodina);
			}
			try
			{
				datumRodjenja = new DateTime(nGodina, nMesec, nDan);
			}
			catch (ArgumentOutOfRangeException)
			{
				return false;
			}
			return true;
		}

		public double GetStarost()
		{
			double age = (DateTime.Now - datumRodjenja).TotalDays / 365.4;
			if (age > 120)
			{
				throw new InvalidAgeException();
			}
			return age;
		}

		public override bool Equals(object obj)
		{
			if (obj != null && obj is IOsoba osoba)
			{
				return osoba.JMBG == JMBG;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public bool CheckJMBG()
		{
			Regex reg = new Regex(@"^\d{13}$");
			return reg.IsMatch(JMBG);
		}
	}
}
