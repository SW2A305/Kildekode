using System.Collections.Generic;

namespace McSntt.Models
{
    public class Team
    {
        public enum ClassLevel
        {
            First = 1,
            Second = 2
        }

        #region Properties
        public virtual long TeamId { get; set; }
        public virtual string Name { get; set; }
        public virtual ClassLevel Level { get; set; }

        public virtual ICollection<StudentMember> TeamMembers { get; set; }
        public virtual ICollection<Lecture> Lectures { get; set; }
        #endregion
    }
}
