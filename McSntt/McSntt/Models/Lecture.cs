using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McSntt.Models
{
    public class Lecture
    {
        public virtual int LectureId { get; set; }
        public virtual Team Team { get; set; }
        [Column(TypeName = "DateTime2")]
        public virtual DateTime DateOfLecture { get; set; }
        #region UndervisningsOmråder
        public virtual bool RopeWorksLecture { get; set; }
        public virtual bool Navigation { get; set; }
        public virtual bool Motor { get; set; }
        public virtual bool Drabant { get; set; }
        public virtual bool Gaffelrigger { get; set; }
        public virtual bool Night { get; set; }
        #endregion

        public virtual ICollection<StudentMember> PresentMembers { get; set; }
    }
}
