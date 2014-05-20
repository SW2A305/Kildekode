using System;
using System.IO;
using System.Xml;
using McSntt.DataAbstractionLayer;
using McSntt.Models;

namespace McSntt.Helpers
{
    public class XmlMemberParser
    {
        private readonly ISailClubMemberDal _sailClubMemberDal;

        /// <summary>
        /// </summary>
        public XmlMemberParser() : this(DalLocator.SailClubMemberDal) {}

        /// <summary>
        /// </summary>
        /// <param name="dal"></param>
        public XmlMemberParser(ISailClubMemberDal dal)
        {
            this._sailClubMemberDal = dal;
        }

        /// <summary>
        ///     Imports members from an XML file that has been exported from an Access DB.
        /// </summary>
        /// <param name="xmlStream">The stream containing the XML data.</param>
        /// <remarks>
        ///     It might be a good idea to execute this on a separate thread, just to make it scale better. It won't be important
        ///     with only a few members, of course, but still.
        /// </remarks>
        public void ImportMembersFromXml(Stream xmlStream)
        {
            SailClubMember member = null;

            // Parse file
            using (XmlReader reader = XmlReader.Create(xmlStream))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        String tagName = reader.Name;

                        if (tagName == "tblMembers") {
                            member = new SailClubMember();
                        }
                        else if (member != null && reader.Read())
                        {
                            switch (tagName)
                            {
                                case "Id":
                                    member.SetMemberId(reader.Value.Trim());
                                    break;

                                case "FirstName":
                                    member.FirstName = reader.Value.Trim();
                                    break;

                                case "LastName":
                                    member.LastName = reader.Value.Trim();
                                    break;

                                case "Address":
                                    member.Address = reader.Value.Trim();
                                    break;

                                case "PostCode":
                                    member.Postcode = reader.Value.Trim();
                                    break;

                                case "CityName":
                                    member.Cityname = reader.Value.Trim();
                                    break;

                                case "Birthdate":
                                    member.DateOfBirth = reader.Value.Trim();
                                    break;

                                case "IsMale":
                                    member.Gender = reader.Value.Trim().Equals("0") ? Gender.Female : Gender.Male;
                                    break;
                            }
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "tblMembers")
                    {
                        // TODO Perhaps we should be checking for the return value here, as it will be false if something went wrong... But what should we do if it does?
                        this._sailClubMemberDal.Create(member);

                        member = null;
                    }
                }
            }
        }

        public void ImportMembersFromXml(String xmlFilePath)
        {
            this.ImportMembersFromXml(new FileStream(xmlFilePath, FileMode.Open, FileAccess.Read));
        }
    }
}
