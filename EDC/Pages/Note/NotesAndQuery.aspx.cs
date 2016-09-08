using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EDC.Pages.Note
{
    public partial class NotesAndQuery : System.Web.UI.Page
    {
        static Models.Repository.NoteRepository NR = new Models.Repository.NoteRepository();

        static int pageSize = 50;
        static int recordCount = 0;
        List<Models.Note> notes;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                LoadNotes();
                LoadDTDataItem();
            }
        }

        void LoadNotes()
        {
            if(User.IsInRole(Core.Roles.Data_Manager.ToString()) || User.IsInRole(Core.Roles.Administrator.ToString()))
            {
                notes = NR.SelectAll().OrderBy(x => x.NoteID).Skip((CurrentPage - 1) * pageSize).Take(pageSize).ToList();
            }
            else if(User.IsInRole(Core.Roles.Investigator.ToString()))
            {
                string currentUserName = User.Identity.Name;
                notes = NR.GetManyByFilter(x => x.ForUser == currentUserName).OrderBy(x => x.NoteID).Skip((CurrentPage - 1) * pageSize).Take(pageSize).ToList();
            }
            gvNotes.DataSource = notes;
            gvNotes.DataBind();
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
                return (int)Math.Ceiling((decimal)NR.SelectAll().Count() / pageSize);
            }
        }

        protected void dtInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            pageSize = dtInfo.DropDownSelectedValue;
            int maxRecordsOnPage = (CurrentPage * pageSize) > recordCount ? recordCount : (CurrentPage * pageSize);

            LoadDTDataItem();
        }

        void LoadDTDataItem()
        {
            string pageInfo = String.Format(Localization.Page, CurrentPage, MaxPageCount);
            DownTableDataItem dtDataItem = new DownTableDataItem(
                CurrentPage,
                MaxPageCount,
                "~/Notes/",
                Core.DropDownListItems25,
                pageSize, pageInfo);

            dtInfo.DataItem = dtDataItem;
            dtInfo.DataBind();
        }
    }
}