using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDC.Models
{
    public class Token : Core.Rule.IToken
    {
        public long TokenID { get; set; }
        public string Value { get; set; }

        public EDC.Core.Rule.TokenType Type { get; set; }

        [ForeignKey("Rule")]
        public long RuleID { get; set; }
        public Rule Rule { get; set; }
    }
}