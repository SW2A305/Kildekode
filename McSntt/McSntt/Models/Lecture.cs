using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McSntt.Models
{
    public class Lecture
    {
        private Team _team;
        private long _teamId;

        public virtual long LectureId { get; set; }

        public virtual Team Team
        {
            get
            {
                return this._team;
            }
            set
            {
                this._team = value;
                this.TeamId = value.TeamId;
            }
        }

        public long TeamId
        {
            get
            {
                if (_teamId == 0 && _team != null) { _teamId = _team.TeamId; }
                return this._teamId;
            }
            set
            {
                this._teamId = value;
            }
        }

        [Column(TypeName = "DateTime2")]
        public virtual DateTime DateOfLecture { get; set; }
        #region UndervisningsOmråder
        public virtual bool RopeWorksLecture { get; set; }
        public virtual bool Navigation { get; set; }
        public virtual bool Motor { get; set; }
        public virtual bool Drabant { get; set; }
        public virtual bool Gaffelrigger { get; set; }
        public virtual bool Night { get; set; }
        #endregion

        public virtual ICollection<StudentMember> PresentMembers { get; set; }
    }
}
