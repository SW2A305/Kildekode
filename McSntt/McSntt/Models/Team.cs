using System.Collections.Generic;

namespace McSntt.Models
{
    public class Team
    {
        private SailClubMember _teacher;
        private long _teacherId;

        public enum ClassLevel
        {
            First = 1,
            Second = 2
        }

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
                TeacherId = (value != null ? value.SailClubMemberId : 0);
            }
        }

        public long TeacherId
        {
            get
            {
                if (_teacherId == 0 && _teacher != null) { _teacherId = _teacher.SailClubMemberId; }
                return this._teacherId;
            }
            set { this._teacherId = value; }
        }

        public ICollection<StudentMember> TeamMembers { get; set; }
        public ICollection<Lecture> Lectures { get; set; }
        #endregion
    }
}
