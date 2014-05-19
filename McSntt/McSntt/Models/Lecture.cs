using System;
using System.Collections.Generic;
using System.Globalization;

namespace McSntt.Models
{
    public class Lecture
    {
        private Team _team;
        private long _teamId;

        public long LectureId { get; set; }

        public Team Team
        {
            get { return this._team; }
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
                if (this._teamId == 0 && this._team != null) { this._teamId = this._team.TeamId; }
                return this._teamId;
            }
            set { this._teamId = value; }
        }

        public string DateOfLectureString
        {
            get
            {
                return this.DateOfLecture.ToString("d", new CultureInfo("es-ES")) + " kl. " +
                       this.DateOfLecture.ToString("t", new CultureInfo("da-DK"));
            }
        }

        public DateTime DateOfLecture { get; set; }

        public ICollection<StudentMember> PresentMembers { get; set; }

        #region UndervisningsOmråder
        public bool RopeWorksLecture { get; set; }
        public bool Navigation { get; set; }
        public bool Motor { get; set; }
        public bool Drabant { get; set; }
        public bool Gaffelrigger { get; set; }
        public bool Night { get; set; }
        #endregion
    }
}
