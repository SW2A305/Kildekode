using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace McSntt.Models
{
    public abstract class SailTrip
    {
        [Column(TypeName = "DateTime2")]
        public virtual DateTime DepartureTime { get; set; }

        [Column(TypeName = "DateTime2")]
        public virtual DateTime ArrivalTime { get; set; }

        public virtual Boat Boat { get; set; }
        public long BoatId { get; set; }
        public virtual string WeatherConditions { get; set; }
        public virtual Logbook Logbook { get; set; }
        public long LogbookId { get; set; }
    }
}
