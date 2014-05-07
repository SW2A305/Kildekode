using System.ComponentModel.DataAnnotations.Schema;

namespace McSntt.Models
{
    [Table("StudentMembers")]
    public class StudentMember : SailClubMember
    {
        public StudentMember() { base.Position = Positions.Student; }

        public int StudentMemberId { get; set; }
        public virtual Team AssociatedTeam { get; set; }

        #region Undervisnings kriterier
        public virtual bool RopeWorks { get; set; }
        public virtual bool Navigation { get; set; }
        public virtual bool Motor { get; set; }
        public virtual bool Drabant { get; set; }
        public virtual bool Gaffelrigger { get; set; }
        public virtual bool Night { get; set; }
        #endregion
    }
}
