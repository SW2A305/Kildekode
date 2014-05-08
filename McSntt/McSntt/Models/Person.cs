using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public long PersonId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }
        public string Cityname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        // TODO Should we change this back to a date?
        public string DateOfBirth { get; set; }
        public bool BoatDriver { get; set; }
        public Gender Gender { get; set; }

        public string FullName
        {
            get { return this.FirstName + " " + this.LastName; }
        }

        public override string ToString()
        {
            return this.FullName;
        }
    }
}
