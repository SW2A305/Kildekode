using System.Linq;
using McSntt.DataAbstractionLayer;
using McSntt.DataAbstractionLayer.Mock;
using McSntt.Helpers;
using NUnit.Framework;

namespace McSntt.Test.Helpers
{
    [TestFixture]
    public class DbSeedDataTests
    {
        private BoatMockDal _boatDal;
        private EventMockDal _eventDal;
        private LectureMockDal _lectureDal;
        private LogbookMockDal _logbookDal;
        private PersonMockDal _personDal;
        private RegularTripMockDal _tripDal;
        private SailClubMemberMockDal _scmDal;
        private StudentMemberMockDal _smDal;
        private TeamMockDal _teamDal;

        [SetUp]
        public void Initialize()
        {
            DbSeedData.CreateSeedData(true);

            // Instantiate various DALs, not calling them as "useForTests", 
            _boatDal = new BoatMockDal();
            _eventDal = new EventMockDal();
            _lectureDal = new LectureMockDal();
            _logbookDal = new LogbookMockDal();
            _personDal = new PersonMockDal();
            _tripDal = new RegularTripMockDal();
            _scmDal = new SailClubMemberMockDal();
            _smDal = new StudentMemberMockDal();
            _teamDal = new TeamMockDal();
        }

        [Test]
        public void CreateSeedData_Boats_ThereShouldBeFive()
        {
            const int expectedCount = 5;

            Assert.AreEqual(expectedCount, _boatDal.GetAll().Count());
        }

        [Test]
        public void CreateSeedData_Events_ThereShouldBeSix()
        {
            const int expectedCount = 6;
            
            Assert.AreEqual(expectedCount, _eventDal.GetAll().Count());
        }

        [Test]
        public void CreateSeedData_Persons_ThereShouldBeTwentyThree()
        {
            const int expectedCount = 23;
            
            Assert.AreEqual(expectedCount, _personDal.GetAll().Count());
        }

        [Test]
        public void CreateSeedData_SailClubMember_AllShouldHavePersonId()
        {
            const int noIdSet = 0;
            var members = _scmDal.GetAll();

            foreach (var sailClubMember in members) {
                Assert.Greater(sailClubMember.PersonId, noIdSet);
            }
        }

        [Test]
        public void CreateSeedData_StudentMember_AllShouldHavePersonId()
        {
            const int noIdSet = 0;
            var students = _smDal.GetAll();

            foreach (var studentMember in students) {
                Assert.Greater(studentMember.PersonId, noIdSet);
            }
        }

        [Test]
        public void CreateSeedData_StudentMember_AllShouldHaveSailClubMemberId()
        {
            const int noIdSet = 0;
            var students = _smDal.GetAll();

            foreach (var studentMember in students) {
                Assert.Greater(studentMember.SailClubMemberId, noIdSet);
            }
        }

        [Test]
        public void CreateSeedData_Team_ShouldHaveTwo()
        {
            const int expectedCount = 2;

            Assert.AreEqual(expectedCount, _teamDal.GetAll().Count());
        }

        [Test]
        public void CreateSeedData_Team_ShouldHaveFourOrMoreTeamMembers()
        {
            const int minimumTeamMemberCount = 4;
            var teams = _teamDal.GetAll();

            _teamDal.LoadData(teams);

            foreach (var team in teams) {
                Assert.NotNull(team.TeamMembers);
                Assert.GreaterOrEqual(team.TeamMembers.Count, minimumTeamMemberCount);
            }
        }

        [Test]
        public void CreateSeedData_Team_ShouldHaveLecturesSet()
        {
            var teams = _teamDal.GetAll();
            
            _teamDal.LoadData(teams);

            foreach (var team in teams) {
                Assert.NotNull(team.Lectures);
                Assert.Greater(team.Lectures.Count, 0);
            }
        }
    }
}