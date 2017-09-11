using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EDC.Models.Repository;

namespace EDC.Pages.Event
{
    public partial class Events : BasePage
    {
        EventRepository ER = new EventRepository();
        static int pageSize = 50;
        static int recordCount = 0;

        static List<EDC.Models.Event> events = new List<Models.Event>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(User.IsInRole(Core.Roles.Administrator.ToString()) || User.IsInRole(Core.Roles.Data_Manager.ToString())))
                Response.Redirect("~/");
            if (User.IsInRole(Core.Roles.Data_Manager.ToString()))
            {
                dtInfo.ViewButton = false;
                gvEvents.Columns[gvEvents.Columns.Count - 1].Visible = false;
            }
            if(!IsPostBack)
            {
                LoadEvents();
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
                return (int)Math.Ceiling((decimal)ER.SelectAll().Count() / pageSize);
            }
        }

        void LoadDTDataItem()
        {
            DownTableDataItem dtDataItem = new DownTableDataItem(
                Resources.LocalizedText.Add,
                "~/Events/Add",
                CurrentPage,
                MaxPageCount,
                "~/Events/",
                Core.Core.DropDownListItems25,
                pageSize);

            dtInfo.DataItem = dtDataItem;
            dtInfo.DataBind();
        }

        void LoadEvents()
        {
            events = ER.SelectAll().OrderBy(x=>x.Position).ToList();
            gvEvents.DataSource = events;
            gvEvents.DataBind();
        }

        protected void dtInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            pageSize = dtInfo.DropDownSelectedValue;
            int maxRecordsOnPage = (CurrentPage * pageSize) > recordCount ? recordCount : (CurrentPage * pageSize);

            //EntryesInfo.Text = string.Format(Localization.Records, ((CurrentPage - 1) * pageSize + 1), maxRecordsOnPage, recordCount);
            LoadDTDataItem();
        }

        protected void gvEvents_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int pos = events[e.RowIndex].Position;
                List<Models.Event> editedEvents = ER.GetManyByFilter(x => x.Position > pos).OrderBy(x => x.Position).ToList();

                for (int i = 0; i < editedEvents.Count; i++)
                {
                    editedEvents[i].Position = pos+i;
                    ER.Update(editedEvents[i]);
                }

                ER.Delete(events[e.RowIndex].EventID);

                ER.Save();
            }
            catch(Exception error)
            {
                //вывод
            }
            LoadEvents();
        }

        protected void gvEvents_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            Response.Redirect("~/Events/Configuration/"+events[e.NewSelectedIndex].EventID);
        }

        protected void gvEvents_RowEditing(object sender, GridViewEditEventArgs e)
        {
            Response.Redirect("~/Events/Edit/"+events[e.NewEditIndex].EventID);
        }

        protected void gvEvents_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType== DataControlRowType.DataRow)
            {
                DropDownList ddlPos = (DropDownList)e.Row.FindControl("ddlPos");
                for (int i = 0; i < events.Count;i++ )
                {
                    ddlPos.Items.Add((i + 1).ToString());
                }
                ddlPos.SelectedIndex = e.Row.RowIndex;

            }
        }


        protected void ddlPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.Parent.Parent;
            Models.Event _event = ER.SelectByID(events[row.RowIndex].EventID);
            int newPos = ddl.SelectedIndex + 1;
            if (newPos < _event.Position)
            {
                List<Models.Event> editedEvents = ER.GetManyByFilter(x => x.Position >= newPos && x.Position <= _event.Position).OrderBy(x => x.Position).ToList();

                for (int i = 0; i < editedEvents.Count; i++)
                {
                    editedEvents[i].Position = newPos + (i + 1);
                    ER.Update(editedEvents[i]);
                }
            }
            else
            {
                List<Models.Event> editedEvents = ER.GetManyByFilter(x => x.Position <= newPos && x.Position >= _event.Position).OrderBy(x => x.Position).ToList();

                for (int i = 0; i < editedEvents.Count; i++)
                {
                    editedEvents[editedEvents.Count-i-1].Position = newPos - (i + 1);
                    ER.Update(editedEvents[editedEvents.Count - i-1]);
                }
            }

            _event.Position = newPos;
            try
            {
                ER.Update(_event);
                ER.Save();
            }
            catch(Exception error)
            {
                //вывод
            }
            LoadEvents();
        }
    }
}