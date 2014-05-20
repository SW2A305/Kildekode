using System.Collections.Generic;

namespace McSntt.Models
{
    public class Team
    {
        #region ClassLevel enum
        public enum ClassLevel
        {
            First = 1,
            Second = 2
        }
        #endregion

        #region Properties
        public long TeamId { get; set; }
        public string Name { get; set; }
        public ClassLevel Level { get; set; }

        public SailClubMember Teacher
        {
            get { return this._teacher; }
            set
            {
                this._teacher = value;
                this.TeacherId = (value != null ? value.SailClubMemberId : 0);
            }
        }

        public long TeacherId
        {
            get
            {
                if (this._teacherId == 0 && this._teacher != null) { this._teacherId = this._teacher.SailClubMemberId; }
                return this._teacherId;
            }
            set { this._teacherId = value; }
        }

        public ICollection<StudentMember> TeamMembers { get; set; }
        public ICollection<Lecture> Lectures { get; set; }
        #endregion

        private SailClubMember _teacher;
        private long _teacherId;
    }
}
