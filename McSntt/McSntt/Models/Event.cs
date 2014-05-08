using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace McSntt.Models
{
    public class Event
    {
        public virtual long EventId { get; set; }

        [Column(TypeName = "DateTime2")]
        public virtual DateTime EventDate { get; set; }
        public virtual string EventTitle { get; set; }
        public virtual bool SignUpReq { get; set; }
        public virtual string Description { get; set; }
        public virtual string SignUpMsg { get; set; }
        public virtual bool Created { get; set; }

        public virtual ICollection<Person> Participants { get; set; }
        
        public override string ToString()
        {
            return Description;
        }
    }
}
