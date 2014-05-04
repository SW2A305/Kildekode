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
    [Table("Persons")]
    public class Person
    {
        [Key]
        public virtual int PersonId { get; set; }

        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Address { get; set; }
        public virtual string Postcode { get; set; }
        public virtual string Cityname { get; set; }
        // TODO Should we change this back to a date?
        public virtual string DateOfBirth { get; set; }
        public virtual bool BoatDriver { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string Email { get; set; }

        [NotMapped]
        public string FullName
        {
            get { return this.FirstName + " " + this.LastName; }
        }

        [InverseProperty("Crew")]
        public virtual ICollection<RegularTrip> PartOfCrewOn { get; set; }

        [InverseProperty("Captain")]
        public virtual ICollection<RegularTrip> CaptainOn { get; set; }

        [InverseProperty("Participants")]
        public virtual ICollection<Event> ParticipatingInEvents { get; set; }

        [InverseProperty("ActualCrew")]
        public virtual ICollection<Logbook> PartOfActualCrewOn { get; set; }

        public override string ToString()
        {
            return this.FullName;
        }
    }
}
