using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDC.Models
{
    public class SubjectsCRF
    {
        public long CRFInSubjectEventID { get; set; }
        public long SubjectEventID { get; set; }
        [ForeignKey("SubjectEventID")]
        public SubjectEvent SubjectsEvent { get; set; }

        public long CRFID { get; set; }
        [ForeignKey("CRFID")]
        public CRF CRF { get; set; }

        public bool IsClosed { get; set; }
    }
}