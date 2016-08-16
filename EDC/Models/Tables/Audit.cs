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
        public DateTime Date { get; set; }//Дата и время
        public string RecordType { get; set; }//Тип записи (query, запись, пользователь и т.д.)
        public string ChangesType { get; set; }//тип изменений(удаление, добавление, изменение)
        public string OldValue { get; set; }//старое значение
        public string NewValue { get; set; }//новое значение

    }
}