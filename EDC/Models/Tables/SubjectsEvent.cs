using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDC.Models
{
    public class SubjectsEvent
    {
        public long EventID { get; set; }
        [ForeignKey("EventID")]
        public virtual Event Event { get; set; }

        public long SubjectID { get; set; }
        [ForeignKey("SubjectID")]
        public virtual Subject Subject { get; set; }
        //public virtual List<SubjectsCRF> CRFs { get; set; }


    }
}