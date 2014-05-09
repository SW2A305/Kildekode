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
                            {}
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