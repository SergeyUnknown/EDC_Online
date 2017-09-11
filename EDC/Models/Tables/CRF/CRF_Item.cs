using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDC.Models
{
    public class CRF_Item
    {
        public long CRF_ItemID { get; set; }
        public string Identifier { get; set; }
        public long CRFID { get; set; }
        [ForeignKey("CRFID")]
        public virtual CRF CRF { get; set; }
        public string Name { get; set; }
        public string DescriptionLabel { get; set; }
        public string LeftItemText { get; set; }
        public string Units { get; set; }
        public string RightItemText { get; set; }
        public long SectionID { get; set; }
        [ForeignKey("SectionID")]
        public virtual CRF_Section Section { get; set; }

        public long? GroupID { get; set; }
        [ForeignKey("GroupID")]
        public virtual CRF_Group Group { get; set; }
        public bool Ungrouped { get; set; } //без группы
        public string Header { get; set; }
        public string Subheader { get; set; }
        //public string ParentItem { get; set; }
        public int ColumnNumber { get; set; }
        public int PageNumber { get; set; } //??????????????????????
        public int QuestionNumber { get; set; }
        public Core.ResponseType ResponseType { get; set; }
        public string ResponseLabel { get; set; }
        public string ResponseOptionText { get; set; }
        public string ResponseValuesOrCalculation { get; set; }
        public string ResponseLayout { get; set; }
        public string DefaultValue { get; set; }
        public Core.DataType DataType { get; set; }
        public int WidthDecimal { get; set; }
        public string Validation { get; set; }
        public string ValidationErrorMessage { get; set; }
        public bool PHI { get; set; }
        public bool Required { get; set; }

        public virtual List<Query> Notes { get; set; }
        public virtual List<SubjectsItem> SubjectItems { get; set; }

        public virtual List<Rule> Rules { get; set; }
    }
}