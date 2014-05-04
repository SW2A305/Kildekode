using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McSntt.Models
{
    public class StudentMember : SailClubMember
    {
        public StudentMember()
        {
            Position = Positions.Student;
        }

        Team AssociatedTeam { get; set; }

        #region Undervisnings kriterier
        public bool RopeWorks { get; set; }
        public bool Navigation { get; set; }
        public bool Motor { get; set; }
        public bool Drabant { get; set; }
        public bool Gaffelrigger { get; set; }
        public bool Night { get; set; }
        #endregion


    }
}
