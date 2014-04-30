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
            TeamId = _nextId++;
        }
        // Teams have an unique ID number½
        private static int _nextId;
        public int TeamId { get; set; }

        private string _name;
        private List<SailClubMember> _teamMembers = new List<SailClubMember>();

        public enum ClassLevel { First = 1, Second = 2}
        private ClassLevel _level;

       

        #region Undervisnings kriterier
        private bool _robeWorks;
        private bool _navigation;
        private bool _motor;
        private bool _drabant;
        private bool _gaffelrigger;
        private bool _night;
        #endregion

        #region Properties
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public List<SailClubMember> TeamMembers
        {
            get { return _teamMembers; }
            set { _teamMembers = value; }
        }

        public bool RobeWorks
        {
            get { return _robeWorks; }
            set { _robeWorks = value; }
        }
        public bool Navigation
        {
            get { return _navigation; }
            set { _navigation = value; }
        }
        public bool Motor
        {
            get { return _motor; }
            set { _motor = value; }
        }
        public bool Drabant
        {
            get { return _drabant; }
            set { _drabant = value; }
        }
        public bool Gaffelrigger
        {
            get { return _gaffelrigger; }
            set { _gaffelrigger = value; }
        }
        public bool Night
        {
            get { return _night; }
            set { _night = value; }
        }
        public ClassLevel Level
        {
            get { return _level; }
            set { _level = value; }
        }
        #endregion

        

    }
}
