using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDC.Models
{
    public class Subject
    {
        public long SubjectID { get; set; }
        public virtual List<Event> Events { get; set; } //subjectEvents
        public MedicalCenter MedicalCenter { get; set; }

        [ForeignKey("MedicalCenter")]
        public long MedicalCenterID { get; set; }

    }
}