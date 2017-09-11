using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EDC.Models.Repository;

namespace EDC.Pages.CRF
{
    public partial class CRFs : BasePage
    {
        CRFRepository CRFR = new CRFRepository();
        static int pageSize = 50;
        static int recordCount = 0;
        
        static List<EDC.Models.CRF> crfs = new List<Models.CRF>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(User.IsInRole(Core.Roles.Administrator.ToString()) || User.IsInRole(Core.Roles.Data_Manager.ToString())))
                Response.Redirect("~/");
            if (User.IsInRole(Core.Roles.Data_Manager.ToString()))
                dtInfo.ViewButton = false;
            if (!IsPostBack)
            {
                LoadCRFs();
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
                return (int)Math.Ceiling((decimal)CRFR.SelectAll().Count() / pageSize);
            }
        }

        void LoadDTDataItem()
        {
            DownTableDataItem dtDataItem = new DownTableDataItem(
                Resources.LocalizedText.Add,
                "~/CRFs/Add",
                CurrentPage,
                MaxPageCount,
                "~/CRFs/",
                Core.Core.DropDownListItems25,
                pageSize);

            dtInfo.DataItem = dtDataItem;
            dtInfo.DataBind();
        }

        void LoadCRFs()
        {
            crfs = CRFR.SelectAll().OrderBy(x=>x.RussianName).ThenBy(x=>x.Name).ToList();
            gvCRFs.DataSource = crfs;
            gvCRFs.DataBind();
        }

        protected void gvCRFs_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Models.CRF _crf = CRFR.SelectByID(crfs[e.RowIndex].CRFID);
            try
            {
                CRFR.Delete(_crf.CRFID);
                CRFR.Save();
                System.IO.File.Delete(_crf.FilePath);
            }
            catch(Exception error)
            {
                Response.Write(error.Message);
            }

            LoadCRFs();
        }

        protected void gvCRFs_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            Response.Redirect("~/CRFs/View/"+crfs[e.NewSelectedIndex].CRFID);
        }

        protected void dtInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            pageSize = dtInfo.DropDownSelectedValue;
            int maxRecordsOnPage = (CurrentPage * pageSize) > recordCount ? recordCount : (CurrentPage * pageSize);

            LoadDTDataItem();
        }
    }
}