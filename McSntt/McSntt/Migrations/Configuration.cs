using System;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations;
using McSntt.Helpers;
using System.Collections.Generic;
using McSntt.Models;

namespace McSntt.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<McSntttContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = false;
            this.ContextKey = "McSntt.McSntttContext";
        }

        protected override void Seed(McSntttContext context)
        {
            //  This method will be called after migrating to the latest version.

            #region Data : Persons
            var persons = new[]
                          {
                              new Person
                              {
                                  FirstName = "Chesty",
                                  LastName = "Duvall",
                                  Address = "Overgade 666",
                                  Postcode = "1666",
                                  Cityname = "Hell-erup",
                                  BoatDriver = false,
                                  DateOfBirth = "1973-05-18",
                                  Email = "chesty.duvall@doesnotexi.st",
                                  Gender = Gender.Female,
                                  PhoneNumber = "11223345"
                              }
                          };
            #endregion

            #region Data : SailClubMembers
            var sailClubMembers = new[]
                                  {
                                      new SailClubMember
                                      {
                                          SailClubMemberId = 276,
                                          FirstName = "John",
                                          LastName = "Filbert",
                                          Address = "Nederensgade 10",
                                          Postcode = "1200",
                                          Cityname = "Etsted",
                                          BoatDriver = true,
                                          Email = "jh247f@stashit.sck",
                                          Gender = Gender.Male,
                                          PhoneNumber = "11223344",
                                          Username = "jfilbert",
                                          PasswordHash = EncryptionHelper.Sha256("God"),
                                          Position = SailClubMember.Positions.Member,
                                          DateOfBirth = "1989-12-24"
                                      },
                                      new SailClubMember
                                      {
                                          SailClubMemberId = 23,
                                          FirstName = "Jonna",
                                          LastName = "Gored",
                                          Address = "Hovedgaden 1",
                                          Postcode = "1004",
                                          Cityname = "Etandetsted",
                                          BoatDriver = false,
                                          Email = "jg1803@stashthistoo.ccf",
                                          Gender = Gender.Female,
                                          PhoneNumber = "11223346",
                                          Username = "jgored",
                                          PasswordHash = EncryptionHelper.Sha256("password"),
                                          Position = SailClubMember.Positions.Admin,
                                          DateOfBirth = "1976-01-31"
                                      }
                                  };
            #endregion

            #region Data : Boats
            var boats = new[]
                        {
                            new Boat
                            {
                                ImagePath = "SundetLogo.png",
                                NickName = "Sinky",
                                Operational = false,
                                Type = BoatType.Drabant
                            },
                            new Boat
                            {
                                ImagePath = "SundetLogo.png",
                                NickName = "Anna",
                                Operational = true,
                                Type = BoatType.Gaffelrigger
                            }
                        };
            #endregion

            #region Data : Logbooks
            var logbooks = new[]
                           {
                               new Logbook()
                               {
                                   ActualArrivalTime = DateTime.Now.AddDays(2).AddHours(6).AddMinutes(13),
                                   ActualCrew = new Collection<Person> {persons[0], sailClubMembers[1]},
                                   ActualDepartureTime = DateTime.Now.AddDays(2).AddHours(3).AddMinutes(30),
                                   AnswerFromBoatChief = "This is my answer!",
                                   DamageDescription = "Massive hole in the side.",
                                   DamageInflicted = true,
                                   FiledBy = sailClubMembers[0]
                               }
                           };
            #endregion

            #region Data : RegularTrips
            var regularTrips = new[]
                               {
                                   new RegularTrip
                                   {
                                       Boat = boats[1],
                                       Captain = sailClubMembers[0],
                                       DepartureTime = DateTime.Now.AddDays(2),
                                       ExpectedArrivalTime = DateTime.Now.AddDays(2).AddHours(5),
                                       Crew = new Collection<Person> {persons[0], sailClubMembers[1], sailClubMembers[0]},
                                       PurposeAndArea = "Terrorisme",
                                       WeatherConditions = "Cloudy with a chance of bullet rain.",
                                       Logbook = logbooks[0],
                                       ArrivalTime = logbooks[0].ActualArrivalTime,
                                       Comments = "With great comments... Something something..."
                                   }
                               };
            #endregion

            // Insert data into database
            context.Persons.AddOrUpdate(p => new {p.FirstName, p.LastName, p.PhoneNumber}, persons);
            context.SailClubMembers.AddOrUpdate(s => new {s.FirstName, s.LastName, s.PhoneNumber}, sailClubMembers);
            context.Boats.AddOrUpdate(b => b.NickName, boats);
            context.Logbooks.AddOrUpdate(l => new {l.DamageInflicted, l.DamageDescription}, logbooks);
            context.RegularTrips.AddOrUpdate(r => new {r.PurposeAndArea, r.WeatherConditions}, regularTrips);
        }
    }
}
