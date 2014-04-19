using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace McSntt.Models
{
    public class RegularTrip : SailTrip
    {
        public int RegularTripId { get; set; }
        public int CaptainId { get; set; }
        [ForeignKey("CaptainId")]
        public virtual Person Captain { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime ExpectedArrivalTime { get; set; }
        public string PurposeAndArea { get; set; }

        public IList<Person> Crew { get; set; }
    }
}