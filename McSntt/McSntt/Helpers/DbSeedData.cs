using System;
using McSntt.Models;
using Xceed.Wpf.DataGrid;

namespace McSntt.Helpers
{
    public class DbSeedData
    {
        public static void CreateSeedData()
        {
            #region Arrays
            #region DataArray :: Boats
            var boats = new[]
                        {
                            new Boat()
                            {
                                NickName = "Anastasia",
                                Type = BoatType.Gaffelrigger,
                                ImagePath = "Gaffelrigger.jpg",
                                Operational = true
                            },

                            new Boat()
                            {
                                NickName = "Anna",
                                Type = BoatType.Drabant,
                                ImagePath = "Drabant.jpg",
                                Operational = true
                            },

                            new Boat()
                            {
                                NickName = "Isabella",
                                Type = BoatType.J80,
                                ImagePath = "J80.jpg",
                                Operational = false
                            },

                            new Boat()
                            {
                                NickName = "Kennara",
                                Type = BoatType.Mini12,
                                ImagePath = "Mini12.jpg",
                                Operational = true
                            }, 

                            new Boat()
                            {
                                NickName = "Trine",
                                Type = BoatType.Spækhugger,
                                ImagePath = "Spækhugger.jpg",
                                Operational = true
                            }
                        };
            #endregion

            #region DataArray :: Events
            var events = new Event[]
                         {
                             new Event()
                             {
                                 Created = true,
                                 Description = "Vi spiser Pizza ved Molen",
                                 EventDate = DateTime.Now.AddDays(2),
                                 EventTitle = "PizzaParty",
                                 SignUpReq = true,
                             },

                             new Event()
                             {
                                 Created = true,
                                 Description = "Der er fest klokken 23:00. Klæd jer ud som forskellige bådtyper! Det bliver både sjovt og hyggeligt!",
                                 EventDate = DateTime.Now.AddDays(2),
                                 EventTitle = "Fest i Klubhuset",
                                 SignUpReq = true,
                             }
                         };
            #endregion

            #region DataArray :: Lectures
            #endregion

            #region DataArray :: Logbooks
            #endregion

            #region DataArray :: Persons
            #endregion

            #region DataArray :: RegularTrips
            var trips = new RegularTrip[]
                        {
                            new RegularTrip()
                            {
                                PurposeAndArea = "Plyndre England, husk nattøj",
                                WeatherConditions = "12 m/s fra vest, overskyet",
                                DepartureTime = new DateTime(2014, 05, 12, 13, 0, 0),
                                ArrivalTime = new DateTime(2014, 05, 15, 19, 0, 0)
                            },
                            new RegularTrip()
                            {
                                PurposeAndArea = "Heldagstur",
                                WeatherConditions = "2 m/s fra nord, høj sol",
                                DepartureTime = new DateTime(2014, 05, 14, 9, 0, 0),
                                ArrivalTime = new DateTime(2014, 05, 14, 21, 0, 0)
                            },
                            new RegularTrip()
                            {
                                PurposeAndArea = "Fredag den 13, en spooky tur!",
                                WeatherConditions = "4 m/s, Fuldmåne",
                                DepartureTime = new DateTime(2014, 06, 13, 21, 0, 0),
                                ArrivalTime = new DateTime(2014, 06, 14, 08, 0, 0)
                            },
                            new RegularTrip()
                            {
                                PurposeAndArea = "Torsdag den 12, meget freligt",
                                WeatherConditions = "4 m/s, høj sol.",
                                DepartureTime = new DateTime(2014, 06, 12, 13, 0, 0),
                                ArrivalTime = new DateTime(2014, 06, 14, 17, 0, 0)
                            },
                            new RegularTrip()
                            {
                                PurposeAndArea = "Generobre skåne",
                                WeatherConditions = "6 m/s fra vest, klar himmel",
                                DepartureTime = new DateTime(2014, 05, 21, 21, 0, 0),
                                ArrivalTime = new DateTime(2014, 05, 23, 08, 0, 0)
                            }
                        };
            #endregion

            #region DataArray :: SailClubMembers
            var sailClubMembers = new[]
            {
                new SailClubMember
                {
                    FirstName = "Oskar",
                    LastName = "Lauridsen",
                    Address = "Skovvej 15",
                    Postcode = "1755",
                    Cityname = "København V",
                    BoatDriver = true,
                    Email = "OskarLauridsen@armyspy.com",
                    Gender = Gender.Male,
                    PhoneNumber = "22879720",
                    Username = "oskar",
                    PasswordHash = EncryptionHelper.Sha256("lauridsen"),
                    Position = SailClubMember.Positions.Admin,
                    DateOfBirth = "1963-06-06"
                },
                new SailClubMember
                {
                    FirstName = "Katrine",
                    LastName = "Holm",
                    Address = "Borgmester Hansensvej 20",
                    Postcode = "8870",
                    Cityname = "Langå",
                    BoatDriver = true,
                    Email = "KatrineHolm@armyspy.com",
                    Gender = Gender.Female,
                    PhoneNumber = "31503622",
                    Username = "katrine",
                    PasswordHash = EncryptionHelper.Sha256("holm"),
                    Position = SailClubMember.Positions.Member,
                    DateOfBirth = "1984-03-25"
                }
            };
            #endregion

            #region DataArray :: StudentMembers
            var studentMembers = new[]
            {
                new StudentMember
                {
                    FirstName = "Michelle",
                    LastName = "Kristiansen",
                    Address = "Møllevænget 31",
                    Postcode = "3500",
                    Cityname = "Værløse",
                    BoatDriver = false,
                    Email = "MichelleKristiansen@dayrep.com",
                    Gender = Gender.Female,
                    PhoneNumber = "50301773",
                    Username = "michelle",
                    PasswordHash = EncryptionHelper.Sha256("kristensen"),
                    DateOfBirth = "1998-10-26"
                },
                new StudentMember
                {
                    FirstName = "Nikolaj",
                    LastName = "Berg",
                    Address = "Møllebakken 89",
                    Postcode = "6840",
                    Cityname = "Oksbøl",
                    BoatDriver = false,
                    Email = "NikolajBerg@rhyta.com",
                    Gender = Gender.Male,
                    PhoneNumber = "52828168",
                    Username = "nikolaj",
                    PasswordHash = EncryptionHelper.Sha256("berg"),
                    DateOfBirth = "1996-08-10"
                },
                new StudentMember
                {
                    FirstName = "Clara",
                    LastName = "Kjeldsen",
                    Address = "Sønderstræde 57",
                    Postcode = "1706",
                    Cityname = "København V",
                    BoatDriver = false,
                    Email = "ClaraKjeldsen@armyspy.com",
                    Gender = Gender.Female,
                    PhoneNumber = "52320020",
                    Username = "clara",
                    PasswordHash = EncryptionHelper.Sha256("kjeldsen"),
                    DateOfBirth = "1998-01-31"
                },
                new StudentMember
                {
                    FirstName = "Christoffer",
                    LastName = "Bak",
                    Address = "Margrethes Plads 96",
                    Postcode = "8723",
                    Cityname = "Løsning",
                    BoatDriver = false,
                    Email = "ChristofferBak@jourrapide.com",
                    Gender = Gender.Male,
                    PhoneNumber = "61872005",
                    Username = "christoffer",
                    PasswordHash = EncryptionHelper.Sha256("bak"),
                    DateOfBirth = "1996-06-29"
                },
                new StudentMember
                {
                    FirstName = "Katrine",
                    LastName = "Jensen",
                    Address = "Lersey Allé 56",
                    Postcode = "1240",
                    Cityname = "København K",
                    BoatDriver = false,
                    Email = "KatrineJensen@jourrapide.com",
                    Gender = Gender.Female,
                    PhoneNumber = "52539214",
                    Username = "christoffer",
                    PasswordHash = EncryptionHelper.Sha256("bak"),
                    DateOfBirth = "1996-06-29"
                },
                new StudentMember
                {
                    FirstName = "Philip",
                    LastName = "Olsen",
                    Address = "Sibiriensvej 51",
                    Postcode = "4000",
                    Cityname = "Roskilde",
                    BoatDriver = false,
                    Email = "PhilipTOlsen@teleworm.us",
                    Gender = Gender.Male,
                    PhoneNumber = "26537537",
                    Username = "philip",
                    PasswordHash = EncryptionHelper.Sha256("olsen"),
                    DateOfBirth = "1998-08-02"
                }
            };
            #endregion

            #region DataArray :: Teams
            #endregion
            #endregion

            #region Data linking
            #endregion
        }
    }
}