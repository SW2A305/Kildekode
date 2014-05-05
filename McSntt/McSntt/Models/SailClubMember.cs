using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace McSntt.Models
{
    /// <summary>
    ///     A member of the club should be of this type, containing additional information.
    /// </summary>
    [Table("SailClubMembers")]
    public class SailClubMember : Person
    {
        private Positions _position;

        #region Properties
        [Index(IsUnique = true)]
        public virtual int SailClubMemberId { get; set; }

        public virtual Positions Position
        {
            get { return this._position; }
            set { this._position = value; }
        }

        public virtual string Username { get; set; }
        public virtual string PasswordHash { get; set; }

        [InverseProperty("FiledBy")]
        public virtual ICollection<Logbook> FiledLogbooks { get; set; }
        #endregion

        #region Enumerations
        /// <summary>
        ///     Represents the various positions that a member can have.
        /// </summary>
        public enum Positions
        {
            SupportMember = 10,
            Student = 20,
            Member = 30,
            Teacher = 40,
            Admin = 50
        };
        #endregion

        /// <summary>
        ///     Default constructor.
        /// </summary>
        public SailClubMember()
        {
            this._position = Positions.Member;
        }

        /// <summary>
        ///     Convenience method for setting the MemberId property through a string value. If there's an error parsing the
        ///     number, the MemberId will be set to the default int-value, which should ensure that Entity Framework will
        ///     autoincrement MemberId... Ideally.
        /// </summary>
        /// <param name="memberId">String (possibly) containing the MemberId to set.</param>
        public void SetMemberId(String memberId)
        {
            int parsedNumber;

            this.SailClubMemberId = int.TryParse(memberId, out parsedNumber) ? parsedNumber : default(int);
        }
    }
}
