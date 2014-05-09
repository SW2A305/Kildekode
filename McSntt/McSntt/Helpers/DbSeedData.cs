﻿using McSntt.Models;

namespace McSntt.Helpers
{
    public class DbSeedData
    {
        public static void CreateSeedData()
        {
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
                                
                            }
                        };
            #endregion

            #region DataArray :: Events
            #endregion

            #region DataArray :: Lectures
            #endregion

            #region DataArray :: Logbooks
            #endregion

            #region DataArray :: Persons
            #endregion

            #region DataArray :: RegularTrips
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
        }
    }
}