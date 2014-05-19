namespace McSntt.Models
{
    public class StudentMember : SailClubMember
    {
        private Team _associatedTeam;
        private long _associatedTeamId;

        public StudentMember() { base.Position = Positions.Student; }

        public long StudentMemberId { get; set; }

        public Team AssociatedTeam
        {
            get { return this._associatedTeam; }
            set
            {
                this._associatedTeam = value;
                this.AssociatedTeamId = (value != null ? value.TeamId : 0);
            }
        }

        public long AssociatedTeamId
        {
            get
            {
                if (this._associatedTeamId == 0 && this._associatedTeam != null) {
                    this._associatedTeamId = this._associatedTeam.TeamId;
                }
                return this._associatedTeamId;
            }
            set { this._associatedTeamId = value; }
        }

        #region Undervisnings kriterier
        public bool RopeWorks { get; set; }
        public bool Navigation { get; set; }
        public bool Motor { get; set; }
        public bool Drabant { get; set; }
        public bool Gaffelrigger { get; set; }
        public bool Night { get; set; }
        #endregion
    }
}
