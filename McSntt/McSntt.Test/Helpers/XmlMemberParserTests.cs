using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using McSntt.DataAbstractionLayer;
using McSntt.Helpers;
using McSntt.Models;
using NSubstitute;
using NUnit.Framework;

namespace McSntt.Test.Helpers
{
    [TestFixture]
    public class XmlMemberParserTests : XmlMemberParser
    {
        private ISailClubMemberDal _memberDalSubstitute;
        private List<String> _xmlStrings;
        private Stream _stream;
        private StreamWriter _streamWriter;
        private XmlMemberParser _parser;
        private List<SailClubMember> _sailClubMemberList;

        [SetUp]
        public void Initialize()
        {
            _memberDalSubstitute = Substitute.For<ISailClubMemberDal>();
            _parser = new XmlMemberParser(_memberDalSubstitute);
            _sailClubMemberList = new List<SailClubMember>();

            // React to "creation" calls
            _memberDalSubstitute
                .Create(Arg.Any<SailClubMember>())
                .ReturnsForAnyArgs(true)
                .AndDoes(x => _sailClubMemberList.AddRange((SailClubMember[])x[0]));
        }

        [TearDown]
        public void DeInitialize()
        {
            _memberDalSubstitute = null;
            _parser = null;
            _stream = null;
            _streamWriter = null;
            _xmlStrings = null;
        }

        private void InitializeXmlStrings1()
        {
            _stream = new MemoryStream();
            _streamWriter = new StreamWriter(_stream);

            _xmlStrings = new List<string>
                          {
                              "<?xml version=\"1.0\" encoding=\"UTF-8\"?>",
                              "<dataroot xmlns:od=\"urn:schemas-microsoft-com:officedata\" generated=\"2014-03-30T17:24:08\">",
                              "<tblMembers>",
                              "<Id>1</Id>",
                              "<FirstName>Anna</FirstName>",
                              "<LastName>Thorsen</LastName>",
                              "<Address>Vesterskovvej 57</Address>",
                              "<PostCode>8570</PostCode>",
                              "<CityName>Trustrup</CityName>",
                              "<MemberSince>2012-08-08T00:00:00</MemberSince>",
                              "<Birthdate>1994-04-05T00:00:00</Birthdate>",
                              "<IsMale>0</IsMale>",
                              "</tblMembers>",
                              "</dataroot>"
                          };

            foreach (var xmlString in _xmlStrings)
            {
                _streamWriter.WriteLine(xmlString);
            }

            _streamWriter.Flush();
            _stream.Seek(0, SeekOrigin.Begin);
        }

        [Test]
        public void Constructor_NoParameters_CanInstantiate()
        {
            var parser = new XmlMemberParser();

            Assert.NotNull(parser);
        }

        [Test]
        public void Constructor_OneParameter_CanInstantiate()
        {
            var parser = new XmlMemberParser(_memberDalSubstitute);

            Assert.NotNull(parser);
        }

        [Test]
        public void ImportMembersFromXml_InputtingOneMember_OneMemberSaved()
        {
            this.InitializeXmlStrings1();
            _parser.ImportMembersFromXml(_stream);

            _memberDalSubstitute.Received(1).Create(Arg.Any<SailClubMember>());
        }

        [Test]
        public void ImportMembersFromXml_InputtingOneMember_ParsesCorrectId()
        {
            this.InitializeXmlStrings1();
            _parser.ImportMembersFromXml(_stream);
            
            Assert.AreEqual(1, _sailClubMemberList[0].MemberId);
        }

        [Test]
        public void ImportMembersFromXml_InputtingOneMember_ParsesCorrectFirstName()
        {
            this.InitializeXmlStrings1();
            _parser.ImportMembersFromXml(_stream);

            Assert.AreEqual("Anna", _sailClubMemberList[0].FirstName);
        }

        [Test]
        public void ImportMembersFromXml_InputtingOneMember_ParsesCorrectLastName()
        {
            this.InitializeXmlStrings1();
            _parser.ImportMembersFromXml(_stream);

            Assert.AreEqual("Thorsen", _sailClubMemberList[0].LastName);
        }

        [Test]
        public void ImportMembersFromXml_InputtingOneMember_ParsesCorrectAddress()
        {
            this.InitializeXmlStrings1();
            _parser.ImportMembersFromXml(_stream);

            Assert.AreEqual("Vesterskovvej 57", _sailClubMemberList[0].Address);
        }

        [Test]
        public void ImportMembersFromXml_InputtingOneMember_ParsesCorrectPostCode()
        {
            this.InitializeXmlStrings1();
            _parser.ImportMembersFromXml(_stream);

            Assert.AreEqual("8570", _sailClubMemberList[0].Postcode);
        }

        [Test]
        public void ImportMembersFromXml_InputtingOneMember_ParsesCorrectCityName()
        {
            this.InitializeXmlStrings1();
            _parser.ImportMembersFromXml(_stream);

            Assert.AreEqual("Trustrup", _sailClubMemberList[0].Cityname);
        }

        [Test]
        public void ImportMembersFromXml_InputtingOneMember_ParsesCorrectBirthdate()
        {
            this.InitializeXmlStrings1();
            _parser.ImportMembersFromXml(_stream);

            Assert.AreEqual("1994-04-05T00:00:00", _sailClubMemberList[0].DateOfBirth);
        }

        [Test]
        public void ImportMembersFromXml_InputtingOneMember_ParsesCorrectGender()
        {
            this.InitializeXmlStrings1();
            _parser.ImportMembersFromXml(_stream);

            Assert.AreEqual(Gender.Female, _sailClubMemberList[0].Gender);
        }
    }
}
