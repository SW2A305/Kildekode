using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McSntt.Models
{
    public class Boat
    {
        public virtual int BoatId { get; set; }

        public virtual BoatType Type { get; set; }
        public virtual string NickName { get; set; }
        public virtual string ImagePath { get; set; }
        public virtual bool Operational { get; set; }

        public override string ToString()
        {
            return NickName;
        }    }

    public enum BoatType
    {
        Gaffelrigger = 0,
        Drabant = 1,
        Spækhugger = 2,
        J80 = 3,
        Mini12 = 4
    }
}
