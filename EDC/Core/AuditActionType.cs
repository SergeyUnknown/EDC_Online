using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDC.Core
{
    public enum AuditActionType
    {
        Subject,
        SubjectParam,
        SubjectEvent,
        SubjectCRF,
        SubjectCRFStatus,
        SubjectItem,
        User
    }
}