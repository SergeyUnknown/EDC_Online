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
        public string Name { get; set; }
        public string Identifier { get; set; }
        public DateTime DateCreation { get; set; }
        public string CreatedBy { get; set; }
        public int Position { get; set; }
        public List<EventCRF> CRFs { get; set; }
    }
}