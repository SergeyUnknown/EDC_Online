using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDC.Models
{
    public class UnloadingProfileCenter
    {
        public int UnloadingProfileID { get; set; }
        [ForeignKey("UnloadingProfileID")]
        public virtual UnloadingProfile UnloadingProfile { get; set; }
        public long MedicalCenterID { get; set; }
        [ForeignKey("MedicalCenterID")]
        public virtual MedicalCenter MedicalCenter { get; set; }
    }
}