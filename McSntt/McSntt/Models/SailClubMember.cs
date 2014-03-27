namespace McSntt.Models
{
    /// <summary>
    ///     Represents the various positions that a member can have.
    /// </summary>
    /// TODO We should really make sure that this goes from lowest to highest, to make it easier on ourselves.
    public enum Positions
    {
        Member,
        Student,
        SupportMember,
        Admin,
        Teacher
    };

    /// <summary>
    ///     A member of the club should be of this type, containing additional information.
    /// </summary>
    public class SailClubMember : Person
    {
        public int MemberId { get; set; }
        public Positions Position { get; set; }
    }
}
