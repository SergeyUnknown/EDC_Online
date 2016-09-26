using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDC.Models
{
    public class SubjectsItem
    {

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
        public virtual CRF_Item Item { get; set; }

        public int IndexID { get; set; }    //если без группы индекс -1, если в группе >=1; показывает номер строки
        
        public bool IsGrouped { get; set; } //В группе
        public string Value { get; set; }   //значение

        public string CreatedBy { get; set; } //UserName

        public virtual List<Note> Notes { get; set; }

        public virtual List<Audit> Audits { get; set; } //Аудит
    }
}