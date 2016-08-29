using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDC.Models
{
    public class CRFInEvent
    {
        public long EventID { get; set; }
        [ForeignKey("EventID")]
        public virtual Event Event { get; set; }

        public long CRFID { get; set; }
        [ForeignKey("CRFID")]
        public virtual CRF CRF { get; set; }

        public int Position { get; set; } //Позиция CRF в событии
    }
}