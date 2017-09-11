using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace EDC.Models
{
    public class SubjectsCRF
    {
        
        public long SubjectID { get; set; }
        [ForeignKey("SubjectID")]
        public virtual Subject Subject { get; set; }
        public long EventID { get; set; }
        [ForeignKey("EventID")]
        public virtual Event Event { get; set; }
        public long CRFID { get; set; }
        [ForeignKey("CRFID")]
        public virtual CRF CRF { get; set; }

        /// <summary>
        /// Ввод данных начат
        /// </summary>
        [DefaultValue(false)]
        public bool IsStart { get; set; }
        public string IsStartBy { get; set; }
        public DateTime? IsStartDate { get; set; }
        /// <summary>
        /// Ввод данных завершен
        /// </summary>
        [DefaultValue(false)]
        public bool IsEnd { get; set; }
        public string IsEndBy { get; set; }
        public DateTime? IsEndDate { get; set; }

        /// <summary>
        /// Подписано
        /// </summary>
        [DefaultValue(false)]
        public bool IsApproved { get; set; }
        public string IsApprovedBy { get; set; }
        public DateTime? IsApprovedDate { get; set; }

        /// <summary>
        /// Сверка проведена
        /// </summary>
        [DefaultValue(false)]
        public bool IsCheckAll { get; set; }
        public string IsCheckAllBy { get; set; }
        public DateTime? IsCheckAllDate { get; set; }

        [DefaultValue(false)]
        public bool IsStopped { get; set; } //ввод данных остановлен
        public string IsStoppedBy { get; set; }
        public DateTime? IsStoppedDate { get; set; }
        [DefaultValue(false)]
        public bool IsLock { get; set; } //заблокировано
        public string IsLockBy { get; set; }
        public DateTime? IsLockDate { get; set; }
        
        [DefaultValue(false)]
        public bool IsDeleted { get; set; } //удалено
        public string IsDeletedBy { get; set; }
        public DateTime? IsDeletedDate { get; set; }
        public virtual List<Audit> Audits { get; set; } //Аудит
        public virtual List<AuditEditReason> EditReasons { get; set; } //Причины редактирования
        public virtual List<SubjectsItem> Items { get; set; } //Причины редактирования
    }
}