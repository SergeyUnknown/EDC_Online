using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDC.Models
{
    public class CRF_Group
    {
        public long CRF_GroupID { get; set; }

        public long CRFID { get; set; }
        [ForeignKey("CRFID")]
        public CRF CRF { get; set; }
        public string Label { get; set; }
        public string Header { get; set; }
        public int RepeatNumber { get; set; }
        public int RepeatMax { get; set; }
        public virtual List<CRF_Item> Items { get; set; }
    }
}