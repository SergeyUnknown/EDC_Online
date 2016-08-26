using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDC.Models
{
    public class SubjectsItem
    {
        public long ItemID { get; set; }
        [ForeignKey("ItemID")]
        public CRF_Item Item { get; set; }

        public long SubjectsCRFID { get; set; }
        [ForeignKey("SubjectsCRFID")]
        public SubjectsCRF SubjectsCRF { get; set; }
        public string Value { get; set; }
    }
}