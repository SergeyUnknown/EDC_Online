using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDC.Models
{
    public class AuditEditReason
    {
        public int AuditEditReasonID { get; set; } //ID
        public Guid UserID { get; set; }//ID пользователя
        public string UserName { get; set; } //Логин пользователя
        public DateTime ActionDate { get; set; }//Дата и время
        public string EditReason { get; set; } //причина редактирования

        public virtual Subject Subject { get; set; }
        public virtual SubjectsEvent SubjectEvent { get; set; } //Изменяемое событие
        public virtual SubjectsCRF SubjectCRF { get; set; } //изменяемое ИРК

        //Subject
        public long? SubjectID { get; set; }
        //
        //SubjectEvent
        public long? EventID { get; set; }
        //
        //SubjectCRF
        public long? CRFID { get; set; }
        //

    }

    
}