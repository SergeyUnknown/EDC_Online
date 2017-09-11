using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDC.Models
{
    public class UnloadingItem
    {
        public int UnloadingProfileID { get; set; }
        [ForeignKey("UnloadingProfileID")]
        public virtual UnloadingProfile Profile { get; set; }

        public virtual CRFInEvent CRFInEvent { get; set; }
        public long EventID { get; set; }
        [ForeignKey("EventID")]
        public virtual Event Event { get; set; }
        public long CRFID { get; set; }
        [ForeignKey("CRFID")]
        public virtual CRF CRF { get; set; }
        public long ItemID { get; set; }
        [ForeignKey("ItemID")]
        public virtual CRF_Item Item { get; set; }

    }
}