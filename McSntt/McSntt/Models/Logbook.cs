using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Controls.Primitives;

namespace McSntt.Models
{
    public class Logbook
    {
        public virtual int LogbookId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public virtual DateTime ActualDepartureTime { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public virtual DateTime ActualArrivalTime { get; set; }

        public virtual bool DamageInflicted { get; set; }
        public virtual string DamageDescription { get; set; }
        public virtual string AnswerFromBoatChief { get; set; }

        public virtual SailClubMember FiledBy { get; set; }

        public virtual ICollection<Person> ActualCrew { get; set; }
        
    }
}