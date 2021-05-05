using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTVezbe;
using UTVezbe.Exceptions;

namespace UTVezbeTestSuite
{
    [TestFixture]
    public class OsobaTest
    {
        [Test]
        [TestCase("Pera", "Peric", "0102991153321")]
        [TestCase("Milos", "Micic", "0312002130321")]
        [TestCase("Ana", "Djuric", "2510965236557")]
        public void OsobaKonstruktorDobriParametri(string ime, string prezime, string jmbg)
        {
            IOsoba osoba = new Osoba(jmbg, ime, prezime);

            Assert.AreEqual(osoba.Ime, ime);
            Assert.AreEqual(osoba.Prezime, prezime);
            Assert.AreEqual(osoba.JMBG, jmbg);

            if (int.Parse(jmbg.Substring(9, 3)) < 500)
            {
                Assert.AreEqual(osoba.PolOsobe, Pol.Musko);
            }
            else
            {
                Assert.AreEqual(osoba.PolOsobe, Pol.Zensko);
            }
        }


        [Test]
       
        [TestCase("Milos", "Micic", "03120021303211")]
        [TestCase("Ana", "Djuric", "251096523655")]
        [TestCase("Milos", "Micic", "3212002130321")]
        [TestCase("Ana", "Djuric", "31026523655d")]
        public void OsobaKonstruktorLosiParametriJMBG(string ime, string prezime, string jmbg)
        {
            Assert.Throws<JMBGCheckException>(() =>
            {
                IOsoba osoba = new Osoba(jmbg, ime, prezime);
            }
            );
        }
        [Test]
        [TestCase("Milos", "", "0312002130321")]
        [TestCase("Ana", "Djuric", "")]
        [TestCase("", "Peric", "0102991153321")]
        [TestCase(null, "Djuric", "31026523655d")]
        [TestCase("Ana", null, "31026523655d")]
        [TestCase("Ana", "Djuric", null)]
        public void OsobaKonstruktorLosiParametriArguments(string ime, string prezime, string jmbg)
        {

            Assert.Throws<ArgumentException>(() =>
            {
                IOsoba osoba = new Osoba(jmbg, ime, prezime);
            }
            );
        }

        [Test]
        public void CheckJMBGTest()
        {
            IOsoba o = new Osoba("1212122134321", "Petar", "Petrovic");
            Assert.AreEqual(true, o.CheckJMBG());
            o.JMBG = "1231231";
            Assert.AreEqual(false, o.CheckJMBG());
            o.JMBG = "aasda23324";
            Assert.AreEqual(false, o.CheckJMBG());
        }
    }
}
