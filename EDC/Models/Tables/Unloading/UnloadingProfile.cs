using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDC.Models
{
    public class UnloadingProfile
    {
        public int UnloadingProfileID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public bool ReadyToUnloading { get; set; }
        public DateTime CreationDate { get; set; }
        public string CrfStatus { get; set; }

        public virtual List<UnloadingItem> Items { get; set; }
        public virtual List<UnloadingProfileCenter> Centers { get; set; }
        public virtual List<UnloadingFile> Files { get; set; }

    }
}