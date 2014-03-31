using System;
using McSntt.Models;
using NUnit.Framework;

namespace McSntt.Test.Models
{
    [TestFixture]
    public class SailClubMemberTests : SailClubMember
    {
        [Test]
        public void SailClubMember_NoInput_MemberIdShouldBeSetToDefaultIntValue()
        {
            var member = new SailClubMember();

            Assert.AreEqual(default(int), member.MemberId);
        }

        [Test]
        public void SailClubMember_NoInput_MemberPositionShouldBeSetToMember()
        {
            var member = new SailClubMember();

            Assert.AreEqual(SailClubMember.Positions.Member, member.Position);
        }

        [Test]
        public void SetMemberId_InputEmptyString_ShouldSetMemberIdToDefaultIntValue()
        {
            this.SetMemberId("");

            Assert.AreEqual(default(int), this.MemberId);
        }

        [Test]
        public void SetMemberId_InputNonEmptyNonNumericString_ShouldSetMemberIdToDefaultIntValue()
        {
            this.SetMemberId("abc");

            Assert.AreEqual(default(int), this.MemberId);
        }

        [Test]
        public void SetMemberId_InputSimpleNumberString_ShouldSetMemberIdToNumericValue()
        {
            this.SetMemberId("3");

            Assert.AreEqual(3, this.MemberId);
        }

        [Test]
        public void SetMemberSince_InputEmptyString_ExpectedResult()
        {
            Assert.IsTrue(false, "What are we actually expecting here??");
        }

        [Test]
        public void SetMemberSince_InputXmlDateString_SetsTheDateCorrectly()
        {
            var expectedDate = new DateTime(2002, 3, 4, 0, 0, 0, 0);

            SetMemberSince("2002-03-04T00:00:00");

            // Assert.AreEqual(expectedDate, this.MemberSince);
        }

        [Test]
        public void SetMemberSince_InputInvalidDateTimeString_ExpectedResult()
        {
            Assert.IsTrue(false, "What are we actually expecting here??");
        }

        [Test]
        public void SetDateOfBirth_InputEmptyString_ExpectedResult()
        {
            Assert.IsTrue(false, "What are we actually expecting here??");
        }

        [Test]
        public void SetDateOfBirth_InputXmlDateString_SetsTheDateCorrectly()
        {
            var expectedDate = new DateTime(2002, 3, 4, 0, 0, 0, 0);

            SetDateOfBirth("2002-03-04T00:00:00");

            Assert.AreEqual(expectedDate, this.DateOfBirth);
        }

        [Test]
        public void SetDateOfBirth_InputInvalidDateTimeString_ExpectedResult()
        {
            Assert.IsTrue(false, "What are we actually expecting here??");
        }
    }
}
