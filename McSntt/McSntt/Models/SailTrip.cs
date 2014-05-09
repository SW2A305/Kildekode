using System;

namespace McSntt.Models
{
    public abstract class SailTrip
    {
        private Boat _boat;
        private long _boatId;
        private Logbook _logbook;
        private long _logbookId;

        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string WeatherConditions { get; set; }

        public Boat Boat
        {
            get { return this._boat; }
            set
            {
                this._boat = value;
                this.BoatId = (value != null ? value.BoatId : 0);
            }
        }

        public long BoatId
        {
            get
            {
                if (this._boatId == 0 && this._boat != null) { this._boatId = this._boat.BoatId; }
                return this._boatId;
            }
            set { this._boatId = value; }
        }

        public Logbook Logbook
        {
            get { return this._logbook; }
            set
            {
                this._logbook = value;
                this._logbookId = (value != null ? value.LogbookId : 0);
            }
        }

        public long LogbookId
        {
            get
            {
                if (this._logbookId == 0 && this._logbook != null) { this._logbookId = this._logbook.LogbookId; }
                return this._logbookId;
            }
            set { this._logbookId = value; }
        }
    }
}
