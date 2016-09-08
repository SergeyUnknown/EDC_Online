using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDC.Models
{
    public class Note
    {
        public long NoteID { get; set; }

        public Core.QueryStatus Status { get; set; } //статус (новой, обновленное, закрытое, заметка)

        public Core.NoteType Type { get; set; } //тип (заметка или квери)
        public string ForUser { get; set; } //для кого
        public string FromUser { get; set; } //от кого

        public string Header { get; set; } //заголовок
        public string Text { get; set; } //текст
        public DateTime CreationDate { get; set; } //дата создания


        public Models.SubjectsItem SubjectItem { get; set; } //итем

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


        public long? PreviousNoteID { get; set; }
        [ForeignKey("PreviousNoteID")]
        public Note PreviousNote { get; set; }

        public long? MedicalCenterID { get; set; }
        [ForeignKey("MedicalCenterID")]
        public virtual MedicalCenter MedicalCenter { get; set; }
    }
}