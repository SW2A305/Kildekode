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
        public virtual int TeamId { get; set; }
        public virtual string Name { get; set; }
        public virtual bool RobeWorks { get; set; }
        public virtual bool Navigation { get; set; }
        public virtual bool Motor { get; set; }
        public virtual bool Drabant { get; set; }
        public virtual bool Gaffelrigger { get; set; }
        public virtual bool Night { get; set; }
        public virtual ClassLevel Level { get; set; }

        public virtual ICollection<SailClubMember> TeamMembers { get; set; }
        #endregion
    }
}
