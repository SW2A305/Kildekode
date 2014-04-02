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
    }
}
