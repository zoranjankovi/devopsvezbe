using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTVezbe
{
    public class Banka : IBanka
    {
        private readonly string bankId = "245";

        private Dictionary<IOsoba, List<IRacun>> racuni = new Dictionary<IOsoba, List<IRacun>>();

        public Banka(string bankId)
        {
            if (bankId == null || !Int32.TryParse(bankId, out _))
            {
                throw new ArgumentException();
            }
            this.bankId = bankId;

        }

        public Dictionary<IOsoba, List<IRacun>> Racuni { get => racuni; set => racuni = value; }

        public IRacun NoviRacun(IOsoba osoba, decimal dozvoljeniMinus)
        {
            if (osoba == null || dozvoljeniMinus <= 0)
            {
                throw new ArgumentException();
            }

            if (!racuni.TryGetValue(osoba, out List<IRacun> racunList))
            {
                racunList = new List<IRacun>();
                racuni.Add(osoba, racunList);
            }
            IRacun racun = new Racun(osoba, bankId + osoba.JMBG, dozvoljeniMinus);
            racunList.Add(racun);
            return racun;
        }
    }
}
