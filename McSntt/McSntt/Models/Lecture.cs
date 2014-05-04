using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McSntt.Models
{
    public class Lecture
    {
        public Lecture()
        {
            // Assign the id and increment the next
            LectureId = ++_nextId;
        }
        // Teams have an unique ID number½
        private static int _nextId;
        public int LectureId { get; set; }
        public DateTime DateOfLecture { get; set; }
        #region UndervisningsOmråder
        public bool RopeWorksLecture { get; set; }
        public bool Navigation { get; set; }
        public bool Motor { get; set; }
        public bool Drabant { get; set; }
        public bool Gaffelrigger { get; set; }
        public bool Night { get; set; }
        #endregion
        private IList<StudentMember> _presentMembers = new List<StudentMember>();

        public IList<StudentMember> PresentMembers
        {
            get { return _presentMembers; }
            set { _presentMembers = value; }
        }
    }
}
