using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDC.Core
{
    public enum Roles
    {
        Administrator, //администратор
        Data_Manager,  //Data manager
        Monitor,       //Монитор
        Principal_Investigator,  //Главный Исследователь
        Investigator, //исследователь/координатор
        Auditor         //Аудитор
    }
}