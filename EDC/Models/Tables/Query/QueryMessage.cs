using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDC.Models
{
    public class QueryMessage
    {
        public long QueryMessageID { get; set; }
        public string Text { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime CreationDate { get; set; } //дата создания

        public long QueryID { get; set; }
        [ForeignKey("QueryID")]
        public Query Query { get; set; }
    }
}