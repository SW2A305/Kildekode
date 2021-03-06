﻿using System.Collections.Generic;
using System.Linq;
using McSntt.Helpers;

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
                if (this._captainId == 0 && this._captain != null) { this._captainId = this._captain.PersonId; }
                return this._captainId;
            }
            set { this._captainId = value; }
        }

        public Person CreatedBy
        {
            get { return this._createdBy; }
            set
            {
                this._createdBy = value;
                this.CreatedById = (value != null ? value.PersonId : 0);
            }
        }

        public long CreatedById
        {
            get
            {
                if (this._createdById == 0 && this._createdBy != null) { this._createdById = this._createdBy.PersonId; }
                return this._createdById;
            }
            set { this._createdById = value; }
        }

        public ICollection<Person> Crew { get; set; }

        public bool CanMakeReservation()
        {
            // The following line will cause the database to be locked.
            IEnumerable<RegularTrip> list = DalLocator.RegularTripDal.GetAll();
            /*
            var dal = DalLocator.RegularTripDal;
            var  list = new List<RegularTrip>();


             Add the first 1000 trips to the list
            for (int i = 0; i < 1000; i++)
            {
                if(dal.GetOne(i) == null)
                    break;

                list.Add(dal.GetOne(i));
            } */

            return
                !list.Any(
                          t =>
                          t != null && t.BoatId == this.BoatId && t.DepartureTime <= this.ArrivalTime
                          && t.ArrivalTime >= this.DepartureTime);
        }
    }
}
