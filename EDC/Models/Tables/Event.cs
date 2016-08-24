using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDC.Models
{
    public class Event
    {
        public long EventID { get; set; }
        public string Name { get; set; } //название события
        public string Identifier { get; set; } //Идентификатор
        public DateTime DateCreation { get; set; } //Дата создания
        public string CreatedBy { get; set; } //Кем создано
        public int Position { get; set; } //Позиция в общем списке Событий
        public bool Required { get; set; } //Обязательно ли данное событие
        public List<EventCRF> CRFs { get; set; } //ИРК внутри данного события
        public List<EventSubject> Subjects { get; set; } //Субъекты задействованные в данном событии
    }
}