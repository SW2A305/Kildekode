using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using McSntt.Views.Windows;

namespace McSntt.Models
{
    public class Event
    {
        public long EventId { get; set; }

        public DateTime EventDate { get; set; }
        public string EventTitle { get; set; }
        public bool SignUpReq { get; set; }
        public string Description { get; set; }
        public string SignUpMsg { get; set; }
        public bool Created { get; set; }

        public ICollection<Person> Participants = new List<Person>();
        public override string ToString()
        {
            return Description;
        }
    }
}
