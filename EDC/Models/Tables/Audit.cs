using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDC.Models
{
    public class Audit
    {
        public int AuditID { get; set; } //ID
        public Guid UserID { get; set; }//ID пользователя
        public string UserName { get; set; } //Логин пользователя
        public DateTime ActionDate { get; set; }//Дата и время
        public Core.AuditActionType ActionType { get; set; }//Тип записи 
        public Core.AuditChangesType ChangesType { get; set; }//тип изменений
        public string ChangedUserName { get; set; } //логин изменяемого пользователя
        public string FieldName { get; set; } //название изменяемого поля
        public string OldValue { get; set; }//старое значение
        public string NewValue { get; set; }//новое значение

        public virtual Subject Subject { get; set; }
        public virtual SubjectsEvent SubjectEvent { get; set; } //Изменяемое событие
        public virtual SubjectsCRF SubjectCRF { get; set; } //изменяемое ИРК
        public virtual SubjectsItem SubjectItem { get; set; } //изменяемый итем

        //Subject
        public long? SubjectID { get; set; }
        //
        //SubjectEvent
        public long? EventID { get; set; }
        //
        //SubjectCRF
<<<<<<< HEAD
=======

>>>>>>> origin/forstyle
        public long? CRFID { get; set; }
        //

        //SubjectItem
        public long? ItemID { get; set; }
        public int? IndexID { get; set; }
        //
        public Audit()
        {
        }

    }

    
}