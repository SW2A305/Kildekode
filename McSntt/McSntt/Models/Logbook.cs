using System;
using System.Collections.Generic;
using System.Windows.Controls.Primitives;

namespace McSntt.Models
{
    public class Logbook
    {
        public Logbook() {         

            // Assign the id and increment the next
            Id = _nextId++;
        }
        // Logbooks have an unique ID number
        private static int _nextId;

        public int Id { get; private set; }

        public DateTime ActualDepartureTime { get; set; }
        public DateTime ActualArrivalTime { get; set; }

        public bool DamageInflicted { get; set; }
        public string DamageDescription { get; set; }
        public string AnswerFromBoatChief { get; set; }

        public SailClubMember FiledBy { get; set; }

        public IList<Person> ActualCrew { get; set; }
        
    }
}