using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace McSntt.Models
{
    public class RegularTrip : SailTrip
    {
        public virtual int RegularTripId { get; set; }
        public virtual Person Captain { get; set; }

        [Column(TypeName = "DateTime2")]
        public virtual DateTime ExpectedArrivalTime { get; set; }
        public virtual string PurposeAndArea { get; set; }

        public virtual ICollection<Person> Crew { get; set; }
    }
}