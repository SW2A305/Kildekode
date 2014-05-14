using System.Collections.Generic;
using System.Linq;
using McSntt.DataAbstractionLayer.Mock;
using McSntt.Models;
using NUnit.Framework;

namespace McSntt.Test.DataAbstractionLayer.Mock
{
    [TestFixture]
    public class PersonMockDalTests
    {
        [SetUp]
        public void Initialize()
        {
            this._personMockDal = new PersonMockDal(true);
            this._persons = new List<Person>
                            {
                                new Person
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
                                },

                                new Person
                                {
                                    FirstName = "Søren",
                                    LastName = "Have",
                                    Address = "Lunden 1",
                                    Postcode = "9879",
                                    Cityname = "Kedeligborg",
                                    BoatDriver = false,
                                    DateOfBirth = "1976-04-26",
                                    Gender = Gender.Male,
                                    Email = "soeren@have.name",
                                    PhoneNumber = "44332211"
                                }
                            };
        }

        private PersonMockDal _personMockDal;
        private List<Person> _persons;

        [Test]
        public void Create_AddingOnePerson_UpdatesPersonId()
        {
            const int expectedPersonIdBefore = 0;

            Assert.AreEqual(expectedPersonIdBefore, _persons[0].PersonId);

            this._personMockDal.Create(_persons[0]);

            Assert.Greater(_persons[0].PersonId, expectedPersonIdBefore);
        }

        [Test]
        public void Create_AddingTwoPersons_AddsTwoPersonsToTheStorage()
        {
            var countBefore = _personMockDal.GetAll().Count();
            var expectedCount = countBefore + 2;

            _personMockDal.Create(_persons[0], _persons[1]);

            var countAfter = _personMockDal.GetAll().Count();

            Assert.AreEqual(expectedCount, countAfter);
        }

        [Test]
        public void GetOne_AddingOnePerson_ReturnsIdenticalPerson()
        {
            var personAdded = _persons[0];

            _personMockDal.Create(personAdded);

            var personFetched = _personMockDal.GetOne(personAdded.PersonId);

            Assert.AreEqual(personAdded.PersonId, personFetched.PersonId);
            Assert.AreEqual(personAdded.Address, personFetched.Address);
            Assert.AreEqual(personAdded.BoatDriver, personFetched.BoatDriver);
            Assert.AreEqual(personAdded.Cityname, personFetched.Cityname);
            Assert.AreEqual(personAdded.DateOfBirth, personFetched.DateOfBirth);
            Assert.AreEqual(personAdded.Email, personFetched.Email);
            Assert.AreEqual(personAdded.FirstName, personFetched.FirstName);
            Assert.AreEqual(personAdded.LastName, personFetched.LastName);
            Assert.AreEqual(personAdded.Gender, personFetched.Gender);
            Assert.AreEqual(personAdded.PhoneNumber, personFetched.PhoneNumber);
            Assert.AreEqual(personAdded.Postcode, personFetched.Postcode);
        }
    }
}
