using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace McSntt.Models
{
    public class Lecture
    {
        private Team _team;
        private long _teamId;

        public long LectureId { get; set; }

        public Team Team
        {
            get
            {
                return this._team;
            }
            set
            {
                this._team = value;
                this.TeamId = (value != null ? value.TeamId : 0);
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

        public DateTime DateOfLecture { get; set; }
        #region UndervisningsOmråder
        public bool RopeWorksLecture { get; set; }
        public bool Navigation { get; set; }
        public bool Motor { get; set; }
        public bool Drabant { get; set; }
        public bool Gaffelrigger { get; set; }
        public bool Night { get; set; }
        #endregion

        public ICollection<StudentMember> PresentMembers { get; set; }
    }
}
