using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace McSntt.Models
{
    /// <summary>
    ///     Represents a gender.
    /// </summary>
    public enum Gender
    {
        Male,
        Female,
        NotDefined
    };

    /// <summary>
    ///     Represents a person, with the information needed for it.
    /// </summary>
    public class Person
    {
        // TODO: THIS SHOULD BE UNIQUE -- (Tristan) It should be unique already, as it's the primary key in the DB.
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }
        public string Cityname { get; set; }
        public string DateOfBirth { get; set; }
        public bool BoatDriver { get; set; }
        public Gender Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        [InverseProperty("Crew")]
        public IList<RegularTrip> PartOfCrewOn { get; set; }
    }
}