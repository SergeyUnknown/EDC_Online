using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDC.Models
{
    public class UnloadingFile
    {
        public int UnloadingFileID { get; set; }
        public string PathToFile { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string Type { get; set; }
        public int UnloadingProfileID { get; set; }
        [ForeignKey("UnloadingProfileID")]
        public UnloadingProfile UnloadingProfile { get; set; }
    }
}