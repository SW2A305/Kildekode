using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McSntt.Models
{
    class Boat
    {
        public Boat()
        {
            // Asign the next id to the new instance of the boat
            id = _nextId++;
        }
        // Boats have an unique ID number
        private static int _nextId;
        public int id { get; private set; }
    }
}
