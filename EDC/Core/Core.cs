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
            Investigator, //исследователь/координатор
            Auditor         //Аудитор
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
                case "Data_Manager": return "Дата менеджер";
                case "Monitor": return "Монитор";
                case "Principal_Investigator": return "Главный исследователь";
                case "Investigator": return "Координатор/Исследователь";
                case "Auditor": return "Аудитор";
                default: return role;
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

        public static List<string> ddlItemsYesNo
        {
            get
            {
                List<string> items = new List<string>();
                items.Add("Выберите значение...");
                items.Add("Да");
                items.Add("Нет");
                return items;
            }
        }

        public enum QueryStatus
        {
            New,
            Updated,
            Closed,
            Note
        }

        public static string GetQueryStatusRusName(QueryStatus item)
        {
            switch(item)
            {
                case QueryStatus.Closed: return "Закрыта";
                case QueryStatus.New: return "Новая";
                case QueryStatus.Note: return "Заметка";
                case QueryStatus.Updated: return "Обновлена";
                default: return "";
            }
        }

        public enum NoteType
        {
            Query,
            Note
        }

        public enum AppStatus
        {
            Design,
            Enable,
            Disable
        }

        /// <summary>
        /// Статус приложения
        /// </summary>
        public const string APP_STATUS = "appStatus";
        /// <summary>
        /// Название исследования
        /// </summary>
        public const string STUDY_NAME = "studyName";
        /// <summary>
        /// Номер протокола
        /// </summary>
        public const string STUDY_PROTOCOL = "protocolID";

        public enum AuditActionType
        {
            Subject,
            SubjectParam,
            SubjectEvent,
            SubjectCRF,
            SubjectItem,
            User
        }

        public static string GetAuditActionTypeRusName(AuditActionType item)
        {
            switch (item)
            {
                case AuditActionType.Subject: return "Субъект";
                case AuditActionType.SubjectParam: return "Параметр Субъекта";
                case AuditActionType.SubjectEvent: return "Событие Субъекта";
                case AuditActionType.SubjectCRF: return "ИРК Субъекта";
                case AuditActionType.SubjectItem: return "Поле данных";
                case AuditActionType.User: return "Пользователь";
                default: return"";
            }
        }

        public enum AuditChangesType
        {
            Create,
            Update,
            Delete
        }

        public static string GetAuditChangesTypeRusName(AuditChangesType item)
        {
            switch (item)
            {
                case AuditChangesType.Create: return "Создание";
                case AuditChangesType.Delete: return "Удаление";
                case AuditChangesType.Update: return "Обновление";
                default: return "";
            }
        }
    }
}