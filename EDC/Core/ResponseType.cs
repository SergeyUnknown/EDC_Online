using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDC.Core
{
    public enum ResponseType
    {
        None,
        Text,
        Textarea,
        SingleSelect,
        Radio,
        MultiSelect,
        Checkbox,
        Calculation,
        GroupCalculation,
        File
    }
}