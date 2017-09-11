using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDC.Models
{
    public class Query
    {
        public long QueryID { get; set; }
        public Core.QueryStatus Status { get; set; } //статус (новой, обновленное, закрытое, заметка)
        public Core.NoteType Type { get; set; } //тип (заметка или квери)
        public string To { get; set; } //для кого
        public string From { get; set; } //от кого
        public string Header { get; set; } //заголовок
        public DateTime CreationDate { get; set; } //дата создания

        public string ClosedBy { get; set; }
        public string ClosedText { get; set; }
        public DateTime? ClosedDate { get; set; }

        public virtual List<QueryMessage> Messages { get; set; }

        public virtual Models.SubjectsItem SubjectItem { get; set; } //итем

        //Внешний ключ
        public long SubjectID { get; set; }
        [ForeignKey("SubjectID")]
        public virtual Subject Subject { get; set; }
        public long EventID { get; set; }
        [ForeignKey("EventID")]
        public virtual Event Event { get; set; }
        public long CRFID { get; set; }
        [ForeignKey("CRFID")]
        public virtual CRF CRF { get; set; }
        public long ItemID { get; set; }
        [ForeignKey("ItemID")]
        public virtual CRF_Item CRFItem { get; set; }
        public int IndexID { get; set; }
        /////////////////////////////////////////////

        public long? MedicalCenterID { get; set; }
        [ForeignKey("MedicalCenterID")]
        public virtual MedicalCenter MedicalCenter { get; set; }

    }
}