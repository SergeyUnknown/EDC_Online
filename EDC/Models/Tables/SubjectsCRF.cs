using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDC.Models
{
    public class SubjectsCRF
    {
        public SubjectsEvent SubjectsEvent { get; set; }
        //ниже внешний ключ
        public long SubjectsEventID { get; set; }
        public long SubjectsEventCRFID { get; set; }
        //end
        public long CRFID { get; set; }
        [ForeignKey("CRFID")]
        public CRF CRF { get; set; }

        public long? SubjectID { get; set; }
        [ForeignKey("SubjectID")]
        public Subject Subject { get; set; }
        public virtual List<SubjectsItem> Items { get; set; }

        public bool IsClosed { get; set; }
    }
}