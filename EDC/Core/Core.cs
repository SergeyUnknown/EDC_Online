using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDC
{
    public static class Core
    {
        public enum Roles
        {
            Administrator, //администратор
            Data_Manager,  //Data manager
            Monitor,       //Монитор
            Principal_Investigator,  //Главный Исследователь
            Investigator //исследователь/координатор
        }

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

        public static ResponseType StringToResponseType(string str)
        {
            switch(str.ToLower())
            {
                case "text": return ResponseType.Text;
                case "textarea": return ResponseType.Textarea;
                case "singleselect": return ResponseType.SingleSelect;
                case "radio": return ResponseType.Radio;
                case "multiselect": return ResponseType.MultiSelect;
                case "checkbox": return ResponseType.Checkbox;
                case "calculation": return ResponseType.Calculation;
                case "groupcalculation": return ResponseType.GroupCalculation;
                case "file": return ResponseType.File;
                default: return ResponseType.None;
            }
        }

        public enum DataType
        {
            NONE,
            ST,
            INT,
            REAL,
            DATE,
            FILE
        }

        public static DataType StringToDataType(string str)
        {
            switch(str.ToUpper())
            {
                case "ST": return DataType.ST;
                case "INT": return DataType.INT;
                case "REAL": return DataType.REAL;
                case "DATE": return DataType.DATE;
                case "FILE": return DataType.FILE;
                default: return DataType.NONE;
            }
        }

        public static string GetRoleRusName(string role)
        {
            switch (role)
            {
                case "Administrator": return "Администратор";
                case "Data_Manager": return "Data Manager";
                case "Monitor": return "Монитор";
                case "Principal_Investigator": return "Главный исследователь";
                case "Investigator": return "Исследователь";
                default: return "";
            }
        }

        public static List<int> DropDownListItems25
        {
            get
            {
                List<int> items = new List<int>();
                items.Add(25);
                items.Add(50);
                items.Add(75);
                items.Add(100);
                items.Add(125);
                return items;
            }
        }

    }
}