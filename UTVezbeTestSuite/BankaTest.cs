using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTVezbe;

namespace UTVezbeTestSuite
{
    [TestFixture]
    public class BankaTest
    {
        private Mock<IOsoba> vlasnikMock1;
        private IBanka testingObject;

        [SetUp]
        public void SetupTest()
        {
            vlasnikMock1 = new Mock<IOsoba>();
            vlasnikMock1.Setup(t => t.Equals(It.IsAny<IOsoba>())).Returns(true);
        }

        [Test]
        public void ConstructorTest()
        {
            Assert.DoesNotThrow(() => new Banka("234"));
        }

        [Test]
        public void ConstructorFailureTest()
        {
            Assert.Throws<ArgumentException>(() => new Banka("as1"));
        }

        [Test]
        public void NoviRacunPositiveTest()
        {
            testingObject = new Banka("123");
            testingObject.NoviRacun(vlasnikMock1.Object, 1000);
            Assert.IsTrue(testingObject.Racuni.ContainsKey(vlasnikMock1.Object));
        }
    }
}
