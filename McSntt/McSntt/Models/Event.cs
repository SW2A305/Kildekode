using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McSntt.Models
{
    class Event
    {
        public int EventId { get; set; }
        public string EventTitle { get; set; }
        public string EventCreatedBy { get; set; }
        public bool SignUpReq { get; set; }
        public string Description { get; set; }
        public IList<Person> Participants { get; set; }




    }
}
