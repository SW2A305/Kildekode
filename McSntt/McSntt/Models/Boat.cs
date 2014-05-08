using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace McSntt.Models
{
    public class Boat
    {
        public long BoatId { get; set; }

        public BoatType Type { get; set; }
        public string NickName { get; set; }
        public string ImagePath { get; set; }
        public bool Operational { get; set; }

        public override string ToString()
        {
            return this.NickName;
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
