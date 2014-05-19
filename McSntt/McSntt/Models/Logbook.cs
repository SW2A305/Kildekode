using System;
using System.Collections.Generic;

namespace McSntt.Models
{
    public class Logbook
    {
        private SailClubMember _filedBy;
        private long _filedById;

        public long LogbookId { get; set; }
        public DateTime ActualDepartureTime { get; set; }
        public DateTime ActualArrivalTime { get; set; }

        public bool DamageInflicted { get; set; }
        public string DamageDescription { get; set; }
        public string AnswerFromBoatChief { get; set; }

        public SailClubMember FiledBy
        {
            get { return this._filedBy; }
            set
            {
                this._filedBy = value;
                this.FiledById = (value != null ? value.SailClubMemberId : 0);
            }
        }

        public long FiledById
        {
            get
            {
                if (this._filedById == 0 && this._filedBy != null) { this._filedById = this._filedBy.SailClubMemberId; }
                return this._filedById;
            }
            set { this._filedById = value; }
        }

        public ICollection<Person> ActualCrew { get; set; }
    }
}
