using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDC.Models
{
    public class UserProfile
    {
        public long UserProfileID { get; set; } //ID
        public Guid UserID { get; set; }//User ID
        public string Name { get; set; } //Имя
        public string LastName { get; set; }//Фамилия
        public string Phone { get; set; }//Телефон
    }
}