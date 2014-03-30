﻿using System;

namespace McSntt.Models
{
    /// <summary>
    ///     A member of the club should be of this type, containing additional information.
    /// </summary>
    public class SailClubMember : Person
    {
        #region Properties
        public int MemberId { get; set; }
        public Positions Position { get; set; }
        public DateTime MemberSince { get; set; }
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
            this.Position = Positions.Member;
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

            this.MemberId = int.TryParse(memberId, out parsedNumber) ? parsedNumber : default(int);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberSince"></param>
        public void SetMemberSince(String memberSince)
        {
            DateTime parsedDate;

            this.MemberSince = DateTime.Parse(memberSince);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateOfBirth"></param>
        public void SetDateOfBirth(String dateOfBirth)
        {
            DateTime parsedDate;

            this.DateOfBirth = DateTime.Parse(dateOfBirth);
        }
    }
}
