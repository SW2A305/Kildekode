using System.Linq;
using McSntt.DataAbstractionLayer;
using NUnit.Framework;

namespace McSntt.Test.DataAbstractionLayer
{
    [TestFixture]
    public class SailClubMemberEfDalTests
    {
        private SailClubMemberEfDal _sailClubMemberEfDal;

        [SetUp]
        public void Initialize() { _sailClubMemberEfDal = new SailClubMemberEfDal(); }

        [Test]
        public void Crew_NoInput_CheckTheCount()
        {
            var scm = _sailClubMemberEfDal.GetAll().FirstOrDefault();

            if (scm == null) { Assert.Fail("It's dead, Jim!");} else {
            Assert.Fail("Count: {0} / {2} [{3}] ({1})", scm.PartOfCrewOn.Count, scm.PersonId, scm.CaptainOn.Count, scm.CaptainOn.GetType().Name);}
        }
    }
}