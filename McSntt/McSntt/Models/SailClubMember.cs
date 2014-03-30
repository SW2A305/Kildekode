namespace McSntt.Models
{
    /// <summary>
    ///     Represents the various positions that a member can have.
    /// </summary>
    public enum Positions
    {
        Admin,
        Teacher,
        Member,
        Student,
        SupportMember
    };

    /// <summary>
    ///     A member of the club should be of this type, containing additional information.
    /// </summary>
    public class SailClubMember : Person
    {
        public int MemberId { get; set; }
        public Positions Position { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}
