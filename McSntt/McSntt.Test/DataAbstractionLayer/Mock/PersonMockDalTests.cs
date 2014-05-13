using System.Linq;
using McSntt.DataAbstractionLayer.Mock;
using McSntt.Models;
using NUnit.Framework;

namespace McSntt.Test.DataAbstractionLayer.Mock
{
    [TestFixture]
    public class PersonMockDalTests
    {
        private PersonMockDal _personMockDal;

        [SetUp]
        public void Initialize() { _personMockDal = new PersonMockDal(true); }

        [Test]
        public void Create_AddingOnePerson_UpdatesPersonId()
        {
            const int expectedPersonIdBefore = 0;
            var person = new Person()
                         {
                             FirstName = "Anna",
                             LastName = "Paquin",
                             Address = "Vampirestrasse 666",
                             Postcode = "1982",
                             Cityname = "Sparkleville",
                             BoatDriver = true,
                             DateOfBirth = "1805-09-15",
                             Gender = Gender.Female,
                             Email = "anna.p@vampires.vamp",
                             PhoneNumber = "11223344"
                         };

            Assert.AreEqual(expectedPersonIdBefore, person.PersonId);

            _personMockDal.Create(person);

            Assert.Greater(person.PersonId, expectedPersonIdBefore);
        }
    }
}