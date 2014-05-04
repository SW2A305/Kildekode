using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace McSntt.Models
{
    public class Event
    {
        public virtual int EventId { get; set; }

        [Column(TypeName = "DateTime2")]
        public virtual DateTime EventDate { get; set; }

        public virtual string EventTitle { get; set; }
        public virtual bool SignUpReq { get; set; }
        public virtual string Description { get; set; }

        public virtual ICollection<Person> Participants { get; set; }
        // TODO What on earth is this thing doing? We may need to reconsider this one.
        public virtual ICollection<Event> EventList { get; set; }
    }
}
