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
                    Address = "Scoresbysundvej 8",
                    Postcode = "9210",
                    Cityname = "Aalborg SØ",
                    Email = "HalloHallo@gmail.com",
                    PhoneNumber = "12345678",
                    Gender = Gender.Male,
                    MemberId = 1337,
                    Position = SailClubMember.Positions.Admin
                }
                );

            context.SailClubMembers.AddOrUpdate(
                p => p.Username,
                new SailClubMember
                {
                    FirstName = "Søren",
                    LastName = "Kroegh",
                    Address = "Scoresbysundvej 8",
                    Postcode = "9000",
                    Cityname = "Aalborg SØ",
                    Email = "HalloHallo@gmail.com",
                    PhoneNumber = "12345678",
                    Gender = Gender.Male,
                    MemberId = 1337,
                    Position = SailClubMember.Positions.Admin
                }
                );
        }
    }
}
