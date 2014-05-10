using System;
using System.Collections.Generic;
using McSntt.Models;

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
                             },

                             new Event()
                             {
                                 Created = true,
                                 Description = "Generalforsamling i Klubhuset med valg af ny formand og godkendelse af budget.",
                                 EventDate = DateTime.Now.AddDays(10),
                                 EventTitle = "Generalforsamling i Klubhuset",
                                 SignUpReq = true,
                             },

                             new Event()
                             {
                                 Created = true,
                                 Description = "Afslapningsøl efter kapsejlads, taberen giver!",
                                 EventDate = DateTime.Now.AddDays(12),
                                 EventTitle = "Øl efter kapsejlads",
                                 SignUpReq = false,
                             },

                             new Event()
                             {
                                 Created = true,
                                 Description = "Reparation af nyindkøbt J80'er.",
                                 EventDate = DateTime.Now.AddDays(15),
                                 EventTitle = "Bådreparation",
                                 SignUpReq = false,
                             },

                             new Event()
                             {
                                 Created = true,
                                 Description = "Der er åbent hus med brød og pølser samt øl og vand til medlemmer og gæster",
                                 EventDate = DateTime.Now.AddDays(20),
                                 EventTitle = "Åbent hus",
                                 SignUpReq = false,
                             }
                         };
            #endregion

            #region DataArray :: Lectures
            var lectures = new Lecture[]
                         {
                             new Lecture()
                             {
                                 DateOfLecture = new DateTime(2014, 5, 9, 15, 00, 00),
                                 RopeWorksLecture = true,
                                 Drabant = true,
                                 Gaffelrigger = false,
                                 Motor = false,
                                 Navigation = false,
                                 Night = false,
                             },

                             new Lecture()
                             {
                                 DateOfLecture = new DateTime(2014, 5, 21, 12, 30, 00),
                                 RopeWorksLecture = false,
                                 Drabant = false,
                                 Gaffelrigger = true,
                                 Motor = true,
                                 Navigation = false,
                                 Night = false,
                             },

                             new Lecture()
                             {
                                 DateOfLecture = new DateTime(2014, 5, 30, 23, 55, 00),
                                 RopeWorksLecture = false,
                                 Drabant = false,
                                 Gaffelrigger = true,
                                 Motor = false,
                                 Navigation = true,
                                 Night = true
                             }  
                         };
            #endregion

            #region DataArray :: Logbooks

            var logbooks = new Logbook[]
            {
                new Logbook()
                {
                    ActualArrivalTime = new DateTime(2014, 05, 08, 19, 0, 0),
                    ActualDepartureTime = new DateTime(2014, 05, 07, 9, 0, 0),
                    AnswerFromBoatChief = "Der bliver udbedret i løbet af en uge, båden er stadig operationel",
                    DamageDescription = "Nogle bræder er flækket ved styrbord",
                    DamageInflicted = true,
                },

                new Logbook()
                {
                    ActualArrivalTime = new DateTime(2014, 05, 09, 21, 0, 0),
                    ActualDepartureTime = new DateTime(2014, 05, 07, 9, 0, 0),
                    DamageInflicted = false,
                }
            };
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
                                DepartureTime = new DateTime(2014, 05, 07, 13, 0, 0),
                                ArrivalTime = new DateTime(2014, 05, 08, 19, 0, 0)
                            },
                            new RegularTrip()
                            {
                                PurposeAndArea = "Heldagstur",
                                WeatherConditions = "2 m/s fra nord, høj sol",
                                DepartureTime = new DateTime(2014, 05, 07, 9, 0, 0),
                                ArrivalTime = new DateTime(2014, 05, 09, 21, 0, 0)
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
                                DepartureTime = new DateTime(2014, 05, 08, 21, 0, 0),
                                ArrivalTime = new DateTime(2014, 05, 09, 08, 0, 0)
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
                },
                 new SailClubMember
                {
                    FirstName = "Kasper",
                    LastName = "Eriksen",
                    Address = "Hovedvej 33",
                    Postcode = "8210",
                    Cityname = "Århus",
                    BoatDriver = true,
                    Email = "KasperEriksen@armyspy.com",
                    Gender = Gender.Male,
                    PhoneNumber = "88888888",
                    Username = "kasper",
                    PasswordHash = EncryptionHelper.Sha256("eriksen"),
                    Position = SailClubMember.Positions.Member,
                    DateOfBirth = "1979-01-01"
                },
                 new SailClubMember
                {
                    FirstName = "Lars",
                    LastName = "Olsen",
                    Address = "Gaden 22",
                    Postcode = "8400",
                    Cityname = "Balle",
                    BoatDriver = true,
                    Email = "Larsolsen@armyspy.com",
                    Gender = Gender.Male,
                    PhoneNumber = "78961230",
                    Username = "lars",
                    PasswordHash = EncryptionHelper.Sha256("olsen"),
                    Position = SailClubMember.Positions.Admin,
                    DateOfBirth = "1973-08-17"
                },
                 new SailClubMember
                {
                    FirstName = "Karen",
                    LastName = "Wolff",
                    Address = "Bagervej 1",
                    Postcode = "7000",
                    Cityname = "Thisted",
                    BoatDriver = true,
                    Email = "Wolff@armyspy.com",
                    Gender = Gender.Female,
                    PhoneNumber = "22112233",
                    Username = "karen",
                    PasswordHash = EncryptionHelper.Sha256("wolff"),
                    Position = SailClubMember.Positions.Admin,
                    DateOfBirth = "1884-11-25"
                }, new SailClubMember
                {
                    FirstName = "Bodil",
                    LastName = "Kjær",
                    Address = "Rødhusvej 23",
                    Postcode = "1500",
                    Cityname = "København",
                    BoatDriver = true,
                    Email = "Kjær@armyspy.com",
                    Gender = Gender.Female,
                    PhoneNumber = "78451236",
                    Username = "bodil",
                    PasswordHash = EncryptionHelper.Sha256("kjær"),
                    Position = SailClubMember.Positions.Member,
                    DateOfBirth = "1965-02-27"
                }, new SailClubMember
                {
                    FirstName = "Anne",
                    LastName = "Frank",
                    Address = "Overgade 1",
                    Postcode = "6700",
                    Cityname = "Nørager",
                    BoatDriver = true,
                    Email = "AnneF@armyspy.com",
                    Gender = Gender.Female,
                    PhoneNumber = "11223344",
                    Username = "anne",
                    PasswordHash = EncryptionHelper.Sha256("frank"),
                    Position = SailClubMember.Positions.Member,
                    DateOfBirth = "1924-09-02"
                },
                 new SailClubMember
                {
                    FirstName = "Anders",
                    LastName = "And",
                    Address = "Paradisæblevej 123",
                    Postcode = "1111",
                    Cityname = "Andeby",
                    BoatDriver = true,
                    Email = "AndersA@armyspy.com",
                    Gender = Gender.Male,
                    PhoneNumber = "45639878",
                    Username = "anders",
                    PasswordHash = EncryptionHelper.Sha256("and"),
                    Position = SailClubMember.Positions.Member,
                    DateOfBirth = "1945-06-28"
                },
                 new SailClubMember
                {
                    FirstName = "Rip",
                    LastName = "And",
                    Address = "Paradisæblevej 123",
                    Postcode = "1111",
                    Cityname = "Andeby",
                    BoatDriver = true,
                    Email = "RipA@armyspy.com",
                    Gender = Gender.Male,
                    PhoneNumber = "45639871",
                    Username = "rip",
                    PasswordHash = EncryptionHelper.Sha256("and"),
                    Position = SailClubMember.Positions.Member,
                    DateOfBirth = "1976-03-23"
                }, new SailClubMember
                {
                    FirstName = "Marcus",
                    LastName = "Husmand",
                    Address = "Genvej 1",
                    Postcode = "7800",
                    Cityname = "Lånum",
                    BoatDriver = true,
                    Email = "Husmand@armyspy.com",
                    Gender = Gender.Male,
                    PhoneNumber = "79461382",
                    Username = "marcus",
                    PasswordHash = EncryptionHelper.Sha256("husmand"),
                    Position = SailClubMember.Positions.Member,
                    DateOfBirth = "1974-06-17"
                },
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
                },
                   new StudentMember
                {
                    FirstName = "Oswald",
                    LastName = "Bjørn",
                    Address = "Hadsundvej 88",
                    Postcode = "4050",
                    Cityname = "Oslo",
                    BoatDriver = false,
                    Email = "OswaldB@teleworm.us",
                    Gender = Gender.Male,
                    PhoneNumber = "11447799",
                    Username = "oswald",
                    PasswordHash = EncryptionHelper.Sha256("bjørn"),
                    DateOfBirth = "1994-04-03"
                },
                   new StudentMember
                {
                    FirstName = "Ulla",
                    LastName = "Henriksen",
                    Address = "kastevej 2",
                    Postcode = "9010",
                    Cityname = "Aalborg",
                    BoatDriver = false,
                    Email = "UllaH@teleworm.us",
                    Gender = Gender.Female,
                    PhoneNumber = "13467946",
                    Username = "ulla",
                    PasswordHash = EncryptionHelper.Sha256("henriksen"),
                    DateOfBirth = "1992-11-22"
                },
                   new StudentMember
                {
                    FirstName = "Per",
                    LastName = "Teglgård",
                    Address = "Grusvej 200",
                    Postcode = "4500",
                    Cityname = "Herning",
                    BoatDriver = false,
                    Email = "PTeglgård@teleworm.us",
                    Gender = Gender.Male,
                    PhoneNumber = "36148520",
                    Username = "per",
                    PasswordHash = EncryptionHelper.Sha256("teglgård"),
                    DateOfBirth = "1991-01-22"
                },
                #region Students that are used for adding to a team in the user tests - DO NOT ADD TO TEAM IN SEED DATA
                   new StudentMember
                {
                    FirstName = "Isabella",
                    LastName = "Christensen",
                    Address = "Langegade 58",
                    Postcode = "1640",
                    Cityname = "København V",
                    BoatDriver = false,
                    Email = "IsabellaChristensen@armyspy.com",
                    Gender = Gender.Female,
                    PhoneNumber = "21961289",
                    Username = "isabella",
                    PasswordHash = EncryptionHelper.Sha256("christensen"),
                    DateOfBirth = "1997-03-18"
                },
                   new StudentMember
                {
                    FirstName = "Malthe",
                    LastName = "Frandsen",
                    Address = "Skibbroen 7",
                    Postcode = "5320",
                    Cityname = "Agedrup",
                    BoatDriver = false,
                    Email = "MaltheFrandsen@armyspy.com",
                    Gender = Gender.Male,
                    PhoneNumber = "21938966",
                    Username = "malthe",
                    PasswordHash = EncryptionHelper.Sha256("frandsen"),
                    DateOfBirth = "1992-08-22"
                },
                   new StudentMember
                {
                    FirstName = "Malene",
                    LastName = "Jensen",
                    Address = "Stokløkken 2",
                    Postcode = "6683",
                    Cityname = "Føvling",
                    BoatDriver = false,
                    Email = "MaleneJensen@dayrep.com",
                    Gender = Gender.Female,
                    PhoneNumber = "81405098",
                    Username = "malene",
                    PasswordHash = EncryptionHelper.Sha256("jensen"),
                    DateOfBirth = "1995-11-27"
                },
                   new StudentMember
                {
                    FirstName = "Josefine",
                    LastName = "Henriksen",
                    Address = "Møllebakken 96",
                    Postcode = "3310",
                    Cityname = "Ølsted",
                    BoatDriver = false,
                    Email = "JosefineHenriksen@teleworm.us",
                    Gender = Gender.Female,
                    PhoneNumber = "20540781",
                    Username = "josefine",
                    PasswordHash = EncryptionHelper.Sha256("henriksen"),
                    DateOfBirth = "1995-09-27"
                },
                #endregion
            };
            #endregion

            #region DataArray :: Teams
            var teams = new Team[]
                         {
                             new Team()
                             {
                                 Name = "MasterRace",
                                 Level = Team.ClassLevel.Second
                             },

                             new Team()
                             {
                                 Name = "Svenskerne",
                                 Level = Team.ClassLevel.First
                             }
                         };
            #endregion
            #endregion

            #region Data linking

            #region Lectures -> Team
            lectures[0].Team = teams[1];
            lectures[1].Team = teams[0];
            lectures[2].Team = teams[0];
            #endregion

            #region Lectures -> PresentMembers
            lectures[0].PresentMembers = new List<StudentMember>()
                                         {
                                             studentMembers[5],
                                             studentMembers[6],
                                             studentMembers[7],
                                             studentMembers[8]
                                         };
            lectures[1].PresentMembers = new List<StudentMember>()
                                         {
                                             studentMembers[0],
                                             studentMembers[2],
                                             studentMembers[3],
                                             studentMembers[4]
                                         };
            lectures[2].PresentMembers = new List<StudentMember>()
                                         {
                                             studentMembers[0],
                                             studentMembers[1],
                                             studentMembers[2],
                                             studentMembers[3],
                                             studentMembers[4]
                                         };
            #endregion

            #region Logbooks -> FiledBy
            logbooks[0].FiledBy = sailClubMembers[0];
            logbooks[1].FiledBy = sailClubMembers[3];
            #endregion

            #region Logbooks -> ActualCrew
            logbooks[0].ActualCrew = new List<Person>
                                     {
                                         sailClubMembers[3],
                                         sailClubMembers[4],
                                         sailClubMembers[5],
                                         sailClubMembers[7]
                                     };
            logbooks[1].ActualCrew = new List<Person>
                                     {
                                         sailClubMembers[0],
                                         sailClubMembers[1],
                                         sailClubMembers[2],
                                         sailClubMembers[3],
                                         sailClubMembers[4]
                                     };
            #endregion

            #region Trips -> Captain
            trips[0].Captain = sailClubMembers[0];
            trips[1].Captain = sailClubMembers[3];
            trips[2].Captain = sailClubMembers[1];
            trips[3].Captain = sailClubMembers[5];
            trips[4].Captain = sailClubMembers[7];
            #endregion

            #region Trips -> Logbook
            trips[0].Logbook = logbooks[1];
            trips[1].Logbook = logbooks[0];
            #endregion

            #region Trips -> Boat

            trips[0].Boat = boats[0];
            trips[1].Boat = boats[1];
            trips[2].Boat = boats[2];
            trips[3].Boat = boats[1];
            trips[4].Boat = boats[1];

            #endregion Boats

            #region Trips -> Crew

            // TODO add crew to the trips in array trips.

            trips[0].Crew = new List<Person>
                            {
                                sailClubMembers[0],
                                sailClubMembers[1],
                                sailClubMembers[2],
                                sailClubMembers[3],
                                sailClubMembers[4]
                            };

            trips[1].Crew = new List<Person>
                            {
                                sailClubMembers[3],
                                sailClubMembers[4],
                                sailClubMembers[5],
                                sailClubMembers[6],
                                sailClubMembers[7]
                            };

            trips[2].Crew = new List<Person>
                            {
                                sailClubMembers[1],
                                sailClubMembers[3],
                                sailClubMembers[5],
                                sailClubMembers[6],
                                sailClubMembers[7]
                            };

            trips[3].Crew = new List<Person>
                            {
                                sailClubMembers[5],
                                sailClubMembers[4],
                                sailClubMembers[2],
                                sailClubMembers[1],
                                sailClubMembers[7]
                            };

            trips[4].Crew = new List<Person>
                            {
                                sailClubMembers[7],
                                sailClubMembers[3],
                                sailClubMembers[0],
                                sailClubMembers[2],
                                sailClubMembers[5]
                            };
            #endregion Crew

            #region Events -> Participants

            events[0].Participants = new List<Person>
                                     {
                                         sailClubMembers[3],
                                         sailClubMembers[4],
                                         sailClubMembers[2],
                                         sailClubMembers[1],
                                         sailClubMembers[0]
                                     };

            events[1].Participants = new List<Person>
                                     {
                                         sailClubMembers[0],
                                         sailClubMembers[1],
                                         sailClubMembers[2],
                                         sailClubMembers[3],
                                         sailClubMembers[4],
                                         sailClubMembers[5],
                                         sailClubMembers[9]
                                     };
            #endregion

            #region StudentMembers -> AssociatedTeam

            #region Team #0
            studentMembers[0].AssociatedTeam = teams[0];
            studentMembers[1].AssociatedTeam = teams[0];
            studentMembers[2].AssociatedTeam = teams[0];
            studentMembers[3].AssociatedTeam = teams[0];
            studentMembers[4].AssociatedTeam = teams[0];
            #endregion

            #region Team #1
            studentMembers[5].AssociatedTeam = teams[1];
            studentMembers[6].AssociatedTeam = teams[1];
            studentMembers[7].AssociatedTeam = teams[1];
            studentMembers[8].AssociatedTeam = teams[1];
            #endregion

            #endregion

            #region Teams -> Teacher
            teams[0].Teacher = sailClubMembers[3];
            teams[1].Teacher = sailClubMembers[5];
            #endregion

            #endregion

            #region Save data
            var boatDal = DalLocator.BoatDal;
            var eventDal = DalLocator.EventDal;
            var lectureDal = DalLocator.LectureDal;
            var logbookDal = DalLocator.LogbookDal;
            var tripDal = DalLocator.RegularTripDal;
            var scmDal = DalLocator.SailClubMemberDal;
            var smDal = DalLocator.StudentMemberDal;
            var teamDal = DalLocator.TeamDal;

            // Store the ones that doesn't require anything else first...
            boatDal.Create(boats);
            scmDal.Create(sailClubMembers);

            // Now store the others in an order that doesn't clash with anything
            eventDal.Create(events);
            logbookDal.Create(logbooks);
            tripDal.Create(trips);
            teamDal.Create(teams);
            smDal.Create(studentMembers);
            lectureDal.Create(lectures);
            #endregion
        }
    }
}