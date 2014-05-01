using McSntt.Models;

namespace McSntt.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<McSntt.McSntttContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "McSntt.McSntttContext";
        }

        protected override void Seed(McSntt.McSntttContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            

            context.SailClubMembers.AddOrUpdate(
                p => p.Username,
                new SailClubMember
                {
                    FirstName = "Andreas M.",
                    LastName = "Karlsen",
                    Email = "AndreasMKarlsen@rhyta.com",
                    Username = "Aa",
                    DateOfBirth = "1994-06-13",
                    PasswordHash = "9c478bf63e9500cb5db1e85ece82f18c8eb9e52e2f9135acd7f10972c8d563ba"

                }
                );

            context.SailClubMembers.AddOrUpdate(
                p => p.Username,
                new SailClubMember
                {
                    FirstName = "Troels",
                    LastName = "Kroegh",
                    Username = "Røde",
                    Address = "Scoresbysundvej 8",
                    Postcode = "9210",
                    Cityname = "Aalborg SØ",
                    Email = "HalloHallo@gmail.com",
                    PhoneNumber = "12345678",
                    Gender = Gender.Male,
                    MemberId = 1339,
                    Position = SailClubMember.Positions.Member,
                    PasswordHash = "aaa9402664f1a41f40ebbc52c9993eb66aeb366602958fdfaa283b71e64db123" // sha256(mcsntt)
                }
                );

            context.SailClubMembers.AddOrUpdate(
                p => p.Username,
                new SailClubMember
                {
                    FirstName = "Søren",
                    LastName = "Kroegh",
                    Username = "Trampe",
                    Address = "Scoresbysundvej 8",
                    Postcode = "9000",
                    Cityname = "Aalborg SØ",
                    Email = "HalloHallo@gmail.com",
                    PhoneNumber = "12345678",
                    Gender = Gender.Female,
                    MemberId = 1338,
                    Position = SailClubMember.Positions.Admin,
                    PasswordHash = "aaa9402664f1a41f40ebbc52c9993eb66aeb366602958fdfaa283b71e64db123" // sha256(h)

                }
                );

            context.RegularTrips.AddOrUpdate(
                t => t.RegularTripId,
                new RegularTrip
                {
                     Boat = context.Boats.First(x => x.NickName == "Bodil"),
                     ArrivalTime = new DateTime(2014,09,9,12,0,0),
                     BoatId = 1,
                     Captain = context.SailClubMembers.First(x => x.FirstName == "Søren"),
                     Comments = "Det blir sjaw!",
                     Crew = context.SailClubMembers.Where(x=> x.Position == SailClubMember.Positions.Admin).Cast<Person>().ToList(),
                     DepartureTime = new DateTime(2014,09,9,09,0,0),
                     PurposeAndArea = "u' ti' æ ' van' og' hjem' ien...",
                     WeatherConditions = "Det 'en bæt' wind...",
                     RegularTripId = 9
                }
                );

            context.Boats.AddOrUpdate(
                b => b.Id,
                new Boat
                {
                    NickName = "Bodil",
                    Type = BoatType.Drabant,
                    Operational = true,
                    ImagePath = "Images/SundetLogo.png"
                }
                );
        }
    }
}
