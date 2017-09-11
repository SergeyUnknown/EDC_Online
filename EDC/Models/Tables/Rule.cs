using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDC.Models
{
    public class Rule
    {
        public long RuleID { get; set; }
        public string Name { get; set; }
        public DateTime AddedDate { get; set; }
        public string PathToFile { get; set; }
        public string Target { get; set; }
        public string OID { get; set; }
        public string ErrorMessage { get; set; }
        public string Expression { get; set; }
        public bool IfExpressionEvaluates { get; set; }

        [ForeignKey("Event")]
        public long? EventID { get; set; }
        public virtual Event Event { get; set; }

        [ForeignKey("CRF")]
        public long CRFID { get; set; }
        public virtual CRF CRF { get; set; }

        [ForeignKey("Group")]
        public long GroupID { get; set; }
        public virtual CRF_Group Group { get; set; }


        [ForeignKey("Item")]
        public long ItemID { get; set; }
        public virtual CRF_Item Item { get; set; }

        public virtual List<Models.Token> Tokens { get; set; }


    }
}