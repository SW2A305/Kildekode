using System;

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
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }
        public string Cityname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}