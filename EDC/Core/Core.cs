using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDC.Core
{
    public static class Core
    {

        public static ResponseType StringToResponseType(string str)
        {
            switch(str.ToLower())
            {
                case "text": return ResponseType.Text;
                case "textarea": return ResponseType.Textarea;
                case "singleselect": return ResponseType.SingleSelect;
                case "single-select": return ResponseType.SingleSelect;
                case "radio": return ResponseType.Radio;
                case "multiselect": return ResponseType.MultiSelect;
                case "checkbox": return ResponseType.Checkbox;
                case "calculation": return ResponseType.Calculation;
                case "groupcalculation": return ResponseType.GroupCalculation;
                case "group-calculation": return ResponseType.GroupCalculation;
                case "file": return ResponseType.File;
                default: return ResponseType.None;
            }
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

        public static string QueriesStatus(QueryStatus status)
        {
            return GetQueryStatusRusName(status);
        }

        public static string GetQueryStatusRusName(QueryStatus item)
        {
            switch(item)
            {
                case QueryStatus.Closed: return "Закрыто";
                case QueryStatus.New: return "Новое";
                case QueryStatus.Note: return "Заметка";
                case QueryStatus.Updated: return "Обновлено";
                default: return "";
            }
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

        public static string GetAuditActionTypeRusName(AuditActionType item)
        {
            switch (item)
            {
                case AuditActionType.Subject: return "Субъект";
                case AuditActionType.SubjectParam: return "Параметр Субъекта";
                case AuditActionType.SubjectEvent: return "Событие Субъекта";
                case AuditActionType.SubjectCRF: return "ИРК Субъекта";
                case AuditActionType.SubjectCRFStatus: return "Статус ИРК";
                case AuditActionType.SubjectItem: return "Поле данных";
                case AuditActionType.User: return "Пользователь";
                default: return"";
            }
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

        public static List<Models.Query> FilterQueriesAccess(List<Models.Query> queries, System.Security.Principal.IPrincipal user)
        {
            string currentUserName = user.Identity.Name;
            if (user.IsInRole(Roles.Administrator.ToString()))
            {
                //Если администратор
            }
            else if (user.IsInRole(Roles.Data_Manager.ToString()))
            {
                //Если ДМ
            }
            else if (user.IsInRole(Roles.Monitor.ToString()))
            {
                //Если монитор
            }
            else if (user.IsInRole(Roles.Principal_Investigator.ToString()))
            {
                //Если Гл. Исследователь
            }
            else if (user.IsInRole(Roles.Investigator.ToString()))
            {
                //Если исследователь
                queries = queries.Where(x => x.To == currentUserName).ToList();
            }
            else if (user.IsInRole(Roles.Auditor.ToString()))
            {
                //Если Аудитор
                queries = queries.Where(x => x.From == currentUserName || x.To == currentUserName).ToList();
            }
            return queries;
        }

        public static bool HaveAccessToCenter(System.Security.Principal.IPrincipal user, long medicalCenterID)
        {
            Models.Repository.AccessToCenterRepository ATCR = new Models.Repository.AccessToCenterRepository();

            Guid userID = (Guid)System.Web.Security.Membership.GetUser(user.Identity.Name).ProviderUserKey;
            if (ATCR.GetManyByFilter(x => x.MedicalCenterID == medicalCenterID && x.UserID == userID).Count() > 0)
                return true;
            else
                return false;
            
        }

        public static List<Models.MedicalCenter> AccessToCenters(System.Security.Principal.IPrincipal user)
        {
            Models.Repository.AccessToCenterRepository ATCR = new Models.Repository.AccessToCenterRepository();
            Guid userID = (Guid)System.Web.Security.Membership.GetUser(user.Identity.Name).ProviderUserKey;

            return ATCR.GetManyByFilter(x=>x.UserID == userID).ToList().Select(x=>x.MedicalCenter).ToList();
        }

    }
}