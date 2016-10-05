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
        /// <summary>
        /// Ввод данных завершен
        /// </summary>
        [DefaultValue(false)]
        public bool IsEnd { get; set; }
        public string IsEndBy { get; set; }

        /// <summary>
        /// Подписано
        /// </summary>
        [DefaultValue(false)]
        public bool IsApprove { get; set; }
        public string IsApprovedBy { get; set; }

        /// <summary>
        /// Сверка завершена
        /// </summary>
        [DefaultValue(false)]
        public bool IsCheckAll { get; set; } //сверка завершена
        public string IsCheckAllBy { get; set; }
        [DefaultValue(false)]
        public bool IsStopped { get; set; } //ввод данных остановлен
        [DefaultValue(false)]
        public bool IsLock { get; set; } //заблокировано
        [DefaultValue(false)]
        public bool IsDeleted { get; set; } //удалено
        public virtual List<Audit> Audits { get; set; } //Аудит
    }
}