using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace McSntt.Models
{
    public class Logbook
    {
        [Key]
        public virtual long LogbookId { get; set; }

        [Column(TypeName = "DateTime2")]
        public virtual DateTime ActualDepartureTime { get; set; }

        [Column(TypeName = "DateTime2")]
        public virtual DateTime ActualArrivalTime { get; set; }

        public virtual bool DamageInflicted { get; set; }
        public virtual string DamageDescription { get; set; }
        public virtual string AnswerFromBoatChief { get; set; }

        public virtual SailClubMember FiledBy { get; set; }
        public long FiledById { get; set; }

        public virtual ICollection<Person> ActualCrew { get; set; }
    }
}
