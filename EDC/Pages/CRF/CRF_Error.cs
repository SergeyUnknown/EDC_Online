using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDC.Pages.CRF
{
    public class CRF_Error
    {
        public string SectionName { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public string ErrorMessage { get; set; }

        public CRF_Error(string SectionName,
            int Row,
            int Column,
            string ErrorMessage)
        {
            this.SectionName = SectionName;
            this.Row = Row;
            this.Column = Column;
            this.ErrorMessage = ErrorMessage;
        }
    }
}