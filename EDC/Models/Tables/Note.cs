using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDC.Models
{
    public class Note
    {
        public int NoteID { get; set; }
        public Guid UserID { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public long EventID { get; set; }
        [ForeignKey("EventID")]
        public virtual Event Event { get; set; }
        public long CRFID { get; set; }
        [ForeignKey("CRFID")]
        public virtual CRF CRF { get; set; }
        public long? GroupID { get; set; }
        [ForeignKey("GroupID")]
        public virtual CRF_Group Group { get; set; }
        public long? ItemID { get; set; }
        [ForeignKey("ItemID")]
        public virtual CRF_Item Item { get; set; }
    }
}