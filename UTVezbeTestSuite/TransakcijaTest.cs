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
    public class TransakcijaTest
    {
        private Mock<IRacun> racunMoq1;
        private Mock<IRacun> racunMoq2;
        [SetUp]
        public void SetupMethods()
        {
            racunMoq1 = new Mock<IRacun>();
            racunMoq2 = new Mock<IRacun>();
        }

        public void TearDown()
        {
            racunMoq1 = null;
            racunMoq2 = null;
        }

        [Test]
        public void ConstructorPositiveTest()
        {
            Assert.DoesNotThrow(() => new Transakcija(VrstaTransakcije.Isplata, racunMoq1.Object, 20));
            Assert.DoesNotThrow(() => new Transakcija(VrstaTransakcije.Uplata, racunMoq1.Object, 20));
            Assert.DoesNotThrow(() => new Transakcija(VrstaTransakcije.Prenos, racunMoq1.Object, racunMoq2.Object, 20));
        }

        [Test]
        public void ConstructorNegativeTest()
        {
            Assert.Throws<ArgumentNullException>(() => new Transakcija(VrstaTransakcije.Uplata, null, 20));
            Assert.Throws<InvalidTransactionException>(() => new Transakcija(VrstaTransakcije.Isplata, racunMoq1.Object, -20));
            Assert.Throws<InvalidTransactionException>(() => new Transakcija(VrstaTransakcije.Isplata, racunMoq1.Object, racunMoq2.Object, 20));
            Assert.Throws<InvalidTransactionException>(() => new Transakcija(VrstaTransakcije.Prenos, racunMoq1.Object, 20));
        }
    }
}
