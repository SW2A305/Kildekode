using System;
using System.Collections.Generic;

namespace McSntt.Models
{
    public class RegularTrip : SailTrip
    {
        private Person _captain;
        private long _captainId;

        public long RegularTripId { get; set; }

        // TODO: The following is depceciated, it's set it SailTrip SUPERCLASS!
        public DateTime ExpectedArrivalTime { get; set; }
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

        public ICollection<Person> Crew { get; set; }
    }
}