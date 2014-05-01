using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McSntt.Models
{
    class Event
    {
        public virtual int EventId { get; set; }
        public virtual string EventTitle { get; set; }
        public virtual string EventCreatedBy { get; set; }
        public virtual bool SignUpReq { get; set; }
        public virtual string Description { get; set; }
        public virtual ICollection<Person> Participants { get; set; }
    }
}
