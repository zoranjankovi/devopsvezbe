using Moq;
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
    public class RacunTest
    {
        private Mock<IOsoba> vlasnikOsobaMoq;
        private IRacun testingObject;
        private Random random;

        [SetUp]
        public void SetUpMethod()
        {
            vlasnikOsobaMoq = new Mock<IOsoba>();
            random = new Random();
        }

        [TearDown]
        public void TearDownMethod()
        {
            vlasnikOsobaMoq = null;
            random = null;
        }

        /// <summary>
        /// Negativni test sa NSubstitute mocked objektom
        /// </summary>
        [Test]
        public void CheckAccountNumberNegativeTest()
        {
            vlasnikOsobaMoq.Setup(t => t.JMBG).Returns("2400007000670");
            testingObject = new Racun(vlasnikOsobaMoq.Object, "3452407980800067000", 50000);
            Assert.IsFalse(testingObject.ProveriBrojRacuna());
        }

        [Test]
        public void CheckJMBGIsCalled()
        {

            IRacun testingObject1 = new Racun(vlasnikOsobaMoq.Object, "3452407980800067000", 50000);
            vlasnikOsobaMoq.SetupGet(t => t.JMBG).Returns("0000000000000");
            testingObject1.ProveriBrojRacuna();
            vlasnikOsobaMoq.Verify(t => t.JMBG);
        }

        [Test]
        public void CheckOsobaImeIsNotCalled()
        {
            IRacun testingObject1 = new Racun(vlasnikOsobaMoq.Object, "3452407980800067000", 50000);
            vlasnikOsobaMoq.SetupGet(t => t.JMBG).Returns("0000000000000");
            testingObject1.ProveriBrojRacuna();
            vlasnikOsobaMoq.Verify(t => t.Ime, Times.Never);
        }

        [Test]
        public void InvalidaAgeTest()
        {
            vlasnikOsobaMoq.Setup(t => t.GetStarost()).Throws<InvalidAgeException>();
            testingObject = new Racun(vlasnikOsobaMoq.Object, "3452407980800067000", 50000);
            testingObject.PostaviDozvoljeniMinusZaSeniore();
            Assert.AreEqual(testingObject.DozvoljeniMinus, 0);
        }

        [Test]
        public void PostaviDozvoljeniMinusZaSenioreTest()
        {
            vlasnikOsobaMoq.Setup(t => t.GetStarost()).Returns(65);
            testingObject = new Racun(vlasnikOsobaMoq.Object, "3452407980800067000", 50000);
            testingObject.PostaviDozvoljeniMinusZaSeniore();
            Assert.AreEqual(testingObject.DozvoljeniMinus, 10000);
        }

        [Test]
        public void PostaviDozvoljeniMinusZaMladjeTest()
        {
            vlasnikOsobaMoq.Setup(t => t.GetStarost()).Returns(35);
            testingObject = new Racun(vlasnikOsobaMoq.Object, "3452407980800067000", 60000);
            testingObject.PostaviDozvoljeniMinusZaSeniore();
            Assert.AreEqual(testingObject.DozvoljeniMinus, 60000);
        }

        [Test]
        public void UplataNegativeAmountTest()
        {
            Assert.Throws<ArgumentException>(() =>
           new Racun(vlasnikOsobaMoq.Object, "3452407980800067000", 50000).Uplata(-45));
        }

        [Test]
        public void UplataPositiveTest()
        {
            testingObject = new Racun(vlasnikOsobaMoq.Object, "3452407980800067000", 50000);
            Assert.DoesNotThrow(() => testingObject.Uplata((decimal)45.0));
        }

        [Test]
        public void IsplataPositiveTest()
        {
            testingObject = new Racun(vlasnikOsobaMoq.Object, "3452407980800067000", 50000);
            Assert.DoesNotThrow(() => testingObject.Isplata((decimal)45.0));
        }


        [Test]
        public void IsplataNegativeAmountTest()
        {
            Assert.Throws<ArgumentException>(() => new Racun(vlasnikOsobaMoq.Object, "3452407980800067000", 50000).Isplata(-45));
        }

        [Test]
        public void TransakcijaPositiveTest()
        {
            IRacun racunUplate = new Racun(vlasnikOsobaMoq.Object, "3452407980800067000", 50000);
            IRacun racunIsplate = new Racun(vlasnikOsobaMoq.Object, "3452507980800067000", 50000);
            racunIsplate.Uplata(150000);
            Assert.DoesNotThrow(() => racunIsplate.Prenos(racunUplate, 60000));
        }

        [Test]
        public void TransakcijaPrekoracenDozvoljenMinusTest()
        {
            IRacun racunUplate = new Racun(vlasnikOsobaMoq.Object, "3452407980800067000", 50000);
            IRacun racunIsplate = new Racun(vlasnikOsobaMoq.Object, "3452507980800067000", 50000);
            racunIsplate.Uplata(10000);
            Assert.IsFalse(racunIsplate.Prenos(racunUplate, 70000));
        }

        [Test]
        public void PrenosNegativeAmountTest()
        {
            Assert.Throws<ArgumentException>(() => PrenesiNaRacun());
        }

        private void PrenesiNaRacun()
        {
            IRacun racunUplate = new Racun(vlasnikOsobaMoq.Object, "3452407980800067000", 50000);
            IRacun racunIsplate = new Racun(vlasnikOsobaMoq.Object, "3452507980800067000", 50000);
            racunIsplate.Prenos(racunUplate, -1);
        }

        [Test]
        public void PrenosTest()
        {
            IRacun racunUplate = new Racun(vlasnikOsobaMoq.Object, "3452407980800067000", 50000);
            IRacun racunIsplate = new Racun(vlasnikOsobaMoq.Object, "3452507980800067000", 50000);
            racunIsplate.Uplata(10000);
            Assert.IsTrue(racunIsplate.Prenos(racunUplate, 10000));
        }

        [Test]
        public void PrenosIstiRacunTest()
        {
            IRacun racunUplateIsplate = new Racun(vlasnikOsobaMoq.Object, "3452407980800067000", 50000);
            racunUplateIsplate.Uplata(10000);
            Assert.IsTrue(racunUplateIsplate.Prenos(racunUplateIsplate, 10000));
        }

        [Test]
        public void PrenosDrugiRacunTest()
        {
            IRacun racunUplatioca = new Racun(vlasnikOsobaMoq.Object, "3452407980800067000", 50000);
            Mock<IOsoba> vlasnikOsobaMoq2 = new Mock<IOsoba>();
            vlasnikOsobaMoq2.Setup(t => t.JMBG).Returns("2507980800067");
            IRacun racunIsplate = new Racun(vlasnikOsobaMoq2.Object, "3452507980800067000", 50000);
            racunUplatioca.Uplata(10000);
            Assert.IsTrue(racunIsplate.Prenos(racunUplatioca, 10000));
        }

        [Test]
        public void KamataTest()
        {
            IRacun racunUplateIsplate = new Racun(vlasnikOsobaMoq.Object, "3452407980800067000", 50000);
            racunUplateIsplate.Uplata(10000);
            for (int i = 1; i <= 10; i++)
            {
                racunUplateIsplate.Prenos(racunUplateIsplate, 100);
            }


            Assert.IsTrue(racunUplateIsplate.Prenos(racunUplateIsplate, 100));
        }

        [Test]
        public void BrojRacunaTest()
        {
            string br = "3452507980800067000";
            IRacun racun = new Racun(vlasnikOsobaMoq.Object, "3452407980800067000", 50000)
            {
                BrojRacuna = br
            };
            Assert.AreEqual(racun.BrojRacuna, br);
        }

        [Test]
        public void StanjeTest()
        {
            decimal stanje = 11000;
            IRacun racun = new Racun(vlasnikOsobaMoq.Object, "3452407980800067000", 50000)
            {
                Stanje = stanje
            };
            Assert.AreEqual(racun.Stanje, stanje);
        }

        [Test]
        public void InvalidArgumentConstructorTest()
        {
            Assert.Throws<ArgumentException>(() => new Racun(null, null, 0));
        }

        [Test]
        public void TransactionSumTest()
        {
            List<decimal> uplataList = new List<decimal>();
            List<decimal> isplataList = new List<decimal>();
            for (int i = 1; i <= 10; i++)
            {
                uplataList.Add((decimal)i * 2);
                isplataList.Add((decimal)i);
            }
            decimal dozvoljeniMinus = isplataList.Sum();
            Racun racun = new Racun(vlasnikOsobaMoq.Object, "3452407980800067000", dozvoljeniMinus);
            uplataList.ForEach(t => racun.Uplata(t));
            isplataList.ForEach(t => racun.Isplata(t));
            decimal saldo = uplataList.Sum() - isplataList.Sum();
            Assert.AreEqual(saldo, racun.GetTransactionSum());
        }

        [Test]
        public void TransactionSumTest1()
        {
            List<decimal> uplataList = new List<decimal>();
            List<decimal> isplataList = new List<decimal>();
            List<decimal> prenosList = new List<decimal>();
            for (int i = 1; i <= 10; i++)
            {
                uplataList.Add((decimal)i * 2);
                isplataList.Add((decimal)i);
                prenosList.Add(((decimal)i) / 2);
            }
            decimal dozvoljeniMinus = isplataList.Sum();
            IRacun racun = new Racun(vlasnikOsobaMoq.Object, "3452407980800067000", dozvoljeniMinus);
            Mock<IRacun> racunUplate = new Mock<IRacun>();
            vlasnikOsobaMoq.SetupGet(t => t.JMBG).Returns("2407980800067");
            racunUplate.Setup(t => t.Vlasnik).Returns(vlasnikOsobaMoq.Object);
            uplataList.ForEach(t => racun.Uplata(t));
            isplataList.ForEach(t => racun.Isplata(t));
            prenosList.ForEach(t => racun.Prenos(racunUplate.Object, t));
            decimal saldo = uplataList.Sum() - isplataList.Sum() - prenosList.Sum() * (decimal)1.01 + prenosList[9] * (decimal)0.05;
            Assert.AreEqual(saldo, racun.GetTransactionSum());
        }

        

    }
}
