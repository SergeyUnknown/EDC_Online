using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDC.Models
{
    public class CRF
    {
        public long CRFID { get; set; }
        public string Identifier { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public string FilePath { get; set; }
        public virtual List<CRF_Group> Groups { get; set; }
        public virtual List<CRF_Section> Sections { get; set; }
        public virtual List<CRF_Item> Items { get; set; }

        public virtual List<CRFInEvent> Events { get; set; }

        public virtual List<SubjectsItem> Values { get; set; }

    }
}