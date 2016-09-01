﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDC.Models
{
    public class SubjectsItem
    {
        public long SubjectsItemID { get; set; }
        public long? ItemID { get; set; }
        [ForeignKey("ItemID")]
        public CRF_Item Item { get; set; }
        public long? SubjectID { get; set; }
        [ForeignKey("SubjectID")]
        public virtual Subject Subject { get; set; }
        public SubjectsCRF SubjectsCRF { get; set; }
        //Ниже внешний ключ
        public long SubjectsEventID { get; set; }
        public long CRFID { get; set; }
        //end
        public string Value { get; set; }
    }
}