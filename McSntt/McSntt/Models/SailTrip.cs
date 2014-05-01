﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace McSntt.Models
{
    public abstract class SailTrip
    {
        public virtual int SailTripId { get; set; }
        //public virtual int BoatId { get; set; }
        public virtual Boat Boat { get; set; }
        [Column(TypeName = "DateTime2")]
        public virtual DateTime DepartureTime { get; set; }
        [Column(TypeName = "DateTime2")]
        public virtual DateTime ArrivalTime { get; set; }
        public virtual string WeatherConditions { get; set; }
        public virtual string Comments { get; set; }

        public virtual Logbook Logbook { get; set; }
    }
}