using System;
using System.Collections.Generic;

namespace McSntt.Models
{
    public class RegularTrip : SailTrip
    {
        private Person _captain;
        private long _captainId;
        private Person _createdBy;
        private long _createdById;

        public long RegularTripId { get; set; }
        public string PurposeAndArea { get; set; }

        public Person Captain
        {
            get { return this._captain; }
            set
            {
                this._captain = value;
                this.CaptainId = (value != null ? value.PersonId : 0);
            }
        }

        public long CaptainId
        {
            get
            {
                if (_captainId == 0 && _captain != null) { _captainId = _captain.PersonId; }
                return this._captainId;
            }
            set { this._captainId = value; }
        }

        public Person CreatedBy
        {
            get
            {
                return this._createdBy;
            }
            set
            {
                this._createdBy = value;
                CreatedById = (value != null ? value.PersonId : 0);
            }
        }

        public long CreatedById
        {
            get
            {
                if (_createdById == 0 && _createdBy != null) { _createdById = _createdBy.PersonId; }
                return this._createdById;
            }
            set
            {
                this._createdById = value;
            }
        }

        public ICollection<Person> Crew { get; set; }
    }
}