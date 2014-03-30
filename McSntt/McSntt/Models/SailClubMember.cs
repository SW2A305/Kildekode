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
    }
}
