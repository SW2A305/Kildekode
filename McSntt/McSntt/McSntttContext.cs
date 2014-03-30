using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
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

            // Just for good measure
            this.SailClubMembers.Load();

            // Parse file
            using (XmlReader reader = XmlReader.Create(xmlFilePath))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "tblMembers": // Start of an entry
                                member = new SailClubMember();
                                break;

                            case "Id":
                                if (member != null && reader.Read())
                                {
                                }
                                break;

                            case "FirstName":
                                break;

                            case "LastName":
                                break;

                            case "Address":
                                break;

                            case "PostCode":
                                break;

                            case "CityName":
                                break;

                            case "MemberSince":
                                break;

                            case "Birthdate":
                                break;

                            case "IsMale":
                                break;
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
