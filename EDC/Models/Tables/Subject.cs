﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace EDC.Models
{
    public class Subject
    {
        public long SubjectID { get; set; }
        public string Number { get; set; }
        public virtual List<SubjectsEvent> Events { get; set; }
        public virtual List<SubjectsCRF> CRFs { get; set; }
        public virtual List<SubjectsItem> Items { get; set; }
        public virtual MedicalCenter MedicalCenter { get; set; }
        [ForeignKey("MedicalCenter")]
        public long MedicalCenterID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime InclusionDate { get; set; }
        public virtual List<Audit> Audits { get; set; } //Аудит
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
        public string IsDeletedBy { get; set; }
        public DateTime? IsDeletedDate { get; set; }


        [DefaultValue(false)]
        public bool IsStopped { get; set; } //ввод данных остановлен
        public string IsStoppedBy { get; set; }
        public DateTime? IsStoppedDate { get; set; }

        [DefaultValue(false)]
        public bool IsLock { get; set; } //заблокировано
        public string IsLockBy { get; set; }
        public DateTime? IsLockDate { get; set; }
    }
}