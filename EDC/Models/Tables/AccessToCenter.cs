using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDC.Models
{
    public class AccessToCenter
    {
        public Guid UserID { get; set; }
        [ForeignKey("UserID")]
        public virtual UserProfile UserProfile { get; set; }
        public long MedicalCenterID { get; set; }
        [ForeignKey("MedicalCenterID")]
        public virtual MedicalCenter MedicalCenter { get; set; }

        public string CreatedBy { get; set; }
    }
}