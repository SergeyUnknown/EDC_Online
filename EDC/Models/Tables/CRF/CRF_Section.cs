using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDC.Models
{
    public class CRF_Section
    {
        public long CRF_SectionID { get; set; }
        public long CRFID { get; set; }
        [ForeignKey("CRFID")]
        public CRF CRF { get; set; }
        public string Label { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Instructions { get; set; }
        public int PageNumber { get; set; }
        //public string ParentSection { get; set; } //??????????????????????????????
        public bool Border { get; set; }
        public virtual List<CRF_Item> Items { get; set; }
    }
}