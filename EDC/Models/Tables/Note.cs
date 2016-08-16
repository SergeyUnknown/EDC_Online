using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDC.Models
{
    public class Note
    {
        public int NoteID { get; set; }
        public Guid UserID { get; set; }
        public string Head { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

    }
}