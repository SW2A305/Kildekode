using McSntt.DataAbstractionLayer;
using McSntt.Helpers;
using NSubstitute;
using NUnit.Framework;

namespace McSntt.Test.Helpers
{
    [TestFixture]
    public class XmlMemberParserTests : XmlMemberParser
    {
        private ISailClubMemberDal _memberDalSubstitute;

        [Test]
        public void Constructor_NoParameters_CanInstantiate()
        {
            var parser = new XmlMemberParser();

            Assert.NotNull(parser);
        }

        [Test]
        public void Constructor_OneParameter_CanInstantiate()
        {
            var memberDalSubstitute = Substitute.For<ISailClubMemberDal>();
            var parser = new XmlMemberParser(memberDalSubstitute);

            Assert.NotNull(parser);
        }

        [Test]
        public void ImportMembersFromXml_InputtingOneMember_OneMemberSaved()
        {
            
        }
    }
}
