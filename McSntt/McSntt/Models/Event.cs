using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McSntt.Models
{
    public class Event
    {
        public Event()
        {
            EventId = _nextId++;
        }

        private static int _nextId;
        public int EventId { get; set; }
        public DateTime EventDate { get; set; }
        public string EventTitle { get; set; }
        public bool SignUpReq { get; set; }
        public string Description { get; set; }
        public IList<Person> Participants { get; set; }
        public IList<Event> EventList { get; set; }
    }
}
