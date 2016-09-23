using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDC.Models
{
    public class Audit
    {
        public int AuditID { get; set; } //ID
        public string PageURL { get; set; }//Страница, на которой произошло действие
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
        public long? SubjectEventID { get; set; }
        public long? EventID { get; set; }
        //
        //SubjectCRF
        public long? SubjectCRFSubjectID { get; set; }
        public long? SubjectCRFEventID { get; set; }
        public long? CRFID { get; set; }
        //

        //SubjectItem
        public long? SubjectItemSubjectID { get; set; }
        public long? SubjectItemEventID { get; set; }
        public long? SubjectItemCRFID { get; set; }
        public long? ItemID { get; set; }
        public int? IndexID { get; set; }
        //
        public Audit()
        {
        }

        public Audit(string pageURL, Guid userID,
            string userName,DateTime actionDate, 
            Core.AuditActionType actionType,Core.AuditChangesType changesType,
            string changesUserName, string fieldname,
            string oldValue, string newValue)
        {
            this.PageURL = pageURL;
            this.UserID = userID;
            this.UserName = userName;
            this.ActionType = actionType;
            this.ChangesType = changesType;
            this.OldValue = oldValue;
            this.NewValue = newValue;
            this.ChangedUserName = changesUserName;
            this.FieldName = fieldname;
        }

    }

    
}