using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDC.Models
{
    public class SubjectsCRF
    {
        
        public long SubjectID { get; set; }
        [ForeignKey("SubjectID")]
        public Subject Subject { get; set; }
        public long EventID { get; set; }
        [ForeignKey("EventID")]
        public Event Event { get; set; }
        public long CRFID { get; set; }
        [ForeignKey("CRFID")]
        public CRF CRF { get; set; }

        public bool IsClosed { get; set; }
    }
}