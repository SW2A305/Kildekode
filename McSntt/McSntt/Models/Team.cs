using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McSntt.Models
{
    public class Team
    {
        public Team()
        {
            // Assign the id and increment the next
            TeamId = ++_nextId;
        }
        // Teams have an unique ID number½
        private static int _nextId;
        public int TeamId { get; set; }

        private string _name;
        private IList<StudentMember> _teamMembers = new List<StudentMember>();

        public enum ClassLevel { First = 1, Second = 2}
        private ClassLevel _level;
        
        #region Properties
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public IList<StudentMember> TeamMembers
        {
            get { return _teamMembers; }
            set { _teamMembers = value; }
        }

       
        public ClassLevel Level
        {
            get { return _level; }
            set { _level = value; }
        }
        #endregion
    }
}
