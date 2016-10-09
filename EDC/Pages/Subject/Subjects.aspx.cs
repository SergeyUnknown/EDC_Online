using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EDC.Pages.Subject
{
    public partial class Subjects : System.Web.UI.Page
    {

        Models.Repository.SubjectRepository SR = new Models.Repository.SubjectRepository();
        static int pageSize = 50;
        static int recordCount = 0;
        static List<Models.Subject> _subjects;
        bool canEdit = false;
        bool canDelete = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            //если Администратор, Исследователь, Главный исследователь
            if (User.IsInRole(Core.Roles.Administrator.ToString()) || User.IsInRole(Core.Roles.Investigator.ToString()) || User.IsInRole(Core.Roles.Principal_Investigator.ToString()))
            {
                canEdit = true;
                dtInfo.ViewButton = true;
                if(User.IsInRole(Core.Roles.Administrator.ToString()))
                {
                    canDelete = true;
                }
            }
            LoadDTDataItem(); 

            if(!IsPostBack)
            {
                LoadSubjects();
            }

        }

        private int GetPageFromRequest()
        {
            int page;
            string reqValue = (string)RouteData.Values["page"] ?? Request.QueryString["page"];
            return reqValue != null && int.TryParse(reqValue, out page) && page > 0 ? page : 1;
        }

        //номер текущей страницы
        protected int CurrentPage
        {
            get
            {
                int page;
                page = GetPageFromRequest();
                return page > MaxPageCount ? MaxPageCount : page;
            }
        }

        //максимальный номер страницы
        protected int MaxPageCount
        {
            get
            {

                return (int)Math.Ceiling((decimal)SR.SelectAllForUser(User).Count() / pageSize);
            }
        } 

        void LoadSubjects()
        {

            if (User.IsInRole(Core.Roles.Administrator.ToString()) || User.IsInRole(Core.Roles.Data_Manager.ToString()))
                _subjects = SR.SelectAllForUser(User).ToList();
            else
                _subjects = SR.GetManyByFilter(x=>!x.IsDeleted).ToList();
            gvSubjects.DataSource = _subjects;
            gvSubjects.DataBind();
        }

        void LoadDTDataItem()
        {
            string pageInfo = String.Format(Localization.Page, CurrentPage, MaxPageCount);
            DownTableDataItem dtDataItem = new DownTableDataItem(
                "Добавить субъекта",
                "~/Subjects/Add",
                CurrentPage,
                MaxPageCount,
                "~/Subjects",
                Core.DropDownListItems25,
                pageSize, pageInfo);

            dtInfo.DataItem = dtDataItem;
            dtInfo.DataBind();
        }

        protected void gvSubjects_RowEditing(object sender, GridViewEditEventArgs e)
        {
            Response.Redirect("~/Subjects/Edit/" + _subjects[e.NewEditIndex].SubjectID);
        }

        protected void gvSubjects_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Models.Subject s = SR.SelectByID(_subjects[e.RowIndex].SubjectID);

            SR.Delete(_subjects[e.RowIndex].SubjectID,User);
            SR.Save();
            LoadSubjects();
        }

        protected void dtInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            pageSize = dtInfo.DropDownSelectedValue;
            int maxRecordsOnPage = (CurrentPage * pageSize) > recordCount ? recordCount : (CurrentPage * pageSize);

            LoadDTDataItem();
        }

        protected void gvSubjects_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                //Редактировать
                Control _control = e.Row.Cells[6].Controls[0];
                _control.Visible = canEdit;

                //Удалять
                _control = e.Row.Cells[7].Controls[0];
                if (_subjects[e.Row.RowIndex].IsDeleted)
                    _control.Visible = false;
                else
                    _control.Visible = canDelete;
            }
        }

        protected string IsDeleted(string number, bool isDeleted)
        {
            if (isDeleted)
                return number + " - Удалён";
            else
                return number;
        }
    }
}