using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EDC.Pages.Rule
{
    public partial class Rules : BasePage
    {
        Models.Repository.RuleRepository RR = new Models.Repository.RuleRepository();
        static int pageSize = 50;
        static int recordCount = 0;
        List<Models.Rule> rules
        {
            get 
            {
                return (List<Models.Rule>)Session["rRules"] ?? new List<Models.Rule>();
            }
            set
            {
                Session["rRules"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(User.IsInRole(Core.Roles.Administrator.ToString()) || User.IsInRole(Core.Roles.Data_Manager.ToString())))
                Response.Redirect("~/");
            if (User.IsInRole(Core.Roles.Data_Manager.ToString()))
                dtInfo.ViewButton = false;
            if (!IsPostBack)
            {
                LoadRules();
            }
            LoadDTDataItem();
        }

        private int GetPageFromRequest()
        {
            int page;
            string reqValue = (string)RouteData.Values["page"] ?? Request.QueryString["page"];
            return reqValue != null && int.TryParse(reqValue, out page) && page > 0 ? page : 1;
        }

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
                return (int)Math.Ceiling((decimal)RR.SelectAll().Count() / pageSize);
            }
        }

        void LoadRules()
        {
            rules = RR.SelectAll().ToList();
            gvRules.DataSource = rules;
            gvRules.DataBind();

        }

        void LoadDTDataItem()
        {
            DownTableDataItem dtDataItem = new DownTableDataItem(
                Resources.LocalizedText.Add,
                "~/Rules/Add",
                CurrentPage,
                MaxPageCount,
                "~/Rules/",
                Core.Core.DropDownListItems25,
                pageSize);

            dtInfo.DataItem = dtDataItem;
            dtInfo.DataBind();
        }

        protected void gvRules_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            RR.Delete(rules[e.RowIndex].RuleID);
            RR.Save();
            LoadRules();
        }

    }
}