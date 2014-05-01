using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McSntt.Models
{
    public class Boat
    {
        public Boat()
        {
            // Assign the id and increment the next
            Id = _nextId++;
        }
        // Boats have an unique ID number
        private static int _nextId;
        public int Id { get; private set; }

        public BoatType Type { get; set; }
        public string NickName { get; set; }
        public string ImagePath { get; set; }
        public bool Operational { get; set; }

        public override string ToString()
        {
            return NickName;
        }
    }

    public enum BoatType
    {
        Gaffelrigger = 0,
        Drabant = 1,
        Spækhugger = 2,
        J80 = 3,
        Mini12 = 4
    }
}
