using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace McSntt.Models
{
    public abstract class SailTrip
    {
        public int SailTripId { get; set; }
        public int BoatId { get; set; }
        public virtual Boat Boat { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DepartureTime { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime ArrivalTime { get; set; }
        public string WeatherConditions { get; set; }
        public string Comments { get; set; }
    }
}