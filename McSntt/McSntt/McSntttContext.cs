using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Odbc;
using System.Xml;
using McSntt.Models;

namespace McSntt
{
    internal class McSntttContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<SailClubMember> SailClubMembers { get; set; }

        /// <summary>
        ///     Imports members from an XML file that has been exported from an Access DB.
        /// </summary>
        /// <param name="xmlFilePath">The path of the XML file to import from.</param>
        /// <remarks>
        ///     It might be a good idea to execute this on a separate thread, just to make it scale better. It won't be important
        ///     with only a few members, of course, but still.
        /// </remarks>
        public void ImportMembersFromXmlFile(String xmlFilePath)
        {
            SailClubMember member = null;
            String tagName;

            // Just for good measure
            this.SailClubMembers.Load();

            // Parse file
            using (XmlReader reader = XmlReader.Create(xmlFilePath))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        tagName = reader.Name;

                        if (tagName == "tblMembers")
                        {
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

                                case "MemberSince":
                                    member.SetMemberSince(reader.Value.Trim());
                                    break;

                                case "Birthdate":
                                    member.SetDateOfBirth(reader.Value.Trim());
                                    break;

                                case "IsMale":
                                    member.Gender = reader.Value.Trim().Equals("0") ? Gender.Female : Gender.Male;
                                    break;
                            }
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "tblMembers")
                    {
                        this.SailClubMembers.AddOrUpdate(m => m.MemberId, member);

                        member = null;
                    }
                }
            }
        }
    }
}
