using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDC.Models
{
    public class UserProfile
    {
        public Guid UserProfileID { get; set; }//User ID
        public string Name { get; set; } //Имя
        public string LastName { get; set; }//Фамилия
        public string Phone { get; set; }//Телефон

        public virtual List<AccessToCenter> MedicalCenters { get; set; }

        public long? CurrentCenterID { get; set; }

        public long? GetCurrentCenterID()
        {
            if (CurrentCenterID != null)
                return CurrentCenterID;
            else
                if(MedicalCenters.Count>0)
                {
                    CurrentCenterID = MedicalCenters.First().MedicalCenterID;
                    return CurrentCenterID;
                }
            return null;

        }
    }

    
}