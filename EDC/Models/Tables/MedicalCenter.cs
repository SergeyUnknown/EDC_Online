using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EDC.Models
{
    public class MedicalCenter
    {
        [Key]
        public long MedialCenterID { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Phone { get; set; }
        public string PrincipalInvestigator { get; set; }
        public virtual List<Subject> Subjects { get; set; }

        public virtual List<Note> Notes { get; set; }
    }
}