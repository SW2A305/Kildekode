using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace McSntt.Models
{
    public class Boat
    {
        public virtual long BoatId { get; set; }

        public virtual BoatType Type { get; set; }
        public virtual string NickName { get; set; }
        public virtual string ImagePath { get; set; }
        public virtual bool Operational { get; set; }

        [InverseProperty("Boat")]
        public virtual List<RegularTrip> SailTrips { get; set; }

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
