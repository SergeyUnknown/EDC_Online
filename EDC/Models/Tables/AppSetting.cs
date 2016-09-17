using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EDC.Models
{
    public class AppSetting
    {
        [Key]
        public string AppSettingID { get; set; }
        public string Value { get; set; }
    }
}