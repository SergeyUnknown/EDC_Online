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

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                LoadSubjects();
            }
            LoadDTDataItem();
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
                return (int)Math.Ceiling((decimal)SR.SelectAll().Count() / pageSize);
            }
        } 

        void LoadSubjects()
        {
            _subjects = SR.SelectAll().ToList();
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
            SR.Delete(_subjects[e.RowIndex].SubjectID);
            SR.Save();
            LoadSubjects();
        }

        protected void dtInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            pageSize = dtInfo.DropDownSelectedValue;
            int maxRecordsOnPage = (CurrentPage * pageSize) > recordCount ? recordCount : (CurrentPage * pageSize);

            //EntryesInfo.Text = string.Format(Localization.Records, ((CurrentPage - 1) * pageSize + 1), maxRecordsOnPage, recordCount);
            LoadDTDataItem();
        }
    }
}